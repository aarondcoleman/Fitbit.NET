using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fitbit.Models
{
    //    "trackerAlarms":[
    //    {
    //        "alarmId":<value>,
    //        "deleted":<true|false>,
    //        "enabled":<true|false>,
    //        "label":<value>,
    //        "recurring":<true|false>,
    //        "snoozeCount":<value>,
    //        "snoozeLength":<value>,
    //        "syncedToDevice":<true|false>,
    //        "time":<value>,
    //        "vibe":<value>,
    //        "weekDays":[
    //            <...>
    //            "TUESDAY",
    //            "WEDNESDAY",
    //            "THURSDAY"
    //            <...>
    //        ]
    //    },
    //    <...>
    //]

    public class TrackerAlarm
    {

        public int AlarmId { get; set; }
        public bool Deleted { get; set; }
        public bool Enabled { get; set; }
        public string Label { get; set; }
        public bool Recurring { get; set; }
        public int SnoozeCount { get; set; }
        public int SnoozeLength { get; set; }
        public bool SyncedToDevice { get; set; }
        public string Time { get; set; }
        public string Vibe { get; set; }
        public List<string> WeekDays { get; set; }

        public TrackerAlarm()
        {
            WeekDays = new List<string>();
        }
    }

}
