namespace Notification.BackgroundTasks
{
    public class BackgroundTaskSettings
    {
        public string MongoConnectionString { get; set; }
        public string MongoDatabase { get; set; }

        public string EventBusConnection { get; set; }

        public int DelaySendMessageTime { get; set; }

        public int CheckUpdateTime { get; set; }

        public string SubscriptionClientName { get; set; }
    }
}
