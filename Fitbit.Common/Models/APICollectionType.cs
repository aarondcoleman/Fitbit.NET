namespace Fitbit.Models
{
    public enum APICollectionType
    {
        [StringValue("activities")]
        activities,

        [StringValue("foods")]
        foods,

        [StringValue("meals")]
        meals,

        [StringValue("sleep")]
        sleep,

        [StringValue("body")]
        body,
        
        [StringValue("")]
        user,

        [StringValue("weight")]
        weight    
    }
}
