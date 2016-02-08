using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Fitbit.Models
{
    public class Activity
    {
        public ActivitySummary Summary { get; set; }
        public List<ActivityLog> Activities { get; set; }
        public ActivityGoals Goals { get; set; }

    }
}
