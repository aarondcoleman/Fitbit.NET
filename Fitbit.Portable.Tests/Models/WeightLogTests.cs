using System;
using Fitbit.Models;
using NUnit.Framework;

namespace Fitbit.Portable.Tests.Models
{
    [TestFixture]
    public class WeightLogTests
    {
        [Test] [Category("Portable")]
        public void WeightLog_DateTime_Mash()
        {
            var date = new DateTime(2014, 10, 2);
            var time = new DateTime(1970, 1, 1, 12, 45, 56);
            var weightLog = new WeightLog();
            weightLog.Date = date;
            weightLog.Time = time;

            Assert.AreEqual(new DateTime(2014, 10, 2, 12, 45, 56), weightLog.DateTime);
        }
    }
}