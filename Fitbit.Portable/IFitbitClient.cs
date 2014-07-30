using System.Collections.Generic;
using System.Threading.Tasks;
using Fitbit.Models;

namespace Fitbit.Api.Portable
{
    public interface IFitbitClient
    {
        Task<List<Device>> GetDevicesAsync();
        Task<List<UserProfile>> GetFriendsAsync(string encodedUserId = default(string));
        Task<UserProfile> GetUserProfileAsync(string encodedUserId = default(string));
    }
}