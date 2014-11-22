using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fitbit.Models
{
    public class WaterData
    {
        public List<WaterLog> Water { get; set; }
        public WaterSummary Summary { get; set; } //amount of LIQUIDS units in the selected unit system
    }
}
