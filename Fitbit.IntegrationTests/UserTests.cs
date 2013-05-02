using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using Fitbit.Api;
using RestSharp;
using Fitbit.Models;


namespace Fitbit.IntegrationTests
{
    [TestFixture]
    public class UserTests : TestsBase
    {
        [Test]
        public void Retrieve_User_Profile_Data()
        {
            UserProfile userProfile = client.GetUserProfile(Configuration.FitbitUserId);

            Assert.IsNotNull(userProfile);

            Assert.IsNotNull(userProfile.FullName);
            Console.WriteLine("FullName: " + userProfile.FullName);

            Assert.IsNotNull(userProfile.EncodedId);
            Console.WriteLine("EncodedId: " + userProfile.EncodedId);

            Assert.IsNotNull(userProfile.MemberSince);
            Assert.AreNotEqual(new DateTime(), userProfile.MemberSince); //checking for initialized but not set date
        }

        [Test]
        public void Retrieve_User_Friends()
        {
            List<UserProfile> userFriends = client.GetFriends();

            Assert.IsNotNull(userFriends);

            Console.WriteLine("# of friends:" + userFriends.Count);
            foreach(UserProfile friend in userFriends)
            {
                Console.WriteLine("Friend:" + friend.DisplayName + " - " + friend.EncodedId);
            }
        }

        [Test]
        public void Retrieve_User_Devices()
        {
            List<Device> userDevices = client.GetDevices();

            Assert.IsNotNull(userDevices);
            Assert.IsTrue(userDevices.Count > 0);

            Console.WriteLine("# of devices:" + userDevices.Count);
            foreach (Device device in userDevices)
            {
                Console.WriteLine("Device:" + device.Id + " - " + device.DeviceVersion + " - " + device.Type + " - Battery:" + device.Battery);
            }
        }

        [Test]
        public void Retrieve_Intraday_Calories()
        {
            IntradayData intradayData = client.GetIntraDayTimeSeries(IntradayResourceType.CaloriesOut, new DateTime(2012, 10, 1, 0, 0, 0), new TimeSpan(24,0,0));

            Assert.IsNotNull(intradayData);
            Assert.IsTrue(intradayData.DataSet.Count() == 1440);

            //Console.WriteLine("# of devices:" + userDevices.Count);
            //foreach (Device device in userDevices)
            //{
            //    Console.WriteLine("Device:" + device.Id + " - " + device.DeviceVersion + " - " + device.Type + " - Battery:" + device.Battery);
            //}
        }

        [Test]
        public void Retrieve_Tracker_First_Sync_Day()
        {
            DateTime? firstReportDate = client.GetActivityTrackerFirstDay();

            Assert.IsNotNull(firstReportDate);

            if(firstReportDate.HasValue)
                Console.WriteLine("User's First Tracker Sync Day:" + firstReportDate.ToString());

        }

        [Test]
        public void Retrieve_Weight_Last_Week()
        {
            DateTime startDate = DateTime.Now.AddDays(-7);
            DateTime endDate = DateTime.Now;
            Weight weights = client.GetWeight(startDate, endDate);

            Assert.IsNotNull(weights);
            Assert.IsNotNull(weights.Weights);

            Assert.IsTrue(weights.Weights.Count > 0);
            WeightLog firstWeight = weights.Weights[0];
            Assert.GreaterOrEqual(firstWeight.DateTime, startDate.Date);
            Assert.Less(firstWeight.DateTime, endDate.AddDays(1).Date);
            Assert.IsTrue(firstWeight.LogId > 0);
            Assert.IsTrue(firstWeight.Bmi > 0);
            Assert.IsTrue(firstWeight.Weight > 0);
        }
    }
}
