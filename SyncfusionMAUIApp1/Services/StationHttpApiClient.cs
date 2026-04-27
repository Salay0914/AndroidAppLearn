using System.Net.Http.Json;
using SyncfusionMAUIApp1.Contracts;

namespace SyncfusionMAUIApp1.Services;

/// <summary>
/// 车站主机 REST 客户端骨架；待 OpenAPI 与基地址就绪后实现具体路径。
/// </summary>
public sealed class StationHttpApiClient
{
    private readonly HttpClient _http;

    public StationHttpApiClient(HttpClient http) => _http = http;

    public Task<HttpResponseMessage> PingAsync(CancellationToken cancellationToken = default) =>
        _http.GetAsync("api/health", cancellationToken);

    public Task<HttpResponseMessage> PostInspectionAsync(InspectionSubmitDto dto, CancellationToken cancellationToken = default) =>
        _http.PostAsJsonAsync("api/inspections", dto, cancellationToken);
}
