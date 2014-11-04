using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using NUnit.Framework;

namespace Fitbit.Portable.Tests
{
    internal class FakeResponseHandler : DelegatingHandler
    {
        private readonly Dictionary<Uri, HttpResponseMessage> _responses = new Dictionary<Uri, HttpResponseMessage>();
        private readonly Dictionary<Uri, bool> _responseCalls = new Dictionary<Uri, bool>(); 
        
        public int CallCount { get; private set; }

        public FakeResponseHandler()
        {
            CallCount = 0;
        }

        public void AddResponse(Uri uri, HttpResponseMessage responseMessage)
        {
            _responses.Add(uri, responseMessage);
            _responseCalls.Add(uri, false);
        }

        protected async override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, System.Threading.CancellationToken cancellationToken)
        {
            CallCount++;
            
            HttpResponseMessage response;

            // todo: register the expected response messaghe content
            
            if (_responses.ContainsKey(request.RequestUri))
            {
                _responseCalls[request.RequestUri] = true;
                response = _responses[request.RequestUri];
            }
            else
            {
                response = new HttpResponseMessage(HttpStatusCode.NotFound) {RequestMessage = request};
            }

            return response;
        }

        public void AssertAllCalled()
        {
            if (!_responseCalls.All(x => x.Value))
            {
                Assert.IsTrue(false, "Not all specified response calls were called");
            }
        }
    }
}