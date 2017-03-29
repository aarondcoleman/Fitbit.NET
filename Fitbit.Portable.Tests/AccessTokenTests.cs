using System;
using System.Diagnostics;
using Fitbit.Api.Portable;
using Fitbit.Api.Portable.OAuth2;
using FluentAssertions;
using NUnit.Framework;
using Ploeh.AutoFixture;

namespace Fitbit.Portable.Tests
{
    [TestFixture]
    public class AccessTokenTests
    {
        [Test]
        [Category("Portable")]
        public void Can_Deserialize_AccessToken()
        {
            string content = SampleDataHelper.GetContent("AccessToken.json");

            var deserializer = new JsonDotNetSerializer();
            var result = deserializer.Deserialize<OAuth2AccessToken>(content);

            Validate(result);
        }

        [Test]
        [Category("Portable")]
        public void OAuth2Helper_Can_Deserialize_AccessToken()
        {
            string content = SampleDataHelper.GetContent("AccessToken.json");

            var result = OAuth2Helper.ParseAccessTokenResponse(content);

            Validate(result);
        }

        [Test]
        [Category("Portable")]
        public void OAuth2Helper_Throws_FitbitTokenException()
        {
            string content = SampleDataHelper.GetContent("ApiError-Request-StaleToken.json");

            try
            {
                OAuth2Helper.ParseAccessTokenResponse(content);
            }
            catch (FitbitException exception)
            {
                // can only use ShouldThrow on async funcs of Task
                exception.ApiErrors.Count.Should().Be(1);
            }
        }

        [Test]
        [Category("Portable")]
        [Category("OAuth2")]
        public void AccessToken_Knows_Token_Is_Valid()
        {
            //arrenge
            var fixture = new Fixture();
            //Create a token whose expiration date is in the future
            var tokenWithFutureExpirationDate = fixture.Build<OAuth2AccessToken>()
                                                .With(t => t.UtcExpirationDate, DateTime.UtcNow.AddHours(1.0))
                                                .Create();

            Assert.IsTrue(tokenWithFutureExpirationDate.IsFresh());
        }

        [Test]
        [Category("Portable")]
        [Category("OAuth2")]
        public void AccessToken_Knows_Token_Is_Stale()
        {
            //arrenge
            var fixture = new Fixture();
            //Create a token whose expiration date is in the future
            var tokenWithFutureExpirationDate = fixture.Build<OAuth2AccessToken>()
                                                .With(t => t.UtcExpirationDate, DateTime.UtcNow.AddHours(-1.0))
                                                .Create();

            Assert.IsFalse(tokenWithFutureExpirationDate.IsFresh());
        }

        [Test]
        [Category("Portable")]
        [Category("OAuth2")]
        public void AccessToken_With_No_Expiration_Time_Throws_On_IsFresh()
        {
            //arrenge
            var fixture = new Fixture();
            //Create a token with no Expiration time
            var sut = new OAuth2AccessToken();

            Assert.Throws<InvalidOperationException>(() => sut.IsFresh());
        }

        public void Validate(OAuth2AccessToken token)
        {
            token.Should().NotBeNull();
            token.Token.Should().Be("eyJhbGciOiJIUzI1NiJ9.eyJleHAiOjE0MzAzNDM3MzUsInNjb3BlcyI6Indwcm8gd2xvYyB3bnV0IHdzbGUgd3NldCB3aHIgd3dlaSB3YWN0IHdzb2MiLCJzdWIiOiJBQkNERUYiLCJhdWQiOiJJSktMTU4iLCJpc3MiOiJGaXRiaXQiLCJ0eXAiOiJhY2Nlc3NfdG9rZW4iLCJpYXQiOjE0MzAzNDAxMzV9.z0VHrIEzjsBnjiNMBey6wtu26yHTnSWz_qlqoEpUlpc");
            token.TokenType.Should().Be("Bearer");
            token.ExpiresIn.Should().Be(3600);
            token.RefreshToken.Should().Be("c643a63c072f0f05478e9d18b991db80ef6061e4f8e6c822d83fed53e5fafdd7");
            token.UserId.Should().Be("26FWFL");
            token.Scope.Should().Be("heartrate weight nutrition settings activity sleep");
        }
    }
}

