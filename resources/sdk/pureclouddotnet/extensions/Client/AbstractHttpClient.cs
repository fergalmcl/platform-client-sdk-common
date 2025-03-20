using System;
using System.Net;
using System.Net.Http;
using System.Security.Cryptography.X509Certificates;
using System.Security.Authentication;
using System.Net.Security;
using System.Threading;
using System.Threading.Tasks;
using {{=it.packageName}}.Extensions;

namespace {{=it.packageName }}.Client
{
    public abstract class AbstractHttpClient
    {
        protected int Timeout { get; set; } = 100000;
        protected string UserAgent { get; set; } = "null";

        public void SetTimeout(int timeout)
        {
            if (timeout <= 0)
            {
                throw new ArgumentException("Timeout must be greater than 0");
            }
            Timeout = timeout;
        }

        public void SetUserAgent(string userAgent)
        {
            if (string.IsNullOrEmpty(userAgent))
            {
                throw new ArgumentException("UserAgent must not be null or empty");
            }
            UserAgent = userAgent;
        }

        public abstract Task<IHttpResponse> ExecuteAsync(IHttpRequest httpRequest, CancellationToken cancellationToken = default(CancellationToken));
        public abstract IHttpResponse Execute(IHttpRequest httpRequest);
    }
}
