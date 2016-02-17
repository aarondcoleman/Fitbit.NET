using Fitbit.Api.Portable;

namespace Fitbit.Models
{
    public enum IntradayResourceType
    {
        // StringEnum implementation: http://stackoverflow.com/questions/424366/c-sharp-string-enums
        // Fitbit: https://wiki.fitbit.com/display/API/API-Get-Intraday-Time-Series
        //activity
        [StringValue("/activities/calories")]
        CaloriesOut,
        [StringValue("/activities/steps")]
        Steps,
        [StringValue("/activities/floors")]
        Floors,
        [StringValue("/activities/elevation")]
        Elevation
    }
}
