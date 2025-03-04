using System;
using System.Net;
using System.Net.Http;
using System.Security.Cryptography.X509Certificates;
using System.Net.Security;
using System.Threading;
using System.Threading.Tasks;
using RestSharp;


namespace {{=it.packageName }}.Client
{
    public abstract class AbstractHttpClient<TRequest, TResponse>
    {
        protected int Timeout { get; set; } = 100000;

        public void SetTimeout(int timeout)
        {
            if (timeout <= 0)
            {
                throw new ArgumentException("Timeout must be greater than 0");
            }
            Timeout = timeout;
        }

        // public void SetHttpsAgent(string httpsAgent)
        // {

        // }

        public abstract Task<TResponse> ExecuteAsync(HttpRequestOptions httpRequestOptions, CancellationToken cancellationToken = default(CancellationToken));
        public abstract TResponse Execute(HttpRequestOptions httpRequestOptions);
    }
}
