using System;
using System.Collections.Generic;
using Fitbit.Models;

namespace Fitbit.Api.Portable
{
    internal static class FitbitClientExtensions
    {
        /// <summary>
        /// Converts the REST api resource into the fully qualified url
        /// </summary>
        /// <param name="apiCall"></param>
        /// <param name="encodedUserId"></param>
        /// <param name="args"></param>
        /// <returns></returns>
        internal static string ToFullUrl(this string apiCall, string encodedUserId = default(string), params object[] args)
        {
            string userSignifier = "-"; //used for current user
            if (!string.IsNullOrWhiteSpace(encodedUserId))
            {
                userSignifier = encodedUserId;    
            }
            
            var parameters = new List<object> {userSignifier};
            if (args != null)
            {
                parameters.AddRange(args);
            }

            apiCall = string.Format(apiCall, parameters.ToArray());

            if (apiCall.StartsWith("/"))
            {
                apiCall = apiCall.TrimStart(new[] { '/' });
            }
            return Constants.BaseApiUrl + apiCall;
        }

        /// <summary>
        /// Converts the timeseries resource into the property to acces the data
        /// </summary>
        /// <param name="timeseriesResource"></param>
        /// <returns></returns>
        internal static string ToTimeSeriesProperty(this TimeSeriesResourceType timeseriesResource)
        {
            var timeSeriesResourceDisplay = timeseriesResource.GetStringValue();
            if (timeSeriesResourceDisplay.StartsWith("/"))
            {
                timeSeriesResourceDisplay = timeSeriesResourceDisplay.TrimStart(new[] { '/' });
            }

            return timeSeriesResourceDisplay.Replace("/", "-");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dateTime"></param>
        /// <returns></returns>
        internal static string ToFitbitFormat(this DateTime dateTime)
        {
            const string DateFormat = "yyyy-MM-dd";
            return dateTime.ToString(DateFormat);
        }
    }
}