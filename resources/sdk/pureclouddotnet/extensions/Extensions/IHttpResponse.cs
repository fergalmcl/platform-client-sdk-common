using System.Collections.Generic;

namespace {{=it.packageName }}.Extensions
{
    public interface IHttpResponse
    {
        int StatusCode { get; }
        string StatusDescription { get; }
        Dictionary<string, string> Headers { get; }
        string Content { get; }
        string ErrorMessage { get; }
        byte[] RawBytes { get; }
    }
}