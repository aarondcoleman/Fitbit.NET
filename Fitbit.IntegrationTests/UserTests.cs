using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using Fitbit.Api;
using Fitbit.Models;
using System.Net;

namespace Fitbit.IntegrationTests
{
    [TestFixture]
    public class UserTests : TestsBase
    {
        [Test]
        [Ignore]
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
        [Ignore]
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
        [Ignore]
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
        [Ignore]
        public void Retrieve_Intraday_Calories()
        {
            IntradayData intradayData = client.GetIntraDayTimeSeries(IntradayResourceType.CaloriesOut, new DateTime(2014, 3, 30, 0, 0, 0), new TimeSpan(24,0,0));

            Assert.IsNotNull(intradayData);
            Assert.IsTrue(intradayData.DataSet.Count() == 1440);

            //Console.WriteLine("# of devices:" + userDevices.Count);
            //foreach (Device device in userDevices)
            //{
            //    Console.WriteLine("Device:" + device.Id + " - " + device.DeviceVersion + " - " + device.Type + " - Battery:" + device.Battery);
            //}
        }

        [Test]
        [Ignore]
        public void Retrieve_Tracker_First_Sync_Day()
        {
            DateTime? firstReportDate = client.GetActivityTrackerFirstDay();

            Assert.IsNotNull(firstReportDate);

            if (firstReportDate.HasValue)
                Console.WriteLine("User's First Tracker Sync Day:" + firstReportDate.ToString());

        }

        [Test]
        [Ignore]
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
        [Ignore]
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
        [Ignore]
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
        [Ignore]
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
        [Ignore]
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
        [Ignore]
        public void Retrieve_BodyMeasurements_Yesterday()
        {
            BodyMeasurements measurements = client.GetBodyMeasurements(DateTime.Today.AddDays(-1));

            Assert.IsNotNull(measurements);
            Assert.IsNotNull(measurements.Body);

        }

        [Test]
        [Ignore]
        public void Retrieve_BloodPressure_Yesterday()
        {
            BloodPressureData bpData = client.GetBloodPressure(DateTime.Today.AddDays(-1));

            Assert.IsNotNull(bpData);
            Assert.IsNotNull(bpData.BP);

        }

        [Test]
        [Ignore]
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

            HeartRateLog response = client.LogHeartRate(log, null);
            Assert.AreEqual(log.HeartRate, response.HeartRate);
            Assert.AreNotEqual(-1, response.LogId);
            Assert.AreEqual(expectedTime, response.Time);
            Assert.AreEqual(log.Tracker, response.Tracker);
        }

        [Test]
        [Ignore]
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
        [Ignore]
        public void Retrieve_HeartRates_Today()
        {
            DateTime heartRecordDate = DateTime.Now;
            HeartRates heartRateData = client.GetHeartRates(heartRecordDate);

            Assert.IsNotNull(heartRateData);
            Assert.IsNotNull(heartRateData.Average);
            Assert.IsNotNull(heartRateData.Heart);
        }

        [Test]
        [Ignore]
        public void Error_Retrieving_Fake_User_Profile_Data()
        {
            FitbitException exception = Assert.Throws<FitbitException>(() => client.GetUserProfile("123") );

            Assert.IsTrue(exception.ApiErrors.Count == 1);
            Assert.AreEqual(HttpStatusCode.BadRequest, exception.HttpStatusCode);
            ApiError error = exception.ApiErrors.First();
            Assert.AreEqual(ErrorType.Validation, error.ErrorType);
            Assert.AreEqual("resource owner", error.FieldName);
        }

        [Test]
        [Ignore]
        public void Retrieve_Water_On_Date()
        {
            DateTime waterRecordDate = new DateTime(2014, 11, 17); //find a date you know your user has water logs
            WaterData waterData = client.GetWater(waterRecordDate);
            float totalAmount = waterData.Water.Sum(s => s.Amount);

            Assert.IsNotNull(waterData);

            Assert.IsNotNull(waterData.Summary);
            Assert.IsTrue(waterData.Summary.Water > 0);

            Assert.IsTrue(waterData.Water.First().Amount > 0);
            Assert.IsTrue(waterData.Water.Count > 0); //make sure there is at least one water log

            Assert.AreEqual(totalAmount, waterData.Summary.Water); //total amount of water received is the sum of all logs
        }

        [Test]
        [Ignore]
        public void Log_Single_Water_On_Date()
        {
            var logDate = new DateTime(2014, 11, 17);  //the date for which the sample waterlog will be added
            WaterLog log = new WaterLog
            {
                LogId = -1,
                Amount = 300
            };


            WaterLog response = client.LogWater(logDate, log);

            Assert.IsNotNull(response);
            Assert.AreEqual(log.Amount, response.Amount);
            Assert.AreNotEqual(-1, response.LogId);
            Assert.IsTrue(response.LogId > 0);
        }

        [Test]
        [Ignore]
        public void Delete_Last_Water_On_Date()
        {
            var logDate = new DateTime(2014, 11, 17);  //find a date you know your user has water logs
            WaterData initialWaterData = client.GetWater(logDate);

            int initialWaterLogCount = initialWaterData.Water.Count;

            //we expect atleast one water log entry for this date
            long logId = initialWaterData.Water.OrderBy(s => s.LogId).Last().LogId;
            client.DeleteWaterLog(logId);

            WaterData finalWaterData = client.GetWater(logDate);

            //we expect to have 1 water log entry less than before the testrun
            Assert.AreEqual(initialWaterLogCount - 1, finalWaterData.Water.Count);
        }

        [Test]
        [Ignore]
        public void Retrieve_Water_For_User_On_Date()
        {
            DateTime waterRecordDate = new DateTime(2014, 11, 17); //find a date you know your user has water logs
            WaterData waterData = client.GetWater(waterRecordDate, Configuration.FitbitUserId);
            float totalAmount = waterData.Water.Sum(s => s.Amount);

            Assert.IsNotNull(waterData);

            Assert.IsNotNull(waterData.Summary);
            Assert.IsTrue(waterData.Summary.Water > 0);

            Assert.IsTrue(waterData.Water.First().Amount > 0);
            Assert.IsTrue(waterData.Water.Count > 0); //make sure there is at least one water log

            Assert.AreEqual(totalAmount, waterData.Summary.Water); //total amount of water received is the sum of all logs
        }

        [Test]
        [Ignore]
        public void Log_Single_Water_For_User_On_Date()
        {
            var logDate = new DateTime(2014, 11, 17);  //the date for which the sample waterlog will be added
            WaterLog log = new WaterLog
            {
                LogId = -1,
                Amount = 300
            };


            WaterLog response = client.LogWater(logDate, log, Configuration.FitbitUserId);

            Assert.IsNotNull(response);
            Assert.AreEqual(log.Amount, response.Amount);
            Assert.AreNotEqual(-1, response.LogId);
            Assert.IsTrue(response.LogId > 0);
        }

        [Test]
        [Ignore]
        public void Delete_Last_Water_For_User_On_Date()
        {
            var logDate = new DateTime(2014, 11, 17);  //find a date you know your user has water logs
            WaterData initialWaterData = client.GetWater(logDate, Configuration.FitbitUserId);

            int initialWaterLogCount = initialWaterData.Water.Count;

            //we expect atleast one water log entry for this date
            long logId = initialWaterData.Water.OrderBy(s => s.LogId).Last().LogId;
            client.DeleteWaterLog(logId, Configuration.FitbitUserId);

            WaterData finalWaterData = client.GetWater(logDate, Configuration.FitbitUserId);

            //we expect to have 1 water log entry less than before the testrun
            Assert.AreEqual(initialWaterLogCount - 1, finalWaterData.Water.Count);
        }
    }
}
