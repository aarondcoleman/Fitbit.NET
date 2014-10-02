using System.Linq;

namespace Fitbit.Api.Portable
{
    /// <summary>
    /// Additional parsing and access to specific parts of the FitbitResponse.
    /// This must be kept in the same namespace as the FitbitResponse and FitbitCLient so the methods are discoverable
    /// </summary>
    public static class FitbitResponseExtensions
    {
        /// <summary>
        /// If a http response of 429 then the rate limit has been reached; a retry after header is returned in the response
        /// https://wiki.fitbit.com/display/API/Rate+Limit
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="response"></param>
        /// <returns></returns>
        public static int RetryAfter<T>(this FitbitResponse<T> response) where T : class
        {
            int value = 0;
            if ((429 == (int) response.StatusCode) && (response.HttpHeaders != null))
            {
                var retryAfterHeader = response.HttpHeaders.FirstOrDefault(h => h.Key == "Retry-After");
                if (retryAfterHeader.Key != null)
                {
                    string headerValue = retryAfterHeader.Value.FirstOrDefault();
                    if (!string.IsNullOrWhiteSpace(headerValue))
                    {
                        int retryAfter;
                        if (int.TryParse(headerValue, out retryAfter))
                        {
                            value = retryAfter;
                        }
                    }
                }
            }
            return value;
        }
    }
}