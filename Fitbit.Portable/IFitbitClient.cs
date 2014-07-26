using System.Collections.Generic;
using System.Threading.Tasks;
using Fitbit.Models;

namespace Fitbit.Api.Portable
{
    public interface IFitbitClient
    {
        Task<UserProfile> GetUserProfileAsync(string encodedUserId = default(string));
        Task<List<Device>> GetDevicesAsync();
    }
}