namespace Fitbit.Models
{
    /*
    <activityId>51007</activityId>
            <activityParentId>90019</activityParentId>
            <calories>230</calories>
            <description>7mph</description>
            <distance>2.04</distance>
            <duration>1097053</duration>
            <hasStartTime>false</hasStartTime>
            <isFavorite>true</isFavorite>
            <logId>1154701</logId>
            <name>Treadmill, 0% Incline</name>
            <startTime>00:00</startTime>
            <steps>3783</steps>
    */

    public class ActivityLog
    {
        public long ActivityId {get; set;}
        public long ActivityParentId { get; set; } //?
        public int Calories { get; set; } //?
        public string Description { get; set; }
        public float Distance { get; set; } //
        public long Duration { get; set; }
        public bool HasStartTime { get; set; }
        public bool IsFavorite { get; set; }
        public long LogId { get; set; }
        public string Name { get; set; }
        public string StartTime { get; set; }
        public int Steps { get; set; }
    }
}
