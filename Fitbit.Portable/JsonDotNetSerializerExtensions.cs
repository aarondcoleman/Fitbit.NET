using System;
using System.Collections.Generic;
using System.Linq;
using Fitbit.Models;
using Newtonsoft.Json.Linq;

namespace Fitbit.Api.Portable
{
    internal static class JsonDotNetSerializerExtensions
    {
        /// <summary>
        /// GetFriends has to do some custom manipulation with the returned representation
        /// </summary>
        /// <param name="serializer"></param>
        /// <param name="friendsJson"></param>
        /// <returns></returns>
        internal static List<UserProfile> GetFriends(this JsonDotNetSerializer serializer, string friendsJson)
        {
            if (string.IsNullOrWhiteSpace(friendsJson))
            {
                throw new ArgumentNullException("friendsJson", "friendsJson can not be empty, null or whitespace.");
            }

            // todo: additional error checking of json string required
            serializer.RootProperty = "user";
            var friends = JToken.Parse(friendsJson)["friends"];
            return friends.Children().Select(serializer.Deserialize<UserProfile>).ToList();           
        }

        /// <summary>
        /// GetTimeSeriesDataList has to do some custom manipulation with the returned representation
        /// </summary>
        /// <param name="serializer"></param>
        /// <param name="timeSeriesDataJson"></param>
        /// <param name="arrayProperty"></param>
        /// <returns></returns>
        internal static TimeSeriesDataList GetTimeSeriesDataList(this JsonDotNetSerializer serializer, string timeSeriesDataJson)
        {
            if (string.IsNullOrWhiteSpace(timeSeriesDataJson))
            {
                throw new ArgumentNullException("timeSeriesDataJson", "timeSeriesDataJson can not be empty, null or whitespace.");
            }

            var dataPoints = JToken.Parse(timeSeriesDataJson)[serializer.RootProperty];
            var result = new TimeSeriesDataList
            {
                DataList = (from item in dataPoints
                                select new TimeSeriesDataList.Data
                                        {
                                            DateTime = DateTime.Parse(item["dateTime"].ToString()),
                                            Value = item["value"].ToString()
                                        }).ToList()
            };

            return result;
        }

        /// <summary>
        /// GetTimeSeriesDataList has to do some custom manipulation with the returned representation
        /// </summary>
        /// <param name="serializer"></param>
        /// <param name="timeSeriesDataJson"></param>
        /// <param name="arrayProperty"></param>
        /// <returns></returns>
        internal static TimeSeriesDataListInt GetTimeSeriesDataListInt(this JsonDotNetSerializer serializer, string timeSeriesDataJson)
        {
            if (string.IsNullOrWhiteSpace(timeSeriesDataJson))
            {
                throw new ArgumentNullException("timeSeriesDataJson", "timeSeriesDataJson can not be empty, null or whitespace.");
            }

            var dataPoints = JToken.Parse(timeSeriesDataJson)[serializer.RootProperty];
            var result = new TimeSeriesDataListInt
            {
                DataList = (from item in dataPoints
                            select new TimeSeriesDataListInt.Data
                            {
                                DateTime = DateTime.Parse(item["dateTime"].ToString()),
                                Value = int.Parse(item["value"].ToString())
                            }).ToList()
            };

            return result;
        }
    }
}