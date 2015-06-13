using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Fitbit.Models;

namespace Fitbit.Api.Portable
{
    public partial class FitbitClient
    {
        public async Task<FitbitResponse<HeartActivitiesIntraday>> GetHeartRateIntraday(DateTime date, HeartRateResolution resolution)
        {
            string resolutionText = null;

            //this little big of section is necessary because enums can't start with numbers
            if (resolution == HeartRateResolution.oneSecond)
                resolutionText = "1sec";
            else if (resolution == HeartRateResolution.oneMinute)
                resolutionText = "1min";
            else
                resolutionText = "15min";

            string apiCall = String.Format("https://api.fitbit.com/1.1/user/-/activities/heart/date/{0}/{1}/{2}/time/00:00:00/23:59:59.json", date.ToString("yyyy-MM-dd"), date.ToString("yyyy-MM-dd"), resolutionText);

            HttpResponseMessage response = await HttpClient.GetAsync(apiCall);
            var fitbitResponse = await HandleResponse<HeartActivitiesIntraday>(response);
            if (fitbitResponse.Success)
            {
                string responseBody = await response.Content.ReadAsStringAsync();
                var seralizer = new JsonDotNetSerializer();
                fitbitResponse.Data = seralizer.GetHeartRateIntraday(date, responseBody);
            }

            return fitbitResponse;

        }

        public async Task<FitbitResponse<HeartActivitiesTimeSeries>> GetHeartRateTimeSeries(DateTime date, DateRangePeriod dateRangePeriod, string userId = null)
        {
            if (string.IsNullOrWhiteSpace(userId))
            {
                userId = "-";
            }

            string apiCall = String.Format("https://api.fitbit.com/1.1/user/{0}/activities/heart/date/{1}/{2}.json", userId, date.ToString("yyyy-MM-dd"), dateRangePeriod.GetStringValue());

            HttpResponseMessage response = await HttpClient.GetAsync(apiCall);
            var fitbitResponse = await HandleResponse<HeartActivitiesTimeSeries>(response);
            if (fitbitResponse.Success)
            {
                string responseBody = await response.Content.ReadAsStringAsync();
                var seralizer = new JsonDotNetSerializer();
                fitbitResponse.Data = seralizer.GetHeartActivitiesTimeSeries(responseBody);
            }

            return fitbitResponse;
        }
    }
}
