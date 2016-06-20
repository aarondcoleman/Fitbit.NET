using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Fitbit.Models;

namespace Fitbit.Api.Portable
{
    public interface IFitbitClient
    {
        Task<Activity> GetDayActivityAsync(DateTime activityDate, string encodedUserId = default(string));
        Task<ActivitySummary> GetDayActivitySummaryAsync(DateTime activityDate, string encodedUserId = default(string));
        Task<ActivitiesStats> GetActivitiesStatsAsync(string encodedUserId = default(string));
        Task<SleepData> GetSleepAsync(DateTime sleepDate);
        Task<List<Device>> GetDevicesAsync();
        Task<List<UserProfile>> GetFriendsAsync(string encodedUserId = default(string));
        Task<UserProfile> GetUserProfileAsync(string encodedUserId = default(string));
        Task<TimeSeriesDataList> GetTimeSeriesAsync(TimeSeriesResourceType timeSeriesResourceType, DateTime startDate, DateTime endDate, string encodedUserId = default(string));
        Task<TimeSeriesDataList> GetTimeSeriesAsync(TimeSeriesResourceType timeSeriesResourceType, DateTime endDate, DateRangePeriod period, string encodedUserId = default(string));
        Task<TimeSeriesDataListInt> GetTimeSeriesIntAsync(TimeSeriesResourceType timeSeriesResourceType, DateTime startDate, DateTime endDate, string encodedUserId = default(string));
        Task<TimeSeriesDataListInt> GetTimeSeriesIntAsync(TimeSeriesResourceType timeSeriesResourceType, DateTime endDate, DateRangePeriod period, string encodedUserId = default(string));
        Task<IntradayData> GetIntraDayTimeSeriesAsync(IntradayResourceType timeSeriesResourceType, DateTime dayAndStartTime, TimeSpan intraDayTimeSpan);
        Task<Food> GetFoodAsync(DateTime date, string encodedUserId = default(string));
        Task<BloodPressureData> GetBloodPressureAsync(DateTime date, string encodedUserId = default(string));
        Task<BodyMeasurements> GetBodyMeasurementsAsync(DateTime date, string encodedUserId = default(string));
        Task<Fat> GetFatAsync(DateTime startDate, DateRangePeriod period);
        Task<Fat> GetFatAsync(DateTime startDate, DateTime? endDate = null);
        Task<Weight> GetWeightAsync(DateTime startDate, DateRangePeriod period);
        Task<Weight> GetWeightAsync(DateTime startDate, DateTime? endDate = null);
        Task<ActivityGoals> SetGoalsAsync(int caloriesOut = default(int), decimal distance = default(decimal), int floors = default(int), int steps = default(int), int activeMinutes = default(int));
        Task<WaterData> GetWaterAsync(DateTime date);
        Task<WaterLog> LogWaterAsync(DateTime date, WaterLog log);
        Task DeleteWaterLogAsync(long logId);

        Task<List<ApiSubscription>> GetSubscriptionsAsync();
        Task<ApiSubscription> AddSubscriptionAsync(APICollectionType apiCollectionType, string uniqueSubscriptionId, string subscriberId = default(string));
        Task DeleteSubscriptionAsync(APICollectionType collection, string uniqueSubscriptionId, string subscriberId = null);
    }
}