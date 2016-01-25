using System;
using Fitbit.Models;
using NUnit.Framework;

namespace Fitbit.Portable.Tests.Models
{
    [TestFixture]
    public class FatLogTests
    {
        [Test] [Category("Portable")]
        public void FatLog_DateTime_Mash()
        {
            var date = new DateTime(2014, 10, 2);
            var time = new DateTime(1970, 1, 1, 12, 45, 56);
            var fatlog = new FatLog();
            fatlog.Date = date;
            fatlog.Time = time;
            
            Assert.AreEqual(new DateTime(2014, 10, 2, 12, 45, 56), fatlog.DateTime);
        }
    }
}
