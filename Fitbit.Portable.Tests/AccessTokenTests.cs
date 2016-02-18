using System;
using Fitbit.Api.Portable;
using Fitbit.Api.Portable.OAuth2;
using FluentAssertions;
using NUnit.Framework;

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
            catch (FitbitTokenException exception)
            {
                // can only use ShouldThrow on async funcs of Task
                exception.ApiErrors.Count.Should().Be(1);
            }
        }

        public void Validate(OAuth2AccessToken token)
        {
            token.Should().NotBeNull();
            token.Token.Should().Be("eyJhbGciOiJIUzI1NiJ9.eyJleHAiOjE0MzAzNDM3MzUsInNjb3BlcyI6Indwcm8gd2xvYyB3bnV0IHdzbGUgd3NldCB3aHIgd3dlaSB3YWN0IHdzb2MiLCJzdWIiOiJBQkNERUYiLCJhdWQiOiJJSktMTU4iLCJpc3MiOiJGaXRiaXQiLCJ0eXAiOiJhY2Nlc3NfdG9rZW4iLCJpYXQiOjE0MzAzNDAxMzV9.z0VHrIEzjsBnjiNMBey6wtu26yHTnSWz_qlqoEpUlpc");
            token.TokenType.Should().Be("Bearer");
            token.ExpiresIn.Should().Be(3600);
            token.RefreshToken.Should().Be("c643a63c072f0f05478e9d18b991db80ef6061e4f8e6c822d83fed53e5fafdd7");
            token.UserId.Should().Be("26FWFL");
        }
    }
}

