namespace WebMVC.Infrastructure
{
    public static class API
    {
        public static class Notification
        {
            public static string GetAllNotificationsType(string baseUri)
            {
                return baseUri;
            }
        }

        public static class ApplicationEvent
        {
            public static string ApplicationEventEndPoint => "ApplicationEvent";

            public static string CreateApplicationEvent(string baseUri)
            {
                return $"{baseUri}/{ApplicationEventEndPoint}/Create";
            }

            internal static string GetAllApplicationEventsAsync(string baseUri)
            {
                return $"{baseUri}/{ApplicationEventEndPoint}/GetAllApplicationEventsAsync";
            }
        }
    }
}