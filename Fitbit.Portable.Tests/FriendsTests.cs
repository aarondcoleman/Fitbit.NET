using System;
using System.Collections.Generic;
using System.Linq;
using Fitbit.Api.Portable;
using Fitbit.Models;
using NUnit.Framework;

namespace Fitbit.Portable.Tests
{
    [TestFixture]
    public class FriendsTests
    {

        // todo: fitbit client calls for friends resource


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
            string content = "GetFriends-Multiple.json".GetContent();
            var deserializer = new JsonDotNetSerializer();
            
            List<UserProfile> friends = deserializer.GetFriends(content);

            Assert.IsNotNull(friends);
            Assert.IsTrue(friends.Count == 3);

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

        [Test]
        public void Can_Deserialize_Friends_Single()
        {
            string content = "GetFriends-Single.json".GetContent();
            var deserializer = new JsonDotNetSerializer();

            List<UserProfile> friends = deserializer.GetFriends(content);

            Assert.IsNotNull(friends);
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

        [Test]
        public void Can_Deserialize_Friends_None()
        {
            string content = "GetFriends-None.json".GetContent();
            var deserializer = new JsonDotNetSerializer();

            List<UserProfile> friends = deserializer.GetFriends(content);

            Assert.IsNotNull(friends);
            Assert.IsTrue(friends.Count == 0);
        }
    }
}