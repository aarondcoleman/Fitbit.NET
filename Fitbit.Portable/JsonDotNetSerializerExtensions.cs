﻿using System;
using System.Collections.Generic;
using System.Linq;
using Fitbit.Models;
using Newtonsoft.Json.Linq;

namespace Fitbit.Api.Portable
{
    internal static class JsonDotNetSerializerExtensions
    {
        /// <summary>
        /// Parses the error structure which is common when errors are raised from the api
        /// </summary>
        /// <param name="serializer"></param>
        /// <param name="errorJson"></param>
        /// <returns></returns>
        internal static List<ApiError> Errors(this JsonDotNetSerializer serializer, string errorJson)
        {
            if (string.IsNullOrWhiteSpace(errorJson))
            {
                throw new ArgumentNullException(nameof(errorJson), "errorJson can not be empty, null or whitespace");
            }

            serializer.RootProperty = "errors";
            return serializer.Deserialize<List<ApiError>>(errorJson);
        } 

        /// <summary>
        /// GetFat has to doe some custom manipulation with the returned representation
        /// </summary>
        /// <param name="serializer"></param>
        /// <param name="fatJson"></param>
        /// <returns></returns>
        internal static Fat GetFat(this JsonDotNetSerializer serializer, string fatJson)
        {
            if (string.IsNullOrWhiteSpace(fatJson))
            {
                throw new ArgumentNullException(nameof(fatJson), "fatJson can not be empty, null or whitespace");
            }

            var fatlogs = JToken.Parse(fatJson)["fat"];
            var fat = new Fat();
            fat.FatLogs = fatlogs.Children().Select(serializer.Deserialize<FatLog>).ToList();
            return fat;
        }

        /// <summary>
        /// GetWeight has to doe some custom manipulation with the returned representation
        /// </summary>
        /// <param name="serializer"></param>
        /// <param name="weightJson"></param>
        /// <returns></returns>
        internal static Weight GetWeight(this JsonDotNetSerializer serializer, string weightJson)
        {
            if (string.IsNullOrWhiteSpace(weightJson))
            {
                throw new ArgumentNullException(nameof(weightJson), "weightJson can not be empty, null or whitespace");
            }

            var weightlogs = JToken.Parse(weightJson)["weight"];
            var weight = new Weight();
            weight.Weights = weightlogs.Children().Select(serializer.Deserialize<WeightLog>).ToList();
            return weight;
        }

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
                throw new ArgumentNullException(nameof(friendsJson), "friendsJson can not be empty, null or whitespace.");
            }

            serializer.RootProperty = "user";
            var friends = JToken.Parse(friendsJson)["friends"];
            return friends.Children().Select(serializer.Deserialize<UserProfile>).ToList();           
        }

        /// <summary>
        /// GetTimeSeriesDataList has to do some custom manipulation with the returned representation
        /// </summary>
        /// <param name="serializer"></param>
        /// <param name="timeSeriesDataJson"></param>
        /// <returns></returns>
        internal static TimeSeriesDataList GetTimeSeriesDataList(this JsonDotNetSerializer serializer, string timeSeriesDataJson)
        {
            if (string.IsNullOrWhiteSpace(timeSeriesDataJson))
            {
                throw new ArgumentNullException(nameof(timeSeriesDataJson), "timeSeriesDataJson can not be empty, null or whitespace.");
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
        /// <returns></returns>
        internal static TimeSeriesDataListInt GetTimeSeriesDataListInt(this JsonDotNetSerializer serializer, string timeSeriesDataJson)
        {
            if (string.IsNullOrWhiteSpace(timeSeriesDataJson))
            {
                throw new ArgumentNullException(nameof(timeSeriesDataJson), "timeSeriesDataJson can not be empty, null or whitespace.");
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

        internal static IntradayData GetIntradayTimeSeriesData(this JsonDotNetSerializer serializer, string intradayDataJson)
        {
            if (string.IsNullOrWhiteSpace(intradayDataJson))
            {
                throw new ArgumentNullException(nameof(intradayDataJson), "intradayDataJson can not be empty, null or whitespace.");
            }

            var parsedJToken = JToken.Parse(intradayDataJson);

            // need to parse the date first  
            var date = parsedJToken.SelectToken(serializer.RootProperty).First["dateTime"];
            var dataPoints = parsedJToken.SelectTokens(serializer.RootProperty + "-intraday.dataset");

            var result = new IntradayData
            {
                DataSet = (from item in dataPoints.Children()
                    select new IntradayDataValues
                    {
                        Time = DateTime.Parse(date + " " + item["time"]),
                        Value = item["value"].ToString(),
                        METs = item["value"].ToString(),
                        Level = item["level"].ToString()
                    }).ToList()
            };

            return result;
        }
    }
}