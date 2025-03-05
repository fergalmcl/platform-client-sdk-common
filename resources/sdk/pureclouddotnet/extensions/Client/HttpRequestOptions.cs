using System;
using System.Collections.Generic;
using RestSharp;
using System.IO;

namespace {{=it.packageName }}.Client
{
    public class HttpRequestOptions
    {
        public string Url { get; private set; }
        public string Method { get; private set; }
        public List<Tuple<string, string>> QueryParams { get; private set; }

        public Dictionary<string, string> HeaderParams { get; private set; }
        public Dictionary<string, string> FormParams { get; private set; }
        public Dictionary<string, FileParameter> FileParams { get; private set; }
        public Dictionary<string, string> PathParams { get; private set; }
        public object PostBody { get; private set; }
        public string ContentType { get; private set; }

        private static readonly string[] ValidMethods = { "GET", "POST", "PUT", "DELETE", "PATCH", "OPTIONS", "HEAD" };

        public HttpRequestOptions(
            string url,
            string method,
            List<Tuple<string, string>> queryParams = null,
            Dictionary<string, string> headerParams = null,
            Dictionary<string, string> formParams = null,
            Dictionary<string, FileParameter> fileParams = null,
            Dictionary<string, string> pathParams = null,
            object postBody = null,
            string contentType = null)
        {
            SetUrl(url);
            SetMethod(method);

            if (queryParams != null)
            {
                SetQueryParams(queryParams);
            }

            if (headerParams != null)
            {
                SetHeaderParams(headerParams);
            }

            if (formParams != null)
            {
                SetFormParams(formParams);
            }

            if (fileParams != null)
            {
                SetFileParams(fileParams);
            }

            if (pathParams != null)
            {
                SetPathParams(pathParams);
            }

            if (postBody != null)
            {
                SetPostBody(postBody);
            }
        }

        // Mandatory fields with validation
        public void SetUrl(string url)
        {
            if (string.IsNullOrEmpty(url))
            {
                throw new ArgumentException("The 'url' property is required");
            }
            Url = url;
        }

        public void SetMethod(string method)
        {
            if (string.IsNullOrEmpty(method) || ValidMethods.Contains(method.ToUpper()))
            {
                throw new ArgumentException("The 'method' property is required");
            }
            Method = method.ToUpper();
        }

        // Optional fields
        public void SetQueryParams(List<Tuple<string, string>> queryParams)
        {
            QueryParams = queryParams ?? throw new ArgumentException("QueryParams cannot be null");
        }

        public void SetHeaderParams(Dictionary<string, string> headerParams)
        {
            HeaderParams = headerParams ?? throw new ArgumentException("HeaderParams cannot be null");
        }

        public void SetFormParams(Dictionary<string, string> formParams)
        {
            FormParams = formParams ?? throw new ArgumentException("FormParams cannot be null");
        }

        public void SetFileParams(Dictionary<string, FileParameter> fileParams)
        {
            FileParams = fileParams ?? throw new ArgumentException("FileParams cannot be null");
        }

        public void SetPathParams(Dictionary<string, string> pathParams)
        {
            PathParams = pathParams ?? throw new ArgumentException("PathParams cannot be null");
        }

        public void SetPostBody(object postBody)
        {
            if (postBody == null)
            {
                throw new ArgumentException("The 'postBody' property is required");
            }
            PostBody = postBody;
        }

        public void SetContentType(string contentType)
        {
            ContentType = contentType;
        }

        public void AddHeaderParam(string key, string value)
        {
            HeaderParams[key] = value;
        }
    }
}
