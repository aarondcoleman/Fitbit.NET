namespace Fitbit.Api.Portable
{
    internal static class FitbitClientExtensions
    {
        /// <summary>
        /// Converts the REST api resource into the fully qualified url
        /// </summary>
        /// <param name="apiCall"></param>
        /// <param name="encodedUserId"></param>
        /// <returns></returns>
        internal static string ToFullUrl(this string apiCall, string encodedUserId = default(string))
        {
            string userSignifier = "-"; //used for current user
            if (!string.IsNullOrWhiteSpace(encodedUserId))
                userSignifier = encodedUserId;

            apiCall = string.Format(apiCall, userSignifier);

            if (apiCall.StartsWith("/"))
            {
                apiCall = apiCall.TrimStart(new[] { '/' });
            }
            return Constants.BaseApiUrl + apiCall;
        }
    }
}