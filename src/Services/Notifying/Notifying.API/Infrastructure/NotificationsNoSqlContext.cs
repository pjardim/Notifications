using Microsoft.Extensions.Options;
using MongoDB.Driver;
using Notifying.API.Model;

namespace Notifying.API.Infrastructure
{
    public class NotificationsNoSqlContext
    {
        private readonly IMongoDatabase _database = null;

        public NotificationsNoSqlContext(IOptions<NotifyingSettings> settings)
        {
            var client = new MongoClient(settings.Value.MongoConnectionString);
            if (client != null)
                _database = client.GetDatabase(settings.Value.MongoDatabase);
        }

        public IMongoCollection<Message> Messages
        {
            get
            {
                return _database.GetCollection<Message>("Messages");
            }
        }


    }
}