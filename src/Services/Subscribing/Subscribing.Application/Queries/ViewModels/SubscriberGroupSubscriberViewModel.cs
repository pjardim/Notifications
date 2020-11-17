namespace Subscribing.Application.Queries.ViewModels
{
    public class SubscriberGroupSubscriberViewModel
    {
        public string SubscriberPartyId { get; private set; }
        public SubscriberViewModel Subscriber { get; private set; }

        public int SubscriberGroupId { get; private set; }
        public SubscriberGroupViewModel SubscriberGroup { get; private set; }
    }
}