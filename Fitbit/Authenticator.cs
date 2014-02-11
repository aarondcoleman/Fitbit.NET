using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using RestSharp;
using RestSharp.Authenticators;
using RestSharp.Contrib;
using Fitbit.Models;

namespace Fitbit.Api
{
	public class Authenticator
	{
		const string FitBitBaseUrl = "https://api.fitbit.com";

		private string ConsumerKey;
		private string ConsumerSecret;
		private string RequestTokenUrl;
		private string AccessTokenUrl;
		private string AuthorizeUrl;

        //note: these removed as part of a major breaking change refactor
        //https://github.com/aarondcoleman/Fitbit.NET/wiki/Breaking-Change-on-1-24-2014-as-a-result-of-OAuth-update-in-Fitbit-API
		//private string RequestToken;
		//private string RequestTokenSecret;
		
        private readonly IRestClient client;

		public Authenticator(string ConsumerKey, string ConsumerSecret, string RequestTokenUrl, string AccessTokenUrl,
		                     string AuthorizeUrl, IRestClient restClient = null)
		{
			this.ConsumerKey = ConsumerKey;
			this.ConsumerSecret = ConsumerSecret;
			this.RequestTokenUrl = RequestTokenUrl;
			this.AccessTokenUrl = AccessTokenUrl;
			this.AuthorizeUrl = AuthorizeUrl;
			client = restClient ?? new RestClient(FitBitBaseUrl);
		}

        // note, these removed as part of a major breaking change refactor. 
        // Use GenerateAuthUrlFromRequestToken instead
        // more info: https://github.com/aarondcoleman/Fitbit.NET/wiki/Breaking-Change-on-1-24-2014-as-a-result-of-OAuth-update-in-Fitbit-API

        /*
		/// <summary>
		/// Use this method first to retrieve the url to redirect the user to to allow the url.
		/// Once they are done there, Fitbit will redirect them back to the predetermined completion URL
		/// </summary>
		/// <returns></returns>
		public string GetAuthUrlToken()
		{
			return GenerateAuthUrlToken(false);
		}

		public string GetAuthUrlTokenForcePromptingLogin()
		{
			return GenerateAuthUrlToken(true);
		}

        */
        public string GenerateAuthUrlFromRequestToken(RequestToken token, bool forceLogoutBeforeAuth)
		{
            RestRequest request = null;

			if(forceLogoutBeforeAuth)
				request = new RestRequest("oauth/logout_and_authorize"); //this url will force the user to type in username and password
			else
				request = new RestRequest("oauth/authorize");           //this url will show allow/deny if a user is currently logged in
			request.AddParameter("oauth_token", token.Token);
			var url = client.BuildUri(request).ToString();

			return url;
		} 

        /// <summary>
        /// First step in the OAuth process is to ask Fitbit for a temporary request token. 
        /// From this you should store the RequestToken returned for later processing the auth token.
        /// </summary>
        /// <returns></returns>
        public RequestToken GetRequestToken()
        {
            return GetRequestToken(string.Empty);
        }
        public RequestToken GetRequestToken(string callback)
        {
            client.Authenticator = OAuth1Authenticator.ForRequestToken(this.ConsumerKey, this.ConsumerSecret, callback);

            var request = new RestRequest("oauth/request_token", Method.POST);
            var response = client.Execute(request);

            var qs = HttpUtility.ParseQueryString(response.Content);

            RequestToken token = new RequestToken();

            token.Token = qs["oauth_token"];
            token.Secret = qs["oauth_token_secret"];

            if (response.StatusCode != System.Net.HttpStatusCode.OK)
                throw new Exception("Request Token Step Failed");

            return token;
        }

		/// <summary>
		/// For Desktop authentication. Your code should direct the user to the FitBit website to get
		/// Their pin, they can then enter it here.
		/// </summary>
		/// <param name="pin"></param>
		/// <returns></returns>
		public AuthCredential GetAuthCredentialFromPin(string pin, RequestToken token)
		{
			var request = new RestRequest("oauth/access_token", Method.POST);
			client.Authenticator = OAuth1Authenticator.ForAccessToken(ConsumerKey, ConsumerSecret, token.Token, token.Secret, pin);
			
			var response = client.Execute(request);
			var qs = RestSharp.Contrib.HttpUtility.ParseQueryString(response.Content);

			return new AuthCredential()
			{
				AuthToken = qs["oauth_token"],
				AuthTokenSecret = qs["oauth_token_secret"],
				UserId = qs["encoded_user_id"]
			};
		}

		public AuthCredential ProcessApprovedAuthCallback(RequestToken token)
		{
            if (string.IsNullOrWhiteSpace(token.Token))
                throw new Exception("RequestToken.Token must not be null");
            //else if 

			client.Authenticator = OAuth1Authenticator.ForRequestToken(this.ConsumerKey, this.ConsumerSecret);

			var request = new RestRequest("oauth/access_token", Method.POST);
			

			client.Authenticator = OAuth1Authenticator.ForAccessToken(
				this.ConsumerKey, this.ConsumerSecret, token.Token, token.Secret, token.Verifier
			);
			
			var response = client.Execute(request);

			//Assert.NotNull(response);
			//Assert.Equal(HttpStatusCode.OK, response.StatusCode);

			if (response.StatusCode != HttpStatusCode.OK)
				throw new FitbitException(response.Content, response.StatusCode);

			var qs = HttpUtility.ParseQueryString(response.Content); //not actually parsing querystring, but body is formatted like htat
			var oauth_token = qs["oauth_token"];
			var oauth_token_secret = qs["oauth_token_secret"];
			var encoded_user_id = qs["encoded_user_id"];
			//Assert.NotNull(oauth_token);
			//Assert.NotNull(oauth_token_secret);

			/*
			request = new RestRequest("account/verify_credentials.xml");
			client.Authenticator = OAuth1Authenticator.ForProtectedResource(
				this.ConsumerKey, this.ConsumerSecret, oauth_token, oauth_token_secret
			);

			response = client.Execute(request);

			 */

			return new AuthCredential()
			{
				AuthToken = oauth_token,
				AuthTokenSecret = oauth_token_secret,
				UserId = encoded_user_id
			};

			//Assert.NotNull(response);
			//Assert.Equal(HttpStatusCode.OK, response.StatusCode);

			//request = new RestRequest("statuses/update.json", Method.POST);
			//request.AddParameter("status", "Hello world! " + DateTime.Now.Ticks.ToString());
			//client.Authenticator = OAuth1Authenticator.ForProtectedResource(
			//    consumerKey, consumerSecret, oauth_token, oauth_token_secret
			//);

			//response = client.Execute(request);

			//Assert.NotNull(response);
			//Assert.Equal(HttpStatusCode.OK, response.StatusCode);
		}


	}
}