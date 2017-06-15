using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Fitbit.Api.Portable.Models
{






    public class RootObjectSleepRange
    {
        public SleepLogDate[] sleep { get; set; }
    }

    public class Sleep
    {
        public string dateOfSleep { get; set; }
        public int duration { get; set; }
        public int efficiency { get; set; }
        public bool isMainSleep { get; set; }
        public Levels1 levels { get; set; }
        public long logId { get; set; }
        public int minutesAfterWakeup { get; set; }
        public int minutesAsleep { get; set; }
        public int minutesAwake { get; set; }
        public int minutesToFallAsleep { get; set; }
        public DateTime startTime { get; set; }
        public int timeInBed { get; set; }
        public string type { get; set; }
    }

    public class Levels1
    {
        public Summary1 summary { get; set; }
        public LevelsShortdata[] data { get; set; }
        public LevelsShortdata[] shortData { get; set; }
    }

    public class Summary1
    {
        public Deep deep { get; set; }
        public Light light { get; set; }
        public Rem rem { get; set; }
        public Wake wake { get; set; }
    }


}
