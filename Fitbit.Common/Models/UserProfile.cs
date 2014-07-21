using System;
#if REQUIRES_JSONNET
using Newtonsoft.Json;
#endif

namespace Fitbit.Models
{
    /*{"user":
     * {
     * "aboutMe": "About me text",
     * "avatar":"http://www.fitbit.com/images/profile/defaultProfile_100_male.gif",
     * "avatar150":"http://www.fitbit.com/images/profile/defaultProfile_150_male.gif",
     * "country":"GB",
     * "dateOfBirth":"1983-01-28",
     * "displayName":"Adam",
     * "distanceUnit":"en_US",
     * "encodedId":"XXXXXX",
     * "foodsLocale":"en_US",
     * "fullName":"Adam Storr",
     * "gender":"MALE",
     * "glucoseUnit":"METRIC",
     * "height":170.2,
     * "heightUnit":"en_US",
     * "locale":"en_GB",
     * "memberSince":"2014-01-06",
     * "offsetFromUTCMillis":3600000,
     * "startDayOfWeek":"SUNDAY",
     * "strideLengthRunning":88.5,
     * "strideLengthWalking":70.60000000000001,
     * "timezone":"Europe/London",
     * "waterUnit":"METRIC",
     * "weight":79.3,
     * "weightUnit":"METRIC"
     * }
     * }*/


    public class UserProfile
    {
#if REQUIRES_JSONNET
        [JsonProperty("aboutMe")]
#endif
        public string AboutMe { get; set; }

#if REQUIRES_JSONNET
        [JsonProperty("avatar")]
#endif
        public string Avatar { get; set; }

#if REQUIRES_JSONNET
        [JsonProperty("avatar150")]
#endif
        public string Avatar150 { get; set; }

#if REQUIRES_JSONNET
        [JsonProperty("city")]
#endif
        public string City { get; set; }

#if REQUIRES_JSONNET
        [JsonProperty("country")]
#endif
        public string Country { get; set; }

#if REQUIRES_JSONNET
        [JsonProperty("dateOfBirth")]
#endif
        public DateTime DateOfBirth { get; set; }

#if REQUIRES_JSONNET
        [JsonProperty("displayName")]
#endif
        public string DisplayName { get; set; }

#if REQUIRES_JSONNET
        [JsonProperty("distanceUnit")]
#endif
        public string DistanceUnit { get; set; }

#if REQUIRES_JSONNET
        [JsonProperty("encodedId")]
#endif
        public string EncodedId { get; set; }

#if REQUIRES_JSONNET
        [JsonProperty("foodsLocale")]
#endif
        public string FoodsLocale { get; set; }

#if REQUIRES_JSONNET
        [JsonProperty("fullName")]
#endif
        public string FullName { get; set; }

#if REQUIRES_JSONNET
        [JsonProperty("gender")]
#endif
        public Gender Gender { get; set; }

#if REQUIRES_JSONNET
        [JsonProperty("glucoseUnit")]
#endif
        public string GlucoseUnit { get; set; }

#if REQUIRES_JSONNET
        [JsonProperty("height")]
#endif
        public double Height { get; set; }

#if REQUIRES_JSONNET
        [JsonProperty("heightUnit")]
#endif
        public string HeightUnit { get; set; }

#if REQUIRES_JSONNET
        [JsonProperty("locale")]
#endif
        public string Locale { get; set; }

#if REQUIRES_JSONNET
        [JsonProperty("memberSince")]
#endif
        public DateTime MemberSince { get; set; }

#if REQUIRES_JSONNET
        [JsonProperty("nickname")]
#endif
        public string Nickname { get; set; }

#if REQUIRES_JSONNET
        [JsonProperty("offsetFromUTCMillis")]
#endif
        public int OffsetFromUTCMillis { get; set; }

#if REQUIRES_JSONNET
        [JsonProperty("startDayOfWeek")]
#endif
        public string StartDayOfWeek { get; set; }

#if REQUIRES_JSONNET
        [JsonProperty("state")]
#endif
        public string State { get; set; }

#if REQUIRES_JSONNET
        [JsonProperty("strideLengthRunning")]
#endif
        public double StrideLengthRunning { get; set; }

#if REQUIRES_JSONNET
        [JsonProperty("strideLengthWalking")]
#endif
        public double StrideLengthWalking { get; set; }

#if REQUIRES_JSONNET
        [JsonProperty("timezone")]
#endif
        public string Timezone { get; set; }

#if REQUIRES_JSONNET
        [JsonProperty("volumeUnit")]
#endif
        public string VolumeUnit { get; set; }

#if REQUIRES_JSONNET
        [JsonProperty("waterUnit")]
#endif
        public string WaterUnit { get; set; }

#if REQUIRES_JSONNET
        [JsonProperty("weight")]
#endif
        public double Weight { get; set; }

#if REQUIRES_JSONNET
        [JsonProperty("weightUnit")]
#endif
        public string WeightUnit { get; set; }

        //public string EncodedId{ get; set;}
        //public string DisplayName { get; set; }
        //public Gender Gender { get; set; }
        //public DateTime DateOfBirth { get; set; }
        //public double Height { get; set; }
        //public double Weight { get; set; }
        //public double StrideLengthWalking { get; set; }
        //public double StrideLengthRunning { get; set; }
        //public string FullName { get; set; }
        //public string Nickname { get; set; }
        //public string Country { get; set; }
        //public string State { get; set; }
        //public string City { get; set; }
        //public string AboutMe { get; set; }
        //public DateTime MemberSince { get; set; }
        //public string Timezone { get; set; }
        ///**
        // * Millisecond offset to add to UTC to get timezone
        // */
        //public int OffsetFromUTCMillis { get; set; }
        //public string Locale { get; set; }
        //public string Avatar { get; set; }

        //public string WeightUnit { get; set; }
        //public string DistanceUnit { get; set; }
        //public string HeightUnit { get; set; }
        //public string WaterUnit { get; set; }
        //public string GlucoseUnit { get; set; }
        //public string VolumeUnit { get; set; }
    }

    public enum Gender
    {
        NA,
        MALE,
        FEMALE
    }
}
