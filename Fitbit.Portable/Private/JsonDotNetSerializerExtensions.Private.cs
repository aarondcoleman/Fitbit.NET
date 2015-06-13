using System;
using System.Collections.Generic;
using System.Linq;
using Fitbit.Models;
using Newtonsoft.Json.Linq;

namespace Fitbit.Api.Portable
{
    internal static partial class JsonDotNetSerializerExtensions
    {
        /// <summary>
        /// GetTimeSeriesDataList has to do some custom manipulation with the returned representation
        /// </summary>
        /// <param name="serializer"></param>
        /// <param name="timeSeriesDataJson"></param>
        /// <returns></returns>
        internal static HeartActivitiesIntraday GetHeartRateIntraday(this JsonDotNetSerializer serializer, DateTime date, string heartRateIntradayJson)
        {
            if (string.IsNullOrWhiteSpace(heartRateIntradayJson))
            {
                throw new ArgumentNullException("heartRateIntradayJson", "heartRateIntradayJson can not be empty, null or whitespace.");
            }

            var activitiesHeartIntraday = JToken.Parse(heartRateIntradayJson)["activities-heart-intraday"];
            var dataset = activitiesHeartIntraday["dataset"];

            var result = new HeartActivitiesIntraday
            {
                Dataset = (from item in dataset
                           select new DatasetInterval
                            {
                                Time = DateTime.Parse(date.ToString("yyyy-MM-dd") + " " + item["time"].ToString()), //here, maybe pass in the date so we have a full object of date and time
                                Value = int.Parse(item["value"].ToString())
                            }).ToList(),
                DatasetInterval = Convert.ToInt32(activitiesHeartIntraday["datasetInterval"]),
                DatasetType = activitiesHeartIntraday["datasetType"].ToString()

            };

            return result;
        }


        internal static HeartActivitiesTimeSeries GetHeartActivitiesTimeSeries(this JsonDotNetSerializer serializer, string heartActivitiesTimeSeries)
        {
            if (string.IsNullOrWhiteSpace(heartActivitiesTimeSeries))
            {
                throw new ArgumentNullException("heartActivitiesTimeSeries", "heartActivitiesTimeSeries can not be empty, null or whitespace.");
            }

            var activitiesHeartIntraday = JToken.Parse(heartActivitiesTimeSeries)["activities-heart"];
            //var dataset = activitiesHeartIntraday["dataset"];

            var result = new HeartActivitiesTimeSeries()
            {
                HeartActivities = (from item in activitiesHeartIntraday
                           select new HeartActivities
                           {
                               DateTime = DateTime.Parse(item["dateTime"].ToString()), //here, maybe pass in the date so we have a full object of date and time
                               HeartRateZones = serializer.Deserialize<List<HeartRateZone>>(item["value"]["heartRateZones"]),
                               CustomHeartRateZones = serializer.Deserialize<List<HeartRateZone>>(item["value"]["customHeartRateZones"])
                           }).ToList(),
            };

            return result;
        }
    }
}