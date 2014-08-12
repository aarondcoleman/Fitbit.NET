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
    }
}