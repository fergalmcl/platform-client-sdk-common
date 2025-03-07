using System;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using RestSharp;
using {{=it.packageName}}.Extensions;

namespace {{=it.packageName }}.Client
{
    public class DefaultHttpClient : AbstractHttpClient<IHttpRequest, IHttpResponse>
    {
        private RestClient restClient;

        public DefaultHttpClient(Configuration config, ClientRestOptions clientOptions) : base()
        {

            SetTimeout(config.Timeout);
            SetUserAgent(config.UserAgent);

            RestClientOptions options = BuildRestOptions(config, clientOptions);

            restClient = new RestClient(options);
        }

        private RestClientOptions BuildRestOptions(Configuration config, ClientRestOptions clientOptions)
        {
            var options = new RestClientOptions(config.ApiClient.GetConfUri(clientOptions.Prefix, clientOptions.BaseUrl)) { };
            
            if (clientOptions.HttpMessageHandler != null)
            {
                options = new RestClientOptions(config.ApiClient.GetConfUri(clientOptions.Prefix, clientOptions.BaseUrl))
                {
                    ConfigureMessageHandler = _ => clientOptions.HttpMessageHandler
                };

            }

            options.UserAgent = this.UserAgent;

            options.Timeout = TimeSpan.FromMilliseconds(this.Timeout);

            if (clientOptions.Proxy != null)
            {
                options.Proxy = clientOptions.Proxy;
            }

            return options;
        }

        public override async Task<IHttpResponse> ExecuteAsync(HttpRequestOptions httpRequestOptions, CancellationToken cancellationToken = default(CancellationToken))
        {
            var request = PrepareRestRequest(httpRequestOptions);

            var restResp =  await restClient.ExecuteAsync(request, cancellationToken);

            return ConvertToHttpResponse(restResp);
        }

        public override IHttpResponse Execute(HttpRequestOptions httpRequestOptions)
        {
            var request = PrepareRestRequest(httpRequestOptions);

            var restResp = restClient.Execute(request);

            return ConvertToHttpResponse(restResp);
        }

        private IHttpResponse ConvertToHttpResponse(RestResponse response)
        {
            return new HttpResponse
            {
                StatusCode = (int)response.StatusCode,
                StatusDescription = response.StatusDescription,
                Content = response.Content,
                ErrorMessage = response.ErrorMessage,
                RawBytes = response.RawBytes,
                Headers = response.Headers?
                    .GroupBy(h => h.Name)
                    .ToDictionary(
                        g => g.Key,
                        g => string.Join(";", g.Select(h => h.Value?.ToString()))
                    ) ?? new Dictionary<string, string>()
            };
        }

        public Uri BuildUri(HttpRequestOptions options)
        {
            return restClient.BuildUri(PrepareRestRequest(options));
        }

        private RestRequest PrepareRestRequest(HttpRequestOptions options)
        {
            Method restSharpMethod = ConvertToRestSharpMethod(options.Method);

            var request = new RestRequest(options.Url, restSharpMethod);

            // add path parameter, if any
            foreach (var param in options.PathParams)
                request.AddParameter(param.Key, param.Value, ParameterType.UrlSegment);

            // add header parameter, if any
            foreach (var param in options.HeaderParams)
                request.AddHeader(param.Key, param.Value);

            // add query parameter, if any
            foreach (var param in options.QueryParams)
                request.AddQueryParameter(param.Item1, param.Item2);

            // add form parameter, if any
            foreach (var param in options.FormParams)
                request.AddParameter(param.Key, param.Value);

            // add file parameter, if any
            foreach (var param in options.FileParams)
            {
                request.AddFile(param.Value.Name, param.Value.GetFile, param.Value.FileName, param.Value.ContentType);
            }

            if (options.PostBody != null) // http body (model or byte[]) parameter
            {
                if (options.PostBody.GetType() == typeof(String))
                {
                    request.AddParameter("application/json", options.PostBody, ParameterType.RequestBody);
                }
                else if (options.PostBody.GetType() == typeof(byte[]))
                {
                    request.AddParameter(options.ContentType, options.PostBody, ParameterType.RequestBody);
                }
            }

            request.Timeout = TimeSpan.FromMilliseconds(this.Timeout);

            return request;
        }

        private Method ConvertToRestSharpMethod(string method)
        {
            switch (method.ToUpperInvariant())
            {
                case "GET":
                    return Method.Get;
                case "POST":
                    return Method.Post;
                case "PUT":
                    return Method.Put;
                case "DELETE":
                    return Method.Delete;
                case "HEAD":
                    return Method.Head;
                case "OPTIONS":
                    return Method.Options;
                case "PATCH":
                    return Method.Patch;
                case "MERGE":
                    return Method.Merge;
                case "COPY":
                    return Method.Copy;
                default:
                    throw new ArgumentException($"Unsupported HTTP method: {method}");
            }
        }
    }
}