namespace Fitbit.Models
{
    /// <summary>
    /// Represents an existing subscription for a user: 
    /// https://wiki.fitbit.com/display/API/Fitbit+Subscriptions+API#FitbitSubscriptionsAPI-Listexistingsubscriptions
    /// </summary>
    public class ApiSubscription
    {
        //public string CollectionType { get; set; }
        public APICollectionType CollectionType { get; set; }
        public string OwnerId { get; set; }
        public string SubscriberId { get; set; }
        public string SubscriptionId { get; set; }
    }
}
