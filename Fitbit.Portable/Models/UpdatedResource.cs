using System;

namespace Fitbit.Models
{
    /// <summary>
    /// Class that represents the subscription information sent from Fitbit to alert of new updated resources
    /// https://wiki.fitbit.com/display/API/Fitbit+Subscriptions+API#FitbitSubscriptionsAPI-NotificationFormat
    /// </summary>
    /// 
    public class UpdatedResource
    {
        public APICollectionType CollectionType { get; set; }
        public DateTime Date { get; set; }
        public string OwnerId { get; set; }
        public ResourceOwnerType OwnerType { get; set; }
        public string SubscriptionId { get; set; }
    }
}
