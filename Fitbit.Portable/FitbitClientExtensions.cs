namespace Fitbit.Api.Portable
{
    internal static class FitbitClientExtensions
    {
        /// <summary>
        /// Converts the REST api resource into the fully qualified url
        /// </summary>
        /// <param name="apiCall"></param>
        /// <returns></returns>
        public static string ToFullUrl(this string apiCall)
        {
            if (apiCall.StartsWith("/"))
            {
                apiCall = apiCall.TrimStart(new[] { '/' });
            }
            return Constants.BaseApiUrl + apiCall;
        }
    }
}