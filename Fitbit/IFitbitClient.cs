using System;
using System.Collections.Generic;

namespace Fitbit.Api
{
    public interface IFitbitClient
    {
        Fitbit.Models.ApiSubscription AddSubscription(Fitbit.Models.APICollectionType apiCollectionType, string uniqueSubscriptionId);
        Fitbit.Models.ApiSubscription AddSubscription(Fitbit.Models.APICollectionType apiCollectionType, string uniqueSubscriptionId, string subscriberId);
        DateTime? GetActivityTrackerFirstDay();
        List<Fitbit.Models.TrackerAlarm> GetAlarms(string deviceId);
        Fitbit.Models.BloodPressureData GetBloodPressure(DateTime date);
        Fitbit.Models.BloodPressureData GetBloodPressure(DateTime date, string userId);
        Fitbit.Models.BodyMeasurements GetBodyMeasurements(DateTime date);
        Fitbit.Models.BodyMeasurements GetBodyMeasurements(DateTime date, string userId);
        Fitbit.Models.Activity GetDayActivity(DateTime activityDate);
        Fitbit.Models.ActivitySummary GetDayActivitySummary(DateTime activityDate);
        System.Collections.Generic.List<Fitbit.Models.Device> GetDevices();
        Fitbit.Models.Fat GetFat(DateTime startDate, Fitbit.Models.DateRangePeriod period);
        Fitbit.Models.Fat GetFat(DateTime startDate, DateTime? endDate = null);
        Fitbit.Models.Food GetFood(DateTime date, string userId = null);
        System.Collections.Generic.List<Fitbit.Models.UserProfile> GetFriends();
        Fitbit.Models.IntradayData GetIntraDayTimeSeries(Fitbit.Models.IntradayResourceType timeSeriesResourceType, DateTime dayAndStartTime, TimeSpan intraDayTimeSpan);
        Fitbit.Models.SleepData GetSleep(DateTime sleepDate);
        Fitbit.Models.ActivityGoals SetStepGoal(int newStepGoal);
        System.Collections.Generic.List<Fitbit.Models.ApiSubscription> GetSubscriptions();
        Fitbit.Models.TimeSeriesDataList GetTimeSeries(Fitbit.Models.TimeSeriesResourceType timeSeriesResourceType, DateTime endDate, Fitbit.Models.DateRangePeriod period);
        Fitbit.Models.TimeSeriesDataList GetTimeSeries(Fitbit.Models.TimeSeriesResourceType timeSeriesResourceType, DateTime endDate, Fitbit.Models.DateRangePeriod period, string userId);
        Fitbit.Models.TimeSeriesDataList GetTimeSeries(Fitbit.Models.TimeSeriesResourceType timeSeriesResourceType, DateTime startDate, DateTime endDate);
        Fitbit.Models.TimeSeriesDataList GetTimeSeries(Fitbit.Models.TimeSeriesResourceType timeSeriesResourceType, DateTime startDate, DateTime endDate, string userId);
        Fitbit.Models.TimeSeriesDataListInt GetTimeSeriesInt(Fitbit.Models.TimeSeriesResourceType timeSeriesResourceType, DateTime baseDate, string endDateOrPeriod, string userId);
        Fitbit.Models.TimeSeriesDataListInt GetTimeSeriesInt(Fitbit.Models.TimeSeriesResourceType timeSeriesResourceType, DateTime endDate, Fitbit.Models.DateRangePeriod period);
        Fitbit.Models.TimeSeriesDataListInt GetTimeSeriesInt(Fitbit.Models.TimeSeriesResourceType timeSeriesResourceType, DateTime endDate, Fitbit.Models.DateRangePeriod period, string userId);
        Fitbit.Models.TimeSeriesDataListInt GetTimeSeriesInt(Fitbit.Models.TimeSeriesResourceType timeSeriesResourceType, DateTime startDate, DateTime endDate);
        Fitbit.Models.TimeSeriesDataListInt GetTimeSeriesInt(Fitbit.Models.TimeSeriesResourceType timeSeriesResourceType, DateTime startDate, DateTime endDate, string userId);
        Fitbit.Models.UserProfile GetUserProfile();
        Fitbit.Models.UserProfile GetUserProfile(string encodedUserId);
        Fitbit.Models.Weight GetWeight(DateTime startDate, Fitbit.Models.DateRangePeriod period);
        Fitbit.Models.Weight GetWeight(DateTime startDate, DateTime? endDate = null);
        Fitbit.Models.ApiSubscription RemoveSubscription(Fitbit.Models.APICollectionType apiCollectionType, string uniqueSubscriptionId);
        Fitbit.Models.HeartRateLog LogHeartRate(Fitbit.Models.HeartRateLog log, string userId);
        Fitbit.Models.HeartRates GetHeartRates(DateTime date);
        void DeleteHeartRateLog(int logId);
        Fitbit.Models.WaterData GetWater(DateTime date);
        Fitbit.Models.WaterData GetWater(DateTime date, string userId);
        Fitbit.Models.WaterLog LogWater(DateTime date, Fitbit.Models.WaterLog log);
        Fitbit.Models.WaterLog LogWater(DateTime date, Fitbit.Models.WaterLog log, string userId);
        void DeleteWaterLog(long logId);
        void DeleteWaterLog(long logId, string userId);
    }
}