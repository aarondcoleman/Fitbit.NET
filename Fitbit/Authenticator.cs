using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RestSharp;
using RestSharp.Authenticators;
using RestSharp.Contrib;
using Fitbit.Models;

namespace Fitbit.Api
{
	public class Authenticator
	{
		private string ConsumerKey;
		private string ConsumerSecret;
		private string RequestTokenUrl;
		private string AccessTokenUrl;
		private string AuthorizeUrl;
		private string RequestToken;
		private string RequestTokenSecret;

		public Authenticator(string ConsumerKey, string ConsumerSecret, string RequestTokenUrl, string AccessTokenUrl, string AuthorizeUrl)
		{
			this.ConsumerKey = ConsumerKey;
			this.ConsumerSecret = ConsumerSecret;
			this.RequestTokenUrl = RequestTokenUrl;
			this.AccessTokenUrl = AccessTokenUrl;
			this.AuthorizeUrl = AuthorizeUrl;
		}

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

		private string GenerateAuthUrlToken(bool forceLogoutBeforeAuth)
		{
			var baseUrl = "https://api.fitbit.com";
			var client = new RestClient(baseUrl);
			client.Authenticator = OAuth1Authenticator.ForRequestToken(this.ConsumerKey, this.ConsumerSecret);

			var request = new RestRequest("oauth/request_token", Method.POST);
			var response = client.Execute(request);

			var qs = HttpUtility.ParseQueryString(response.Content);
			RequestToken = qs["oauth_token"];
			RequestTokenSecret = qs["oauth_token_secret"];

			if (response.StatusCode != System.Net.HttpStatusCode.OK)
				throw new Exception("Request Token Step Failed");

			if(forceLogoutBeforeAuth)
				request = new RestRequest("oauth/logout_and_authorize"); //this url will force the user to type in username and password
			else
				request = new RestRequest("oauth/authorize");           //this url will show allow/deny if a user is currently logged in
			request.AddParameter("oauth_token", RequestToken);
			var url = client.BuildUri(request).ToString();

			return url;
		}

		/// <summary>
		/// For Desktop authentication. Your code should direct the user to the FitBit website to get
		/// Their pin, they can then enter it here.
		/// </summary>
		/// <param name="pin"></param>
		/// <returns></returns>
		public AuthCredential GetAuthCredentialFromPin(string pin)
		{
			var baseUrl = "https://api.fitbit.com";
		
			var client = new RestClient(baseUrl);
			var request = new RestRequest("oauth/access_token", Method.POST);
			client.Authenticator = OAuth1Authenticator.ForAccessToken(ConsumerKey, ConsumerSecret, RequestToken, RequestTokenSecret,pin);
			
			var response = client.Execute(request);
			var qs = RestSharp.Contrib.HttpUtility.ParseQueryString(response.Content);

			return new AuthCredential()
			{
				AuthToken = qs["oauth_token"],
				AuthTokenSecret = qs["oauth_token_secret"],
				UserId = qs["encoded_user_id"]
			};
		}

		public AuthCredential ProcessApprovedAuthCallback(string TempAuthToken, string Verifier)
		{
			//var verifier = "123456"; // <-- Breakpoint here (set verifier in debugger)
			
			var baseUrl = "https://api.fitbit.com";
			var client = new RestClient(baseUrl);
			client.Authenticator = OAuth1Authenticator.ForRequestToken(this.ConsumerKey, this.ConsumerSecret);

			var request = new RestRequest("oauth/access_token", Method.POST);
			

			client.Authenticator = OAuth1Authenticator.ForAccessToken(
				this.ConsumerKey, this.ConsumerSecret, TempAuthToken, "123456", Verifier
			);
			
			var response = client.Execute(request);

			//Assert.NotNull(response);
			//Assert.Equal(HttpStatusCode.OK, response.StatusCode);

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
