using System.Net;
using Fitbit.Api;
using Fitbit.Models;
using Moq;
using NUnit.Framework;
using RestSharp;

namespace Fibit.Tests
{
	[TestFixture]
	public class AuthenticatorTests
	{
		[Test]
		public void ShouldRetreiveCredentials()
		{
			GivenFitBitResponseIs(HttpStatusCode.OK, ValidAccessTokenContent);

			WhenProcessApprovedAuthCallback();

			ThenCredentialIs("AuthToken", "AuthTokenSecret", "UserId");
		}

		[Test]
		[ExpectedException(typeof (FitbitException))]
		public void ShouldThrowErrorOnUnauthorized()
		{
			GivenFitBitResponseIs(HttpStatusCode.Unauthorized, UnauthorizedAccessTokenContent);

			WhenProcessApprovedAuthCallback();
		}

		[SetUp]
		public void Setup()
		{
			_client = new Mock<IRestClient>();
		}

		private void GivenFitBitResponseIs(HttpStatusCode httpStatusCode, string content)
		{
			var restResponse = new RestResponse();
			restResponse.StatusCode = httpStatusCode;
			restResponse.Content =
				content;
			_client.Setup(x => x.Execute(It.IsAny<IRestRequest>())).Returns(restResponse);
		}

		private void WhenProcessApprovedAuthCallback()
		{
            RequestToken token = new RequestToken();
            token.Token = "TAT";
            token.Verifier = "V";
			_credential = Authenticator().ProcessApprovedAuthCallback(token);
		}

		private void ThenCredentialIs(string authtoken, string authtokensecret, string userid)
		{
			Assert.AreEqual(_credential.AuthToken, authtoken);
			Assert.AreEqual(_credential.AuthTokenSecret, authtokensecret);
			Assert.AreEqual(_credential.UserId, userid);
		}

		private Mock<IRestClient> _client;
		private AuthCredential _credential;

		private const string UnauthorizedAccessTokenContent =
			@"{""errors"":[{""errorType"":""oauth"",""fieldName"":""oauth_access_token"",""message"":""Invalid signature or token 'XXX' or token 'YYY'""}],""success"":false}";

		private const string ValidAccessTokenContent =
			@"oauth_token=AuthToken&oauth_token_secret=AuthTokenSecret&encoded_user_id=UserId";

		private Authenticator Authenticator()
		{
			return new Authenticator("K", "S", "RTU", "ATU", "AU", _client.Object);
		}
	}
}