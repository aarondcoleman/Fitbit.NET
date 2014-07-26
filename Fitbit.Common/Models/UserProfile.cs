using System;
using Newtonsoft.Json;

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
        [JsonProperty("aboutMe")]
        public string AboutMe { get; set; }

        [JsonProperty("avatar")]
        public string Avatar { get; set; }

        [JsonProperty("avatar150")]
        public string Avatar150 { get; set; }

        [JsonProperty("city")]
        public string City { get; set; }

        [JsonProperty("country")]
        public string Country { get; set; }

        [JsonProperty("dateOfBirth")]
        public DateTime DateOfBirth { get; set; }

        [JsonProperty("displayName")]
        public string DisplayName { get; set; }

        [JsonProperty("distanceUnit")]
        public string DistanceUnit { get; set; }

        [JsonProperty("encodedId")]
        public string EncodedId { get; set; }

        [JsonProperty("foodsLocale")]
        public string FoodsLocale { get; set; }

        [JsonProperty("fullName")]
        public string FullName { get; set; }

        [JsonProperty("gender")]
        public Gender Gender { get; set; }

        [JsonProperty("glucoseUnit")]
        public string GlucoseUnit { get; set; }

        [JsonProperty("height")]
        public double Height { get; set; }

        [JsonProperty("heightUnit")]
        public string HeightUnit { get; set; }

        [JsonProperty("locale")]
        public string Locale { get; set; }

        [JsonProperty("memberSince")]
        public DateTime MemberSince { get; set; }

        [JsonProperty("nickname")]
        public string Nickname { get; set; }

        [JsonProperty("offsetFromUTCMillis")]
        public int OffsetFromUTCMillis { get; set; }

        [JsonProperty("startDayOfWeek")]
        public string StartDayOfWeek { get; set; }

        [JsonProperty("state")]
        public string State { get; set; }

        [JsonProperty("strideLengthRunning")]
        public double StrideLengthRunning { get; set; }

        [JsonProperty("strideLengthWalking")]
        public double StrideLengthWalking { get; set; }

        [JsonProperty("timezone")]
        public string Timezone { get; set; }

        [JsonProperty("volumeUnit")]
        public string VolumeUnit { get; set; }

        [JsonProperty("waterUnit")]
        public string WaterUnit { get; set; }

        [JsonProperty("weight")]
        public double Weight { get; set; }

        [JsonProperty("weightUnit")]
        public string WeightUnit { get; set; }
    }
}
