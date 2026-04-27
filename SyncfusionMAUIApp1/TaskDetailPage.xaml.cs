using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text.Json;
using Microsoft.Maui.Controls;
using SyncfusionMAUIApp1.Contracts;
using SyncfusionMAUIApp1.Services;

namespace SyncfusionMAUIApp1;

[QueryProperty(nameof(TaskId), nameof(TaskId))]
public partial class TaskDetailPage : ContentPage
{
    private readonly ILocalDatabase _db = MauiProgram.Services.GetRequiredService<ILocalDatabase>();
    private readonly ISessionService _session = MauiProgram.Services.GetRequiredService<ISessionService>();
    private readonly IDb01Client _db01 = MauiProgram.Services.GetRequiredService<IDb01Client>();
    private readonly IGeolocationAppService _geo = MauiProgram.Services.GetRequiredService<IGeolocationAppService>();
    private readonly IQrScanService _qr = MauiProgram.Services.GetRequiredService<IQrScanService>();
    private readonly ISyncEngine _sync = MauiProgram.Services.GetRequiredService<ISyncEngine>();
    private readonly INfcReader _nfc = MauiProgram.Services.GetRequiredService<INfcReader>();

    private string _taskId = string.Empty;
    private InspectionTaskDto? _task;
    private readonly ObservableCollection<TaskMarkerRow> _rows = new();

    public string TaskId
    {
        get => _taskId;
        set
        {
            _taskId = Uri.UnescapeDataString(value ?? string.Empty);
            _ = LoadAsync();
        }
    }

    public TaskDetailPage()
    {
        InitializeComponent();
        MarkersView.ItemsSource = _rows;
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        _db01.ReadingReceived += OnDb01Reading;
        _nfc.TagReceived += OnNfcTag;
        _nfc.StartListening();
    }

    protected override void OnDisappearing()
    {
        _db01.ReadingReceived -= OnDb01Reading;
        _nfc.TagReceived -= OnNfcTag;
        _nfc.StopListening();
        base.OnDisappearing();
    }

    private async Task LoadAsync()
    {
        await _db.EnsureInitializedAsync().ConfigureAwait(true);
        var tasks = await _db.GetTaskDtosAsync().ConfigureAwait(true);
        _task = tasks.FirstOrDefault(t => t.Id == _taskId);
        if (_task is null)
        {
            TitleLabel.Text = "未找到任务";
            return;
        }

        TitleLabel.Text = _task.Title;
        await RefreshRowsAsync().ConfigureAwait(true);
    }

    private async Task RefreshRowsAsync()
    {
        if (_task is null)
            return;

        var done = (await _db.GetInspectionRecordsAsync(_task.Id).ConfigureAwait(true)).Select(r => r.MarkerId).ToHashSet(StringComparer.OrdinalIgnoreCase);
        _rows.Clear();
        foreach (var id in _task.RequiredMarkerIds)
        {
            _rows.Add(new TaskMarkerRow
            {
                MarkerId = id,
                IsDone = done.Contains(id),
            });
        }

        UpdateProgressLabel(done);
    }

    private void UpdateProgressLabel(HashSet<string>? doneSet = null)
    {
        if (_task is null)
            return;

        doneSet ??= _rows.Where(r => r.IsDone).Select(r => r.MarkerId).ToHashSet(StringComparer.OrdinalIgnoreCase);
        var total = _task.RequiredMarkerIds.Count;
        var ok = _task.RequiredMarkerIds.Count(id => doneSet.Contains(id));
        ProgressLabel.Text = $"已完成 {ok}/{total} 个标识器点位";
    }

    private void OnDb01Reading(object? sender, Db01Reading e) =>
        MainThread.BeginInvokeOnMainThread(() => _ = HandleDb01ReadingAsync(e));

    private async Task HandleDb01ReadingAsync(Db01Reading e)
    {
        if (_task is null)
            return;

        if (!_task.RequiredMarkerIds.Any(x => string.Equals(x, e.MarkerId, StringComparison.OrdinalIgnoreCase)))
            return;

        var already = (await _db.GetInspectionRecordsAsync(_task.Id).ConfigureAwait(true)).Any(r => string.Equals(r.MarkerId, e.MarkerId, StringComparison.OrdinalIgnoreCase));
        if (already)
            return;

        var geo = await _geo.GetCurrentLocationAsync().ConfigureAwait(true);
        var point = new InspectionPointDto
        {
            MarkerId = e.MarkerId,
            ReadAtUtc = DateTime.UtcNow,
            Latitude = geo?.Latitude,
            Longitude = geo?.Longitude,
            DepthMeters = e.DepthMeters,
        };

        await _db.SaveInspectionRecordAsync(_task.Id, point, _session.CurrentUserName ?? "unknown").ConfigureAwait(true);
        await RefreshRowsAsync().ConfigureAwait(true);
    }

    private async void OnNfcTag(object? sender, string e)
    {
        await TryRecordMarkerAsync(e).ConfigureAwait(true);
    }

    private async void OnSimulateDb01Clicked(object? sender, EventArgs e)
    {
        if (_task is null)
            return;

        var done = (await _db.GetInspectionRecordsAsync(_task.Id).ConfigureAwait(true)).Select(r => r.MarkerId).ToHashSet(StringComparer.OrdinalIgnoreCase);
        var pending = _task.RequiredMarkerIds.FirstOrDefault(id => !done.Contains(id));
        pending ??= _task.RequiredMarkerIds.FirstOrDefault();
        if (pending is null)
            return;

        await HandleDb01ReadingAsync(new Db01Reading
        {
            MarkerId = pending,
            DepthMeters = 1.1,
            SignalStrength = 88,
            DeviceBatteryPercent = 90,
            TimestampUtc = DateTime.UtcNow,
        }).ConfigureAwait(true);
    }

    private async void OnScanClicked(object? sender, EventArgs e)
    {
        var text = await _qr.ScanAsync(this).ConfigureAwait(true);
        if (!string.IsNullOrWhiteSpace(text))
            await TryRecordMarkerAsync(text.Trim()).ConfigureAwait(true);
    }

    private async Task TryRecordMarkerAsync(string markerId)
    {
        if (_task is null)
            return;

        if (!_task.RequiredMarkerIds.Any(x => string.Equals(x, markerId, StringComparison.OrdinalIgnoreCase)))
        {
            await DisplayAlert("提示", "该标识不在本工单计划内。", "确定").ConfigureAwait(true);
            return;
        }

        var already = (await _db.GetInspectionRecordsAsync(_task.Id).ConfigureAwait(true)).Any(r => string.Equals(r.MarkerId, markerId, StringComparison.OrdinalIgnoreCase));
        if (already)
        {
            await DisplayAlert("提示", "该点位已记录。", "确定").ConfigureAwait(true);
            return;
        }

        var geo = await _geo.GetCurrentLocationAsync().ConfigureAwait(true);
        var point = new InspectionPointDto
        {
            MarkerId = markerId,
            ReadAtUtc = DateTime.UtcNow,
            Latitude = geo?.Latitude,
            Longitude = geo?.Longitude,
        };

        await _db.SaveInspectionRecordAsync(_task.Id, point, _session.CurrentUserName ?? "unknown").ConfigureAwait(true);
        await RefreshRowsAsync().ConfigureAwait(true);
    }

    private async void OnPhotoClicked(object? sender, EventArgs e)
    {
        if (_task is null)
            return;

        try
        {
            var photo = await MediaPicker.Default.CapturePhotoAsync().ConfigureAwait(true);
            if (photo is null)
                return;

            var targetDir = Path.Combine(FileSystem.AppDataDirectory, "inspection_photos");
            Directory.CreateDirectory(targetDir);
            var fileName = $"{_task.Id}_{DateTime.UtcNow:yyyyMMddHHmmss}{photo.FileName}";
            var target = Path.Combine(targetDir, fileName);
            await using (var s = await photo.OpenReadAsync().ConfigureAwait(true))
            await using (var o = File.OpenWrite(target))
            {
                await s.CopyToAsync(o).ConfigureAwait(true);
            }

            var pending = _rows.FirstOrDefault(r => !r.IsDone)?.MarkerId ?? _task.RequiredMarkerIds.FirstOrDefault();
            if (pending is null)
                return;

            var geo = await _geo.GetCurrentLocationAsync().ConfigureAwait(true);
            var point = new InspectionPointDto
            {
                MarkerId = pending,
                ReadAtUtc = DateTime.UtcNow,
                Latitude = geo?.Latitude,
                Longitude = geo?.Longitude,
                PhotoPath = target,
            };

            await _db.SaveInspectionRecordAsync(_task.Id, point, _session.CurrentUserName ?? "unknown").ConfigureAwait(true);
            await RefreshRowsAsync().ConfigureAwait(true);
        }
        catch (Exception ex)
        {
            await DisplayAlert("拍照失败", ex.Message, "确定").ConfigureAwait(true);
        }
    }

    private async void OnSubmitClicked(object? sender, EventArgs e)
    {
        if (_task is null)
            return;

        var ok = await _db.IsTaskFullyScannedAsync(_task.Id).ConfigureAwait(true);
        if (!ok)
        {
            await DisplayAlert("无法提交", "尚有计划内标识器未读取。", "确定").ConfigureAwait(true);
            return;
        }

        var points = await _db.GetInspectionRecordsAsync(_task.Id).ConfigureAwait(true);
        var dto = new InspectionSubmitDto
        {
            TaskId = _task.Id,
            Operator = _session.CurrentUserName ?? "unknown",
            Points = points,
        };

        var json = JsonSerializer.Serialize(dto);
        await _db.EnqueueOutboxAsync("SubmitInspection", json).ConfigureAwait(true);
        var processed = await _sync.ProcessOutboxAsync().ConfigureAwait(true);
        await DisplayAlert("已提交", $"离线队列已处理 {processed} 条（演示：Mock 接口不落服务器）。", "确定").ConfigureAwait(true);
    }

    public sealed class TaskMarkerRow : INotifyPropertyChanged
    {
        private bool _isDone;
        private string _markerId = string.Empty;

        public string MarkerId
        {
            get => _markerId;
            set
            {
                if (_markerId == value)
                    return;
                _markerId = value;
                OnPropertyChanged(nameof(MarkerId));
                OnPropertyChanged(nameof(StatusGlyph));
            }
        }

        public bool IsDone
        {
            get => _isDone;
            set
            {
                if (_isDone == value)
                    return;
                _isDone = value;
                OnPropertyChanged(nameof(IsDone));
                OnPropertyChanged(nameof(StatusGlyph));
            }
        }

        public string StatusGlyph => IsDone ? "☑" : "☐";

        public event PropertyChangedEventHandler? PropertyChanged;

        private void OnPropertyChanged(string name) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
}
