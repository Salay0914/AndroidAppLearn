namespace SyncfusionMAUIApp1.Services;

public sealed class QrScanService : IQrScanService
{
    public async Task<string?> ScanAsync(Page hostPage, CancellationToken cancellationToken = default)
    {
        // 无相机/无 ZXing 页面上下文时，允许手工录入演示。
        var manual = await hostPage.DisplayPromptAsync(
            "扫码/录入",
            "请输入标识器或二维码内容",
            accept: "确定",
            cancel: "取消",
            placeholder: "LY801-0001",
            maxLength: 128,
            initialValue: "LY801-0001").ConfigureAwait(true);
        return string.IsNullOrWhiteSpace(manual) ? null : manual.Trim();
    }
}
