using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Fitbit.Models;

namespace Fitbit.Api.Portable
{
    public interface IFitbitClient
    {
        Task<FitbitResponse<Activity>> GetDayActivityAsync(DateTime activityDate, string encodedUserId = default(string));
        Task<FitbitResponse<ActivitySummary>> GetDayActivitySummaryAsync(DateTime activityDate, string encodedUserId = default(string));
        Task<FitbitResponse<SleepData>> GetSleepAsync(DateTime sleepDate);
        Task<FitbitResponse<List<Device>>> GetDevicesAsync();
        Task<FitbitResponse<List<UserProfile>>> GetFriendsAsync(string encodedUserId = default(string));
        Task<FitbitResponse<UserProfile>> GetUserProfileAsync(string encodedUserId = default(string));
        Task<FitbitResponse<TimeSeriesDataList>> GetTimeSeriesAsync(TimeSeriesResourceType timeSeriesResourceType, DateTime startDate, DateTime endDate, string encodedUserId = default(string));
        Task<FitbitResponse<TimeSeriesDataList>> GetTimeSeriesAsync(TimeSeriesResourceType timeSeriesResourceType, DateTime endDate, DateRangePeriod period, string encodedUserId = default(string));
        Task<FitbitResponse<TimeSeriesDataListInt>> GetTimeSeriesIntAsync(TimeSeriesResourceType timeSeriesResourceType, DateTime startDate, DateTime endDate, string encodedUserId = default(string));
        Task<FitbitResponse<TimeSeriesDataListInt>> GetTimeSeriesIntAsync(TimeSeriesResourceType timeSeriesResourceType, DateTime endDate, DateRangePeriod period, string encodedUserId = default(string));
        Task<FitbitResponse<IntradayData>> GetIntraDayTimeSeriesAsync(IntradayResourceType timeSeriesResourceType, DateTime dayAndStartTime, TimeSpan intraDayTimeSpan);
        Task<FitbitResponse<Food>> GetFoodAsync(DateTime date, string encodedUserId = default(string));
        Task<FitbitResponse<BloodPressureData>> GetBloodPressureAsync(DateTime date, string encodedUserId = default(string));
        Task<FitbitResponse<BodyMeasurements>> GetBodyMeasurementsAsync(DateTime date, string encodedUserId = default(string));
        Task<FitbitResponse<Fat>> GetFatAsync(DateTime startDate, DateRangePeriod period);
        Task<FitbitResponse<Fat>> GetFatAsync(DateTime startDate, DateTime? endDate = null);
        Task<FitbitResponse<Weight>> GetWeightAsync(DateTime startDate, DateRangePeriod period);
        Task<FitbitResponse<Weight>> GetWeightAsync(DateTime startDate, DateTime? endDate = null);
        Task<FitbitResponse<ActivityGoals>> SetGoalsAsync(int caloriesOut = default(int), decimal distance = default(decimal), int floors = default(int), int steps = default(int), int activeMinutes = default(int));
        Task<FitbitResponse<WaterData>> GetWaterAsync(DateTime date);
        Task<FitbitResponse<WaterLog>> LogWaterAsync(DateTime date, WaterLog log);
        Task<FitbitResponse<NoData>> DeleteWaterLogAsync(long logId);

        Task<FitbitResponse<List<ApiSubscription>>> GetSubscriptionsAsync();
        Task<FitbitResponse<ApiSubscription>> AddSubscriptionAsync(APICollectionType apiCollectionType, string uniqueSubscriptionId, string subscriberId = default(string));
    }
}