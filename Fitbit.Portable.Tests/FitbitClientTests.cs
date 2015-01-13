using System;
using System.Collections.Generic;
using Fitbit.Api.Portable;
using Fitbit.Models;
using NUnit.Framework;

namespace Fitbit.Portable.Tests
{
    [TestFixture]
    public class FitbitClientTests
    {
        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Constructor_ConsumerKey_Empty()
        {
            new FitbitClient(string.Empty, "secret", "access", "accessSecret");
        }

        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Constructor_ConsumerKey_Null()
        {
            new FitbitClient(null, "secret", "access", "accessSecret");
        }

        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Constructor_ConsumerSecret_Empty()
        {
            new FitbitClient("key", string.Empty, "access", "accessSecret");
        }

        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Constructor_ConsumerSecret_Null()
        {
            new FitbitClient("key", null, "access", "accessSecret");
        }

        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Constructor_AccesToken_Empty()
        {
            new FitbitClient("key", "secret", string.Empty, "accessSecret");
        }

        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Constructor_AccessToken_Null()
        {
            new FitbitClient("key", "secret", null, "accessSecret");
        }

        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Constructor_AccessSecret_Empty()
        {
            new FitbitClient("key", "secret", "access", null);
        }

        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Constructor_AccessSecret_Null()
        {
            new FitbitClient("key", "secret", "access", string.Empty);
        }

        [Test]
        public void Constructor_HttpClient_Create()
        {
            var client = new FitbitClient("key", "secret", "access", "accessToken");
            Assert.IsNotNull(client.HttpClient);
        }

        // todo: doesn't seem to be the right place for this; it'll do at the moment
        [Test]
        public void ProcessSleepData_NullHandled()
        {
            var client = new FitbitClient("key", "secret", "token", "secret");
            client.ProcessSleepData(null);
        }

        [Test]
        public void ProcessSleepData_NullSleepHandled()
        {
            var client = new FitbitClient("key", "secret", "token", "secret");
            client.ProcessSleepData(new SleepData());
        }

        [Test]
        public void ProcessSleepData_NullMinuteDataHandled()
        {
            var client = new FitbitClient("key", "secret", "token", "secret");
            var sleep = new SleepData
            {
                Sleep = new List<SleepLog>
                {
                    new SleepLog()
                }
            };
            client.ProcessSleepData(sleep);
        }

        [Test]
        public void ProcessSleepData_MinuteDataToday()
        {
            var client = new FitbitClient("key", "secret", "token", "secret");
            var sleep = new SleepData
            {
                Sleep = new List<SleepLog>
                {
                    new SleepLog
                    {
                        StartTime = new DateTime(2014, 10,10, 22, 0, 0),
                        MinuteData = new List<MinuteData>
                        {
                            new MinuteData
                            {
                                DateTime = new DateTime(1900, 1, 1, 23, 0, 0) // the date part is derived
                            }
                        }
                    }
                }
            };
            client.ProcessSleepData(sleep);

            Assert.AreEqual(new DateTime(2014, 10 ,10, 23, 0, 0), sleep.Sleep[0].MinuteData[0].DateTime);
        }

        [Test]
        public void ProcessSleepData_MinuteDataTomorrow()
        {
            var client = new FitbitClient("key", "secret", "token", "secret");
            var sleep = new SleepData
            {
                Sleep = new List<SleepLog>
                {
                    new SleepLog
                    {
                        StartTime = new DateTime(2014, 10,10, 22, 0, 0),
                        MinuteData = new List<MinuteData>
                        {
                            new MinuteData
                            {
                                DateTime = new DateTime(1900, 1, 1, 4, 0, 0) // the date part is derived
                            }
                        }
                    }
                }
            };
            client.ProcessSleepData(sleep);

            Assert.AreEqual(new DateTime(2014, 10, 11, 4, 0, 0), sleep.Sleep[0].MinuteData[0].DateTime);
        }
    }
}