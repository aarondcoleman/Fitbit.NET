using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace SampleWebMVCOAuth2.Helpers
{
    /// <summary>
    /// Implementation where we can store OAuth2 tokens in the session data
    /// </summary>
    public class SessionOAuth2AccessTokenRepository : Fitbit.Api.Portable.IOAuth2AccessTokenRepository
    {
        HttpContextBase _httpContext = null;
        internal string _tokenId = null;
        public SessionOAuth2AccessTokenRepository(string tokenId, HttpContextBase currentContext)
        {
            if (currentContext == null)
                throw new ArgumentNullException("You must provide the current HttpContext to save session data");

            if (string.IsNullOrWhiteSpace(tokenId))
                throw new ArgumentNullException("You must provide an ID that this session will use to track");

            _httpContext = currentContext;
            _tokenId = tokenId;
        }

        public void Add(Fitbit.Api.Portable.OAuth2AccessToken accessToken)
        {
            _httpContext.Session[_tokenId] = accessToken;
        }

        public void Update(Fitbit.Api.Portable.OAuth2AccessToken accessToken)
        {
            _httpContext.Session[_tokenId] = accessToken;
        }

        public void Delete()
        {
            _httpContext.Session.Remove(_tokenId);
        }

        public Fitbit.Api.Portable.OAuth2AccessToken Get()
        {
            return (Fitbit.Api.Portable.OAuth2AccessToken)_httpContext.Session[_tokenId];
        }
    }
}
