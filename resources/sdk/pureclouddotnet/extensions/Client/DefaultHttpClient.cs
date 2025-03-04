using System;
using System.Threading;
using System.Threading.Tasks;
using RestSharp;


namespace {{=it.packageName }}.Client
{
    public class DefaultHttpClient : AbstractHttpClient<RestRequest, RestResponse>
    {
        private RestClient restClient;

        public DefaultHttpClient(int timeout = 100000) : base()
        {
            if (timeout > 0)
            {
                SetTimeout(timeout);
            }

            var options = new RestClientOptions(GetConfUri("api", ClientOptions.BaseUrl)){};
            
            if (ClientOptions.HttpMessageHandler != null)
            {
                options = new RestClientOptions(GetConfUri("api", ClientOptions.BaseUrl))
                {
                    ConfigureMessageHandler = _ => ClientOptions.HttpMessageHandler 
                };
               
            }

            if (Configuration.UserAgent != null)
            {
               options.UserAgent = Configuration.UserAgent;   
            }

            if (Configuration.Timeout > 0)
            {
                options.MaxTimeout = Configuration.Timeout;   
            }

            if (ClientOptions.Proxy != null)
            {
                options.Proxy = ClientOptions.Proxy;   
            }

            restClient = new RestClient();
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
            Console.WriteLine(options.Url);
            var request = new RestRequest(options.Url, options.Method);

            // add path parameter, if any
            foreach(var param in options.PathParams)
                request.AddParameter(param.Key, param.Value, ParameterType.UrlSegment);

            // add header parameter, if any
            foreach(var param in options.HeaderParams)
                request.AddHeader(param.Key, param.Value);

            // add query parameter, if any
            foreach(var param in options.QueryParams)
                request.AddQueryParameter(param.Item1, param.Item2);

            // add form parameter, if any
            foreach(var param in options.FormParams)
                request.AddParameter(param.Key, param.Value);

            // add file parameter, if any
            foreach(var param in options.FileParams)
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

            return request;
        }
    }
}