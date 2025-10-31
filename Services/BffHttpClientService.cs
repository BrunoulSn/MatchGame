using Polly;
using Polly.Extensions.Http;
using System.Net.Http.Headers;

namespace BFF_GameMatch.Services;

public static class BffHttpClientService
{
    public static void AddBackendHttpClient(this IServiceCollection services, IConfiguration cfg)
    {
        var baseUrl = cfg["Backend:BaseUrl"] ?? "http://localhost:5000/api";

        static IAsyncPolicy<HttpResponseMessage> Retry() =>
            HttpPolicyExtensions
                .HandleTransientHttpError()
                .OrResult(r => (int)r.StatusCode == 429)
                .WaitAndRetryAsync(new[]
                {
                    TimeSpan.FromMilliseconds(200),
                    TimeSpan.FromMilliseconds(500),
                    TimeSpan.FromSeconds(1)
                });

        static IAsyncPolicy<HttpResponseMessage> Breaker() =>
            HttpPolicyExtensions
                .HandleTransientHttpError()
                .CircuitBreakerAsync(5, TimeSpan.FromSeconds(30));

        services.AddHttpClient("backend", c =>
        {
            c.BaseAddress = new Uri(baseUrl);
            c.Timeout = TimeSpan.FromSeconds(15);
            c.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        })
        .AddPolicyHandler(Retry())
        .AddPolicyHandler(Breaker());
    }
}
