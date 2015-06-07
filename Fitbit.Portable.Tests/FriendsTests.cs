using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading;
using Fitbit.Api.Portable;
using Fitbit.Models;
using NUnit.Framework;

namespace Fitbit.Portable.Tests
{
    [TestFixture]
    public class FriendsTests
    {
        [Test]
        public async void GetFriendsMultipleAsync_Success()
        {
            string content = SampleDataHelper.GetContent("GetFriends-Multiple.json");

            var responseMessage = new Func<HttpResponseMessage>(() =>
            {
                return new HttpResponseMessage(HttpStatusCode.OK) { Content = new StringContent(content) };
            });

            var verification = new Action<HttpRequestMessage, CancellationToken>((message, token) =>
            {
                Assert.AreEqual(HttpMethod.Get, message.Method);
                Assert.AreEqual("https://api.fitbit.com/1/user/-/friends.json", message.RequestUri.AbsoluteUri);
            });

            var fitbitClient = Helper.CreateFitbitClient(responseMessage, verification);
            
            var response = await fitbitClient.GetFriendsAsync();

            Assert.IsTrue(response.Success);
            var friends = response.Data;
            Assert.AreEqual(3, friends.Count);
            ValidateMultipleFriends(friends);
        }

        [Test]
        public async void GetFriendsSingleAsync_Success()
        {
            string content = SampleDataHelper.GetContent("GetFriends-Single.json");

            var responseMessage = new Func<HttpResponseMessage>(() =>
            {
                return new HttpResponseMessage(HttpStatusCode.OK) { Content = new StringContent(content) };
            });

            var verification = new Action<HttpRequestMessage, CancellationToken>((message, token) =>
            {
                Assert.AreEqual(HttpMethod.Get, message.Method);
                Assert.AreEqual("https://api.fitbit.com/1/user/-/friends.json", message.RequestUri.AbsoluteUri);
            });

            var fitbitClient = Helper.CreateFitbitClient(responseMessage, verification);
            
            var response = await fitbitClient.GetFriendsAsync();

            Assert.IsTrue(response.Success);
            var friends = response.Data;
            ValidateSingleFriend(friends);
        }

        [Test]
        public async void GetFriendsAsync_Failure_Errors()
        {
            var responseMessage = Helper.CreateErrorResponse();
            var verification = new Action<HttpRequestMessage, CancellationToken>((message, token) =>
            {
                Assert.AreEqual(HttpMethod.Get, message.Method);
            });

            var fitbitClient = Helper.CreateFitbitClient(responseMessage, verification);
            
            var response = await fitbitClient.GetFriendsAsync();
            
            Assert.IsFalse(response.Success);
            Assert.IsNull(response.Data);
            Assert.AreEqual(1, response.Errors.Count);
        }

        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Throws_Exception_With_Empty_String()
        {
            var deserializer = new JsonDotNetSerializer();
            deserializer.GetFriends(string.Empty);
        }

        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Throws_Exception_With_Null_String()
        {
            var deserializer = new JsonDotNetSerializer();
            deserializer.GetFriends(null);
        }

        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Throws_Exception_With_WhiteSpace()
        {
            var deserializer = new JsonDotNetSerializer();
            deserializer.GetFriends("         ");
        }

        [Test]
        public void Can_Deserialize_Friends_Multiple()
        {
            string content = SampleDataHelper.GetContent("GetFriends-Multiple.json");
            var deserializer = new JsonDotNetSerializer();
            
            List<UserProfile> friends = deserializer.GetFriends(content);

            Assert.IsNotNull(friends);
            Assert.IsTrue(friends.Count == 3);

            ValidateMultipleFriends(friends);
        }

        [Test]
        public void Can_Deserialize_Friends_Single()
        {
            string content = SampleDataHelper.GetContent("GetFriends-Single.json");
            var deserializer = new JsonDotNetSerializer();

            List<UserProfile> friends = deserializer.GetFriends(content);

            Assert.IsNotNull(friends);
            ValidateSingleFriend(friends);
        }

        [Test]
        public void Can_Deserialize_Friends_None()
        {
            string content = SampleDataHelper.GetContent("GetFriends-None.json");
            var deserializer = new JsonDotNetSerializer();

            List<UserProfile> friends = deserializer.GetFriends(content);

            Assert.IsNotNull(friends);
            Assert.IsTrue(friends.Count == 0);
        }

        private void ValidateMultipleFriends(List<UserProfile> friends)
        {
            var friend = friends.First();

            Assert.AreEqual("http://www.fitbit.com/images/profile/defaultProfile_100_male.gif", friend.Avatar);
            Assert.AreEqual("http://www.fitbit.com/images/profile/defaultProfile_150_male.gif", friend.Avatar150);
            Assert.AreEqual("GB", friend.Country);
            Assert.AreEqual(DateTime.Parse("1970-01-01"), friend.DateOfBirth);
            Assert.AreEqual("Andy", friend.DisplayName);
            Assert.AreEqual("242XXX", friend.EncodedId);
            Assert.AreEqual("Andy Fullname", friend.FullName);
            Assert.AreEqual(Gender.MALE, friend.Gender);
            Assert.AreEqual(155, friend.Height);
            Assert.AreEqual("en_GB", friend.Locale);
            Assert.AreEqual(DateTime.Parse("2010-12-25"), friend.MemberSince);
            Assert.AreEqual("", friend.Nickname);
            Assert.AreEqual(3600000, friend.OffsetFromUTCMillis);
            Assert.AreEqual("SUNDAY", friend.StartDayOfWeek);
            Assert.AreEqual(0, friend.StrideLengthRunning);
            Assert.AreEqual(0, friend.StrideLengthWalking);
            Assert.AreEqual("Europe/London", friend.Timezone);
            Assert.AreEqual(84.7, friend.Weight);

            friend = friends.FirstOrDefault(x => x.Gender == Gender.FEMALE);

            Assert.AreEqual("http://www.fitbit.com/images/profile/defaultProfile_100_female.gif", friend.Avatar);
            Assert.AreEqual("http://www.fitbit.com/images/profile/defaultProfile_150_female.gif", friend.Avatar150);
            Assert.AreEqual("GB", friend.Country);
            Assert.AreEqual(DateTime.Parse("1984-09-07"), friend.DateOfBirth);
            Assert.AreEqual("Laura", friend.DisplayName);
            Assert.AreEqual("24WXXX", friend.EncodedId);
            Assert.AreEqual("", friend.FullName);
            Assert.AreEqual(Gender.FEMALE, friend.Gender);
            Assert.AreEqual(165, friend.Height);
            Assert.AreEqual("en_GB", friend.Locale);
            Assert.AreEqual(DateTime.Parse("2013-02-01"), friend.MemberSince);
            Assert.AreEqual("", friend.Nickname);
            Assert.AreEqual(3600000, friend.OffsetFromUTCMillis);
            Assert.AreEqual("SUNDAY", friend.StartDayOfWeek);
            Assert.AreEqual(0, friend.StrideLengthRunning);
            Assert.AreEqual(0, friend.StrideLengthWalking);
            Assert.AreEqual("Europe/London", friend.Timezone);
            Assert.AreEqual(0, friend.Weight);

            friend = friends.Last();

            Assert.AreEqual("http://www.fitbit.com/images/profile/defaultProfile_100_male.gif", friend.Avatar);
            Assert.AreEqual("http://www.fitbit.com/images/profile/defaultProfile_150_male.gif", friend.Avatar150);
            Assert.AreEqual("GB", friend.Country);
            Assert.AreEqual(DateTime.Parse("1978-09-24"), friend.DateOfBirth);
            Assert.AreEqual("Michael", friend.DisplayName);
            Assert.AreEqual("24NXXX", friend.EncodedId);
            Assert.AreEqual("", friend.FullName);
            Assert.AreEqual(Gender.MALE, friend.Gender);
            Assert.AreEqual(190.5, friend.Height);
            Assert.AreEqual("en_GB", friend.Locale);
            Assert.AreEqual(DateTime.Parse("2013-01-01"), friend.MemberSince);
            Assert.AreEqual("", friend.Nickname);
            Assert.AreEqual(3600000, friend.OffsetFromUTCMillis);
            Assert.AreEqual("SUNDAY", friend.StartDayOfWeek);
            Assert.AreEqual(0, friend.StrideLengthRunning);
            Assert.AreEqual(0, friend.StrideLengthWalking);
            Assert.AreEqual("Europe/London", friend.Timezone);
            Assert.AreEqual(0, friend.Weight);
        }

        private void ValidateSingleFriend(List<UserProfile> friends)
        {
            Assert.IsTrue(friends.Count == 1);

            var friend = friends.First();
            Assert.AreEqual("http://www.fitbit.com/images/profile/defaultProfile_100_female.gif", friend.Avatar);
            Assert.AreEqual("http://www.fitbit.com/images/profile/defaultProfile_150_female.gif", friend.Avatar150);
            Assert.AreEqual("GB", friend.Country);
            Assert.AreEqual(DateTime.Parse("1984-09-07"), friend.DateOfBirth);
            Assert.AreEqual("Laura", friend.DisplayName);
            Assert.AreEqual("24WXXX", friend.EncodedId);
            Assert.AreEqual("", friend.FullName);
            Assert.AreEqual(Gender.FEMALE, friend.Gender);
            Assert.AreEqual(165, friend.Height);
            Assert.AreEqual("en_GB", friend.Locale);
            Assert.AreEqual(DateTime.Parse("2013-02-01"), friend.MemberSince);
            Assert.AreEqual("", friend.Nickname);
            Assert.AreEqual(3600000, friend.OffsetFromUTCMillis);
            Assert.AreEqual("SUNDAY", friend.StartDayOfWeek);
            Assert.AreEqual(0, friend.StrideLengthRunning);
            Assert.AreEqual(0, friend.StrideLengthWalking);
            Assert.AreEqual("Europe/London", friend.Timezone);
            Assert.AreEqual(0, friend.Weight);
        }
    }
}