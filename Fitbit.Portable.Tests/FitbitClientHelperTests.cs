using System;
using System.Collections.Generic;
using Fitbit.Api.Portable;
using Fitbit.Models;
using NUnit.Framework;

namespace Fitbit.Portable.Tests
{
    [TestFixture]
    public class FitbitClientExtensionsTests
    {
        [Test]
        [Category("Portable")]
        public void ProcessSleepData_NullHandled()
        {
            FitbitClientExtensions.ProcessSleepData(null);
        }

        [Test]
        [Category("Portable")]
        public void ProcessSleepData_NullSleepHandled()
        {
            FitbitClientExtensions.ProcessSleepData(new SleepData());
        }

        [Test]
        [Category("Portable")]
        public void ProcessSleepData_NullMinuteDataHandled()
        {
            var sleep = new SleepData
            {
                Sleep = new List<SleepLog>
                {
                    new SleepLog()
                }
            };
            FitbitClientExtensions.ProcessSleepData(sleep);
        }

        [Test]
        [Category("Portable")]
        public void ProcessSleepData_MinuteDataToday()
        {
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

            FitbitClientExtensions.ProcessSleepData(sleep);

            Assert.AreEqual(new DateTime(2014, 10, 10, 23, 0, 0), sleep.Sleep[0].MinuteData[0].DateTime);
        }

        [Test]
        [Category("Portable")]
        public void ProcessSleepData_MinuteDataTomorrow()
        {
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

            FitbitClientExtensions.ProcessSleepData(sleep);

            Assert.AreEqual(new DateTime(2014, 10, 11, 4, 0, 0), sleep.Sleep[0].MinuteData[0].DateTime);
        }
    }
}