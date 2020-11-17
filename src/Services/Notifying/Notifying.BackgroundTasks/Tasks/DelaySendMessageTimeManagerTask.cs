using EventBus.Abstractions;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using Notification.BackgroundTasks.Events;
using Notifying.API.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Notification.BackgroundTasks.Tasks
{
    public class DelaySendMessageTimeManagerService : BackgroundService
    {
        private readonly ILogger<DelaySendMessageTimeManagerService> _logger;
        private readonly BackgroundTaskSettings _settings;
        private readonly IEventBus _eventBus;

        public DelaySendMessageTimeManagerService(IOptions<BackgroundTaskSettings> settings, IEventBus eventBus, ILogger<DelaySendMessageTimeManagerService> logger)
        {
            _settings = settings?.Value ?? throw new ArgumentNullException(nameof(settings));
            _eventBus = eventBus ?? throw new ArgumentNullException(nameof(eventBus));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogDebug("DelaySendMessageTimeManagerService is starting.");

            stoppingToken.Register(() => _logger.LogDebug("#1 DelaySendMessageTimeManagerService background task is stopping."));

            while (!stoppingToken.IsCancellationRequested)
            {
                _logger.LogDebug("DelaySendMessageTimeManagerService background task is doing background work.");

                var concludedMessages = GetConcludedDaleyTimeMessageIdsTimeForNotificationType();

                foreach (var messageId in concludedMessages)
                {
                    var expiredDelayTime = new DelaySendMessageTimeExpiredIntegrationEvent(messageId);
                    _logger.LogInformation("----- Publishing integration event: {IntegrationEventId} from {AppName} - ({@IntegrationEvent})", messageId, Program.AppName, expiredDelayTime);
                    _eventBus.Publish(expiredDelayTime);
                }

                await Task.Delay(_settings.CheckUpdateTime, stoppingToken);
            }

            _logger.LogDebug("DelaySendMessageTimeManagerService background task is stopping.");

            await Task.CompletedTask;
        }

        private List<Guid> GetConcludedDaleyTimeMessageIdsTimeForNotificationType()
        {
            _logger.LogDebug("Checking Delay time for the Pending Messages");

            var daleySendMessageTime = _settings.DelaySendMessageTime;

            var concludedMessagesIdDaleyTime = new List<Guid>();
            IMongoDatabase _database = null;

            var client = new MongoClient(_settings.MongoConnectionString);
            if (client != null)
                _database = client.GetDatabase(_settings.MongoDatabase);
            else
            {
                _logger.LogCritical("FATAL ERROR: Database connections could not be opened ConnectionString {Message}", _settings.MongoConnectionString);
            }

            var filter = Builders<Message>.Filter.Eq("MessageStatusId", 1);

            var pendingMessages = _database.GetCollection<Message>("Messages").Find(filter).ToList();

            foreach (var message in pendingMessages)
            {
                var firstDelaySendMinuteNotification = message.Notifications.Min(x => x.DelaySendMinutes);
                if (DateTime.UtcNow >= message.CreatedDate.AddMinutes(firstDelaySendMinuteNotification))
                {
                    concludedMessagesIdDaleyTime.Add(message.Id);
                }
            }

            //using (var conn = new SqlConnection(_settings.NotificationConnectionString))
            //{
            //    try
            //    {
            //        conn.Open();
            //        concludedMessagesIdDaleyTime = conn.Query<Guid>(
            // @"SELECT msg.Id from Message  msg
            // JOIN MessageNotification noti
            // ON msg.Id = noti.MessageId
            // WHERE msg.MessageStatusId = 1
            // GROUP BY msg.Id, msg.CreatedDate
            // HAVING DATEDIFF(minute, msg.CreatedDate, GETDATE()) >= min(noti.DelaySendMinutes)"
            //).ToList();
            //    }
            //    catch (SqlException exception)
            //    {
            //        _logger.LogCritical(exception, "FATAL ERROR: Database connections could not be opened: {Message}", exception.Message);
            //    }
            //}

            return concludedMessagesIdDaleyTime;
        }
    }
}