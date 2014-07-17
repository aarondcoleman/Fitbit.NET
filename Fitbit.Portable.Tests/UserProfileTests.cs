using System;
using System.IO;
using Fitbit.Api.Portable;
using Fitbit.Models;
using Fitbit.Portable.Tests.Helpers;
using NUnit.Framework;

namespace Fitbit.Portable.Tests
{
    [TestFixture]
    public class UserProfileTests
    {
        [Test]
        public void Can_Deserialize_Profile()
        {
            string content = File.ReadAllText(SampleData.PathFor("UserProfile.txt"));
            var deserializer = new JsonDotNetSerializer();
            deserializer.RootProperty = "user";

            UserProfile result = deserializer.Deserialize<UserProfile>(content);

            Assert.IsNotNull(result);
            Assert.AreEqual("http://www.fitbit.com/images/profile/defaultProfile_100_male.gif", result.Avatar);
            Assert.AreEqual("http://www.fitbit.com/images/profile/defaultProfile_150_male.gif", result.Avatar150);
            Assert.AreEqual("GB", result.Country);
            Assert.AreEqual(new DateTime(1983, 1, 28), result.DateOfBirth);
            Assert.AreEqual("Adam", result.DisplayName);
            Assert.AreEqual("en_US", result.DistanceUnit);
            Assert.AreEqual("XXXXXX", result.EncodedId);
            Assert.AreEqual("en_US", result.FoodsLocale);
            Assert.AreEqual("Fitbit User", result.FullName);
            Assert.AreEqual(Gender.MALE, result.Gender);
            Assert.AreEqual("METRIC", result.GlucoseUnit);
            Assert.AreEqual((double) 170.2, result.Height);
            Assert.AreEqual("en_US", result.HeightUnit);
            Assert.AreEqual("en_GB", result.Locale);
            Assert.AreEqual(new DateTime(2014, 1, 6), result.MemberSince);
            Assert.AreEqual(-25200000, result.OffsetFromUTCMillis);
            Assert.AreEqual("SUNDAY", result.StartDayOfWeek);
            Assert.AreEqual(88.5, result.StrideLengthRunning);
            Assert.AreEqual(70.60000000000001, result.StrideLengthWalking);
            Assert.AreEqual("Europe/London", result.Timezone);
            Assert.AreEqual("METRIC", result.WaterUnit);
            Assert.AreEqual((double) 79.3, result.Weight);
            Assert.AreEqual("METRIC", result.WeightUnit);
        }
    }
}