using System.Collections.Generic;
using Fitbit.Models;

namespace Fitbit.Api.Portable.Models.Deprecated
{
    public class SleepData
    {
        public List<SleepLog> Sleep { get; set; }
        public SleepSummary Summary { get; set; }
    }
}