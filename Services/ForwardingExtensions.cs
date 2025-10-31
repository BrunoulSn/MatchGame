using Microsoft.Net.Http.Headers;
using System.Net.Http.Headers;

namespace BFF_GameMatch.Services
{
    public static class ForwardingExtensions
    {
        public static HttpRequestMessage Forward(this HttpRequest req, HttpMethod method, string pathWithQuery)
        {
            var msg = new HttpRequestMessage(method, pathWithQuery);

            if (req.ContentLength > 0)
                msg.Content = new StreamContent(req.Body);

            foreach (var (k, v) in req.Headers)
            {
                if (k.Equals(HeaderNames.ContentType, StringComparison.OrdinalIgnoreCase)) continue;
                msg.Headers.TryAddWithoutValidation(k, v.ToArray());
            }

            if (req.Headers.TryGetValue(HeaderNames.Authorization, out var auth))
                msg.Headers.TryAddWithoutValidation(HeaderNames.Authorization, auth.ToString());

            if (req.Headers.TryGetValue(HeaderNames.ContentType, out var ct))
                msg.Content?.Headers.TryAddWithoutValidation(HeaderNames.ContentType, ct.ToString());

            return msg;
        }

        public static async Task<IResult> ProxyResult(HttpResponseMessage res)
        {
            var stream = await res.Content.ReadAsStreamAsync();
            var contentType = res.Content.Headers.ContentType?.ToString() ?? "application/octet-stream";
            var statusCode = (int)res.StatusCode;

            var result = Results.Stream(stream, contentType);
            return Results.StatusCode(statusCode) switch
            {
                { } => result,
                _ => Results.Stream(stream, contentType)
            };
        }



    }

}
