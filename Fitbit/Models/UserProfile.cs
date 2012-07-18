using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Fitbit.Models
{
    public class UserProfile
    {
        public string EncodedId{ get; set;}
        public string DisplayName { get; set; }
        public Gender Gender { get; set; }
        public DateTime DateOfBirth { get; set; }
        public double Height { get; set; }
        public double Weight { get; set; }
        public double StrideLengthWalking { get; set; }
        public double StrideLengthRunning { get; set; }
        public string FullName { get; set; }
        public string Nickname { get; set; }
        public string Country { get; set; }
        public string State { get; set; }
        public string City { get; set; }
        public string AboutMe { get; set; }
        public DateTime MemberSince { get; set; }
        public string Timezone { get; set; }
        /**
         * Millisecond offset to add to UTC to get timezone
         */
        public int OffsetFromUTCMillis { get; set; }
        public string Locale { get; set; }
        public string Avatar { get; set; }

        public string WeightUnit { get; set; }
        public string DistanceUnit { get; set; }
        public string HeightUnit { get; set; }
        public string WaterUnit { get; set; }
        public string GlucoseUnit { get; set; }
        public string VolumeUnit { get; set; }
    }

    public enum Gender
    {
        NA,
        MALE,
        FEMALE
    }
}
