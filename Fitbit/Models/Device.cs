using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Fitbit.Models
{

    //<device>
    //    <battery>Full</battery>
    //    <id>123456</id> 
    //    <lastSyncTime>2011-08-26T11:19:03.000</lastSyncTime>
    //    <type>TRACKER</type>
    //    <deviceVersion>Ultra</deviceVersion>
    //</device>

    public class Device
    {
        public string Battery { get; set; } //this should probably be something clever or a helper method to make it an emum
        public string Id { get; set; }
        public DateTime LastSyncTime { get; set; }
        public DeviceType Type { get; set; }            //again, maybe a helper method
        public string DeviceVersion { get; set; }   //again, maybe a helper method
    }
}
