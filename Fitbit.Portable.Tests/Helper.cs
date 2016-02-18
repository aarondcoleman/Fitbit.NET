using System;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Fitbit.Api.Portable;
using Moq;
using Moq.Protected;

namespace Fitbit.Portable.Tests
{
    public static class Helper
    {
        /// <summary>
        /// Unit test helper to create a Mock HttpMessageHandler with the specified response and verification callback
        /// </summary>
        /// <param name="response"></param>
        /// <param name="verificationCallback"></param>
        /// <returns></returns>
        public static FitbitClient CreateFitbitClient(Func<HttpResponseMessage> response, Action<HttpRequestMessage, CancellationToken> verificationCallback)
        {
            var handler = new Mock<HttpMessageHandler>();
            handler.Protected()
                .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
                .Returns(Task<HttpResponseMessage>.Factory.StartNew(response))
                .Callback(verificationCallback);

            //We ignore the handler that is wired to IFitbitInterceptor
            return new FitbitClient(messageHandler => new HttpClient(handler.Object));
        }

        /// <summary>
        /// Unit test helper to create a mock HttpResponseMessage action which is errored
        /// </summary>
        /// <returns></returns>
        public static Func<HttpResponseMessage> CreateErrorResponse(HttpStatusCode statusCode = HttpStatusCode.LengthRequired) // general error
        {
            string errorFilePath = "ApiError.json";
            switch (statusCode)
            {
                case HttpStatusCode.Forbidden:
                    errorFilePath = "ApiError-Request-Forbidden.json";
                    break;

                case HttpStatusCode.Unauthorized:
                    errorFilePath = "ApiError-Request-Unauthorized.json";
                    break;
                case HttpStatusCode.BadRequest:
                    errorFilePath = "ApiError-Request-BadRequest.json";
                    break;
            }

            string content = SampleDataHelper.GetContent(errorFilePath);
            var responseMessage =
                new Func<HttpResponseMessage>(() => new HttpResponseMessage(statusCode) {Content = new StringContent(content)});
            return responseMessage;
        }
    }
}
