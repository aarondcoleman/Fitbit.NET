using System;
using System.Xml.Serialization;

namespace Fitbit.Models
{
    /// <summary>
    /// Class that represents the subscription information sent from Fitbit to alert of new updated resources
    /// https://wiki.fitbit.com/display/API/Fitbit+Subscriptions+API#FitbitSubscriptionsAPI-NotificationFormat
    /// </summary>
    /// 
    public class UpdatedResource
    {
        [XmlElement("collectionType")]
        public APICollectionType CollectionType { get; set; }

        [XmlElement("date")]
        public DateTime Date { get; set; }

        [XmlElement("ownerId")]
        public string OwnerId { get; set; }
        public ResourceOwnerType OwnerType { get; set; }

        [XmlElement("subscriptionId")]
        public string SubscriptionId { get; set; }
    }
}
