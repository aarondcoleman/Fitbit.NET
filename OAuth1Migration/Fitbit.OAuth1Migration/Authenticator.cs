using System;
using System.Linq;
using System.Threading.Tasks;
using AsyncOAuth;
using Fitbit.Models;
using RequestToken = Fitbit.Models.RequestToken;

namespace Fitbit.OAuth1Migration
{
    public class Authenticator
    {
        public string ConsumerKey { get; private set; }
        public string ConsumerSecret { get; private set; }

        public Authenticator(string consumerKey, string consumerSecret)
        {
            ConsumerKey = consumerKey;
            ConsumerSecret = consumerSecret;
        }

        public string GenerateAuthUrlFromRequestToken(RequestToken token, bool forceLogoutBeforeAuth)
        {
            var url = Constants.BaseApiUrl + (forceLogoutBeforeAuth ? Constants.LogoutAndAuthorizeUri : Constants.AuthorizeUri);
            return string.Format("{0}?oauth_token={1}", url, token.Token);
        }

        /// <summary>
        /// First step in the OAuth process is to ask Fitbit for a temporary request token. 
        /// From this you should store the RequestToken returned for later processing the auth token.
        /// </summary>
        /// <returns></returns>
        public async Task<RequestToken> GetRequestTokenAsync()
        {
            // create authorizer
            var authorizer = new OAuthAuthorizer(ConsumerKey, ConsumerSecret);

            // get request token
            var tokenResponse = await authorizer.GetRequestToken(Constants.BaseApiUrl + Constants.TemporaryCredentialsRequestTokenUri);
            var requestToken = tokenResponse.Token;

            // return the request token
            return new RequestToken
            {
                Token = requestToken.Key,
                Secret = requestToken.Secret
            };
        }

        /// <summary>
        /// For Desktop authentication. Your code should direct the user to the FitBit website to get
        /// Their pin, they can then enter it here.
        /// </summary>
        /// <param name="pin"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        public async Task<AuthCredential> GetAuthCredentialFromPinAsync(string pin, RequestToken token)
        {
            var oauthRequestToken = new AsyncOAuth.RequestToken(token.Token, token.Secret);
            var authorizer = new OAuthAuthorizer(ConsumerKey, ConsumerSecret);
            var accessTokenResponse = await authorizer.GetAccessToken(Constants.BaseApiUrl + Constants.TemporaryCredentialsAccessTokenUri, oauthRequestToken, pin);
            // save access token.
            var accessToken = accessTokenResponse.Token;
            return new AuthCredential
            {
                AuthToken = accessToken.Key,
                AuthTokenSecret = accessToken.Secret
            };
        }

        public async Task<AuthCredential> ProcessApprovedAuthCallbackAsync(RequestToken token)
        {
            if (token == null)
                throw new ArgumentNullException("token", "RequestToken cannot be null");

            if (string.IsNullOrWhiteSpace(token.Token))
                throw new ArgumentNullException("token", "RequestToken.Token must not be null");

            var oauthRequestToken = new AsyncOAuth.RequestToken(token.Token, token.Secret);
            var authorizer = new OAuthAuthorizer(ConsumerKey, ConsumerSecret);
            var accessToken = await authorizer.GetAccessToken(Constants.BaseApiUrl + Constants.TemporaryCredentialsAccessTokenUri, oauthRequestToken, token.Verifier);

            var result = new AuthCredential
            {
                AuthToken = accessToken.Token.Key,
                AuthTokenSecret = accessToken.Token.Secret,
                UserId = accessToken.ExtraData["encoded_user_id"].FirstOrDefault()
            };
            return result;
        }
    }
}