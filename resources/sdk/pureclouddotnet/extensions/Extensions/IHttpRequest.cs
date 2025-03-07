using System;
using System.Collections.Generic;

namespace {{=it.packageName }}.Extensions
{
    public interface IHttpRequest
    {
        string Url { get; }
        string Method { get; }
        List<Tuple<string, string>> QueryParams { get; }
        Dictionary<string, string> HeaderParams { get; }
        Dictionary<string, string> FormParams { get; }
        Dictionary<string, string> PathParams { get; }
        object PostBody { get; }
        string ContentType { get; }
    }
}