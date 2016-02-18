using System;

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
        public string AboutMe { get; set; }
        public string Avatar { get; set; }
        public string Avatar150 { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string DisplayName { get; set; }
        public string DistanceUnit { get; set; }
        public string EncodedId { get; set; }
        public string FoodsLocale { get; set; }
        public string FullName { get; set; }
        public Gender Gender { get; set; }
        public string GlucoseUnit { get; set; }
        public double Height { get; set; }
        public string HeightUnit { get; set; }
        public string Locale { get; set; }
        public DateTime MemberSince { get; set; }
        public string Nickname { get; set; }
        public int OffsetFromUTCMillis { get; set; }
        public string StartDayOfWeek { get; set; }
        public string State { get; set; }
        public double StrideLengthRunning { get; set; }
        public double StrideLengthWalking { get; set; }
        public string Timezone { get; set; }
        public string VolumeUnit { get; set; }
        public string WaterUnit { get; set; }
        public double Weight { get; set; }
        public string WeightUnit { get; set; }
    }
}