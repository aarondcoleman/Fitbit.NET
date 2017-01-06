using Fitbit.Api.Portable;

namespace Fitbit.Models
{
    /// <summary>
    /// All time series types that can be queried from fitbit
    /// </summary>
    public enum TimeSeriesResourceType
    {
        // Class inspired by : https://github.com/Fitbit/fitbit4j/blob/master/fitbit4j/src/main/java/com/fitbit/api/common/model/timeseries/TimeSeriesResourceType.java
        // StringEnum implementation: http://stackoverflow.com/questions/424366/c-sharp-string-enums

        //food
        [StringValue("/foods/log/caloriesIn")]
        CaloriesIn,
        [StringValue("/foods/log/water")]
        Water,
        //activity
        [StringValue("/activities/calories")]
        CaloriesOut,
        [StringValue("/activities/caloriesBMR")]
        CaloriesBMR,
        [StringValue("/activities/steps")]
        Steps,
        [StringValue("/activities/distance")]
        Distance,
        [StringValue("/activities/minutesSedentary")]
        MinutesSedentary,
        [StringValue("/activities/minutesLightlyActive")]
        MinutesLightlyActive,
        [StringValue("/activities/minutesFairlyActive")]
        MinutesFairlyActive,
        [StringValue("/activities/minutesVeryActive")]
        MinutesVeryActive,
        // removed from Fitbit API:  https://groups.google.com/forum/#!topic/fitbit-api/8IRaX6RW7g4
        //[StringValue("/activities/activeScore")]
        //ActiveScore,
        [StringValue("/activities/activityCalories")]
        ActivityCalories,
        [StringValue("/activities/floors")]
        Floors,
        [StringValue("/activities/elevation")]
        Elevation,
        //tracker activity
        [StringValue("/activities/tracker/calories")]
        CaloriesOutTracker,
        [StringValue("/activities/tracker/steps")]
        StepsTracker,
        [StringValue("/activities/tracker/distance")]
        DistanceTracker,
        // removed from Fitbit API:  https://groups.google.com/forum/#!topic/fitbit-api/8IRaX6RW7g4
        //[StringValue("/activities/tracker/activeScore")]
        //ActiveScoreTracker,
        [StringValue("/activities/tracker/activityCalories")]
        ActivityCaloriesTracker,
        [StringValue("/activities/tracker/floors")]
        FloorsTracker,
        [StringValue("/activities/tracker/elevation")]
        ElevationTracker,
        [StringValue("/activities/tracker/minutesSedentary")]
        MinutesSedentaryTracker,
        [StringValue("/activities/tracker/minutesLightlyActive")]
        MinutesLightlyActiveTracker,
        [StringValue("/activities/tracker/minutesFairlyActive")]
        MinutesFairlyActiveTracker,
        [StringValue("/activities/tracker/minutesVeryActive")]
        MinutesVeryActiveTracker,
        //sleep
        [StringValue("/sleep/minutesAsleep")]
        MinutesAsleep,
        [StringValue("/sleep/minutesAwake")]
        MinutesAwake,
        [StringValue("/sleep/awakeningsCount")]
        AwakeningsCount,
        [StringValue("/sleep/timeInBed")]
        TimeInBed,
        [StringValue("/sleep/minutesToFallAsleep")]
        MinutesToFallAsleep,
        [StringValue("/sleep/minutesAfterWakeup")]
        MinutesAfterWakeup,
        [StringValue("/sleep/startTime")]
        TimeEnteredBed,
        [StringValue("/sleep/efficiency")]
        SleepEfficiency,
        //body
        [StringValue("/body/weight")]
        Weight,
        [StringValue("/body/bmi")]
        BMI,
        [StringValue("/body/fat")]
        Fat        
    }   
}
