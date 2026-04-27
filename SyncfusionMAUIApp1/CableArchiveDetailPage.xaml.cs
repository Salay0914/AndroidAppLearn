using Microsoft.Maui.Controls;
using SyncfusionMAUIApp1.Services;

namespace SyncfusionMAUIApp1;

[QueryProperty(nameof(MarkerId), nameof(MarkerId))]
public partial class CableArchiveDetailPage : ContentPage
{
    private readonly ILocalDatabase _db = MauiProgram.Services.GetRequiredService<ILocalDatabase>();
    private string _markerId = string.Empty;

    public string MarkerId
    {
        get => _markerId;
        set
        {
            _markerId = Uri.UnescapeDataString(value ?? string.Empty);
            _ = LoadAsync();
        }
    }

    public CableArchiveDetailPage()
    {
        InitializeComponent();
    }

    private async Task LoadAsync()
    {
        await _db.EnsureInitializedAsync().ConfigureAwait(true);
        var dto = await _db.GetArchiveByMarkerAsync(_markerId).ConfigureAwait(true);
        if (dto is null)
        {
            MarkerLabel.Text = "未找到档案";
            return;
        }

        MarkerLabel.Text = dto.MarkerId;
        LocationLabel.Text = $"位置：{dto.Location}";
        DepthLabel.Text = $"埋深：{dto.BurialDepthMeters:F2} m";
        MaterialLabel.Text = $"管材：{dto.PipeMaterial}";
        DiameterLabel.Text = $"管径：{dto.DiameterMm:F0} mm";
        OwnerLabel.Text = $"单位：{dto.OwnerUnit}";
        NotesLabel.Text = $"备注（解密）：{dto.Notes}";
    }
}
