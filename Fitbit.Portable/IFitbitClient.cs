using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Fitbit.Models;

namespace Fitbit.Api.Portable
{
    public interface IFitbitClient
    {
        Task<FitbitResponse<List<Device>>> GetDevicesAsync();
        Task<FitbitResponse<List<UserProfile>>> GetFriendsAsync(string encodedUserId = default(string));
        Task<FitbitResponse<UserProfile>> GetUserProfileAsync(string encodedUserId = default(string));

        Task<FitbitResponse<TimeSeriesDataList>> GetTimeSeriesAsync(TimeSeriesResourceType timeSeriesResourceType, DateTime startDate, DateTime endDate, string encodedUserId = default(string));
        Task<FitbitResponse<TimeSeriesDataList>> GetTimeSeriesAsync(TimeSeriesResourceType timeSeriesResourceType, DateTime endDate, DateRangePeriod period, string encodedUserId = default(string));
        Task<FitbitResponse<TimeSeriesDataListInt>> GetTimeSeriesIntAsync(TimeSeriesResourceType timeSeriesResourceType, DateTime startDate, DateTime endDate, string encodedUserId = default(string));
        Task<FitbitResponse<TimeSeriesDataListInt>> GetTimeSeriesIntAsync(TimeSeriesResourceType timeSeriesResourceType, DateTime endDate, DateRangePeriod period, string encodedUserId = default(string));

        Task<FitbitResponse<Food>> GetFoodAsync(DateTime date, string encodedUserId = default(string));

        Task<FitbitResponse<BodyMeasurements>> GetBodyMeasurementsAsync(DateTime date, string encodedUserId = default(string));
        Task<FitbitResponse<Fat>> GetFatAsync(DateTime startDate, DateRangePeriod period);
        Task<FitbitResponse<Fat>> GetFatAsync(DateTime startDate, DateTime? endDate = null);
        Task<FitbitResponse<Weight>> GetWeightAsync(DateTime startDate, DateRangePeriod period);
        Task<FitbitResponse<Weight>> GetWeightAsync(DateTime startDate, DateTime? endDate = null);
    }
}