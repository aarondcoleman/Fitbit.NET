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
        
        // todo: doesn't seem to be the right place for this; it'll do at the moment
        [Test] [Category("Portable")]
        public void ProcessSleepData_NullHandled()
        {
            var client = new FitbitClient("key", "secret", "token", "secret");
            client.ProcessSleepData(null);
        }

        [Test] [Category("Portable")]
        public void ProcessSleepData_NullSleepHandled()
        {
            var client = new FitbitClient("key", "secret", "token", "secret");
            client.ProcessSleepData(new SleepData());
        }

        [Test] [Category("Portable")]
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

        [Test] [Category("Portable")]
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

        [Test] [Category("Portable")]
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