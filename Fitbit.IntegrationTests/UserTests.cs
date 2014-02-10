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
            foreach (UserProfile friend in userFriends)
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
            IntradayData intradayData = client.GetIntraDayTimeSeries(IntradayResourceType.CaloriesOut, new DateTime(2012, 10, 1, 0, 0, 0), new TimeSpan(24, 0, 0));

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

            if (firstReportDate.HasValue)
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

        [Test]
        public void Retrieve_Fat_Last_Week()
        {
            DateTime startDate = DateTime.Now.AddDays(-7);
            DateTime endDate = DateTime.Now;
            Fat fat = client.GetFat(startDate, endDate);

            Assert.IsNotNull(fat);
            Assert.IsNotNull(fat.FatLogs);

            Assert.IsTrue(fat.FatLogs.Count > 0);
            FatLog firstFat = fat.FatLogs[0];
            Assert.GreaterOrEqual(firstFat.DateTime, startDate.Date);
            Assert.Less(firstFat.DateTime, endDate.AddDays(1).Date);
            Assert.IsTrue(firstFat.LogId > 0);
            Assert.IsTrue(firstFat.Fat > 0);
        }

        [Test]
        public void Retrieve_Food_Yesterday()
        {
            Food food = client.GetFood(DateTime.Today);

            Assert.IsNotNull(food);
            Assert.IsNotNull(food.Foods);
            Assert.IsNotNull(food.Goals);
            Assert.IsNotNull(food.Summary);

            Assert.IsTrue(food.Foods.Count > 0);
            FoodLog foodLog = food.Foods[0];
            Assert.AreEqual(foodLog.LogDate, DateTime.Today.Date);
            Assert.IsNotNull(foodLog.LoggedFood);
            Assert.IsNotNull(foodLog.NutritionalValues);
        }

        [Test]
        public void Retrieve_Sleep_On_Date()
        {
            DateTime sleepRecordDate = new DateTime(2013, 12, 10); //find a date you know your user has sleep logs
            SleepData sleepData = client.GetSleep(sleepRecordDate);

            Assert.IsNotNull(sleepData);
            //Assert.IsTrue(sleepData.Sleep.SleepLog.Count > 0);

            Assert.IsNotNull(sleepData.Summary);
            Assert.IsTrue(sleepData.Summary.TotalMinutesAsleep > 0);

            Assert.AreEqual(sleepRecordDate.Day, sleepData.Sleep.First().StartTime.Day);
            Assert.IsTrue(sleepData.Sleep.Count > 0); //make sure there is at least one sleep log
        }

        [Test]
        public void Retrieve_Sleep_TimeSeries()
        {
            TimeSeriesDataListInt dataList = client.GetTimeSeriesInt(TimeSeriesResourceType.TimeInBed, DateTime.UtcNow, DateRangePeriod.Max, null);

            List<TimeSeriesDataListInt.Data> dataNotZero = new List<TimeSeriesDataListInt.Data>();

            foreach (var timeSeriesEvent in dataList.DataList)
            {
                if (timeSeriesEvent.Value > 0)
                    dataNotZero.Add(timeSeriesEvent);
            }

            Console.WriteLine("Total Nonzero in Time Series:" + dataNotZero.Count);

            Assert.IsNotNull(dataList);

        }


        [Test]
        public void Retrieve_BodyMeasurements_Yesterday()
        {
            BodyMeasurements measurements = client.GetBodyMeasurements(DateTime.Today.AddDays(-1));

            Assert.IsNotNull(measurements);
            Assert.IsNotNull(measurements.Body);
            Assert.IsNotNull(measurements.Goals);

        }

    }
}
