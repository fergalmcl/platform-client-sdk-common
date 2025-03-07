using {{=it.packageName}}.Extensions;
using System.Collections.Generic;


namespace {{=it.packageName }}.Client
{
public class HttpResponse : IHttpResponse
{
    public int StatusCode { get; set; }
    public string StatusDescription { get; set; }
    public Dictionary<string, string> Headers { get; set; } 
    public string Content { get; set; }
    public string ErrorMessage { get; set; }
    public byte[] RawBytes { get; set; }
}
}