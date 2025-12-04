namespace BFF_GameMatch.Proxies
{
    public class ProxyResponse
    {
        public int StatusCode { get; }
        public string Content { get; }
        public string ContentType { get; }

        public ProxyResponse(int statusCode, string content, string contentType)
        {
            StatusCode = statusCode;
            Content = content;
            ContentType = contentType;
        }
    }
}
