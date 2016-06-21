using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Net.Http;
using System.Text;

namespace Fitbit.Api.Portable.Helpers
{
    internal class HttpClientFactory
    {

        internal static HttpClient Create(FitbitClient client, IFitbitInterceptor vanillaInterceptor)
        {
            var interceptors = new List<IFitbitInterceptor>();

            interceptors.Add(vanillaInterceptor);

            return HttpClientFactory.Create(client, interceptors);
        }

        internal static HttpClient Create(FitbitClient client, List<IFitbitInterceptor> interceptors)
        {
            var pipeline = client.CreatePipeline(interceptors);

            if(interceptors.Count > 0)
                return new HttpClient(pipeline);
            else
            {
                return new HttpClient();
            }
        }
    }
}
