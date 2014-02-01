﻿using System;
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
            DateTime sleepRecordDate = new DateTime(2014, 1, 28); //find a date you know your user has sleep logs   
            SleepData sleepData = client.GetSleep(sleepRecordDate);

            Assert.IsNotNull(sleepData);
            //Assert.IsTrue(sleepData.Sleep.SleepLog.Count > 0);

            Assert.IsNotNull(sleepData.Summary);
            Assert.IsTrue(sleepData.Summary.TotalMinutesAsleep > 0);

            Assert.LessOrEqual(sleepData.Sleep.First().StartTime, sleepRecordDate);
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
        public void Log_Single_Heart_Rate_Today()
        {
            HeartRateLog log = new HeartRateLog
            {
                LogId = -1,
                HeartRate = 99,
                Time = DateTime.Now,
                Tracker = "Resting Heart Rate"
            };

            var expectedTime = new DateTime(log.Time.Year, log.Time.Month, log.Time.Day, log.Time.Hour, log.Time.Minute, 0);

            HeartRateLog response = client.LogHeartRate(log);
            Assert.AreEqual(log.HeartRate, response.HeartRate);
            Assert.AreNotEqual(-1, response.LogId);
            Assert.AreEqual(expectedTime, response.Time);
            Assert.AreEqual(log.Tracker, response.Tracker);
        }

        [Test]
        public void Delete_Heart_Rates_Today()
        {
            DateTime heartRecordDate = DateTime.Now;
            HeartRates heartRateData = client.GetHeartRates(heartRecordDate);

            foreach (var hr in heartRateData.Heart)
            {
                Assert.Greater(hr.LogId, 0);
                client.DeleteHeartRateLog(hr.LogId);
            }

            heartRateData = client.GetHeartRates(heartRecordDate);
            Assert.AreEqual(heartRateData.Heart.Count, 0);
        }

        [Test]
        public void Retrieve_HeartRates_Today()
        {
            DateTime heartRecordDate = DateTime.Now;
            HeartRates heartRateData = client.GetHeartRates(heartRecordDate);

            Assert.IsNotNull(heartRateData);
            Assert.IsNotNull(heartRateData.Average);
            Assert.IsNotNull(heartRateData.Heart);
        }

        [Test]
        public void Log_Body_Measurements_Today()
        {
            BodyMeasurement log = new BodyMeasurement()
            {
                Bicep = 10.3,
                Calf = 11.2,
                Chest = 26.2,
                Fat = 14.3,
                Forearm = 22.3,
                Hips = 10.3,
                Neck = 10.3,
                Thigh = 10.3,
                Waist = 33,
                Weight = 180,
            };

            var response = client.LogBodyMeasurement(log, DateTime.Now);

            Assert.AreEqual(log.Bicep, response.Bicep);
            Assert.AreEqual(log.Calf, response.Calf);
            Assert.AreEqual(log.Chest, response.Chest);
            Assert.AreEqual(log.Forearm, response.Forearm);
            Assert.AreEqual(log.Hips, response.Hips);
            Assert.AreEqual(log.Neck, response.Neck);
            Assert.AreEqual(log.Thigh, response.Thigh);
            Assert.AreEqual(log.Waist, response.Waist);
            Assert.AreEqual(log.Weight, response.Weight);
        }
    }
}
