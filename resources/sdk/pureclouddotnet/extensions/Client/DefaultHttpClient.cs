using System;
using System.Threading;
using System.Threading.Tasks;
using RestSharp;


namespace {{=it.packageName }}.Client
{
    public class DefaultHttpClient : AbstractHttpClient<RestRequest, RestResponse>
    {
        private RestClient restClient;

        public DefaultHttpClient(Configuration config, ClientRestOptions clientOptions) : base()
        {

            SetTimeout(timeout);
            setUserAgent(userAgent);

            var options = new RestClientOptions(ApiClient.GetConfUri("api", clientOptions.BaseUrl)) { };

            if (clientOptions.HttpMessageHandler != null)
            {
                options = new RestClientOptions(ApiClient.GetConfUri("api", clientOptions.BaseUrl))
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

            restClient = new RestClient(options);
        }

        public override async Task<RestResponse> ExecuteAsync(HttpRequestOptions httpRequestOptions, CancellationToken cancellationToken = default(CancellationToken))
        {
            var request = PrepareRestRequest(httpRequestOptions);

            return await restClient.ExecuteAsync(request, cancellationToken);
        }

        public override RestResponse Execute(HttpRequestOptions httpRequestOptions)
        {
            var request = PrepareRestRequest(httpRequestOptions);

            return restClient.Execute(request);
        }

        public Uri BuildUri(HttpRequestOptions options)
        {
            return restClient.BuildUri(PrepareRestRequest(options));
        }

        private RestRequest PrepareRestRequest(HttpRequestOptions options)
        {
            Method restSharpMethod = (Method)Enum.Parse(typeof(Method), options.Method);

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

            request.Timeourt = this.Timeout;

            return request;
        }
    }
}