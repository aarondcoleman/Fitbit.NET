using Fitbit.Api.Portable.OAuth2;
using System.Threading.Tasks;

namespace Fitbit.Api.Portable
{
    public interface ITokenManager
    {
        Task<OAuth2AccessToken> RefreshTokenAsync(FitbitClient client);
    }
}