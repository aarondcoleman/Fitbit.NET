using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fitbit.Api.Portable
{
    public interface IFitbitClient
    {
        Task<Fitbit.Models.UserProfile> GetUserProfileAsync(string encodedUserId = default(string));
    }
}
