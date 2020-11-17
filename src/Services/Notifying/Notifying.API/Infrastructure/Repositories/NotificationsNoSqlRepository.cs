using Microsoft.Extensions.Options;
using MongoDB.Driver;
using Notifications.Core.Mediator;
using Notifying.API.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Notifying.API.Infrastructure.Repositories
{
    public class Seetings
    {
    }

    public class NotificationsNoSqlRepository : INotificationsNoSqlRepository
    {
        private readonly NotificationsNoSqlContext _context;
        private readonly IMediatorHandler _mediatorHandler;

        public NotificationsNoSqlRepository(IOptions<NotifyingSettings> settings, IMediatorHandler mediatorHandler)
        {
            _context = new NotificationsNoSqlContext(settings);
            _mediatorHandler = mediatorHandler;
        }

        public async Task AddMessage(Message message)
        {
            try
            {
                await _context.Messages.InsertOneAsync(message);

                await _mediatorHandler.DispatchDomainEventsAsync(message);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task UpdateMessage(Message message)
        {
            try
            {
                await _context.Messages.ReplaceOneAsync(
                doc => doc.Id == message.Id, message);

                await _mediatorHandler.DispatchDomainEventsAsync(message);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<Message> GetMesssageAsync(Guid messageId)
        {
            var filter = Builders<Message>.Filter.Eq("Id", messageId);
            return await _context.Messages.Find(filter).FirstAsync();
        }

        public async Task<IEnumerable<Message>> GetPendingMessagesWithNotificationsAsync()
        {
            var filter = Builders<Message>.Filter.Eq("MessageStatusId", MessageStatus.Pending.Id);
            return await _context.Messages

                                 .Find(filter)
                                 .ToListAsync();
        }

        public async Task<Message> GetPendingMessagesByPartyIdAndApplicationEventGroup(List<string> listSubscribersId, string applicationEventName)
        {
            var filter = Builders<Message>.Filter.Eq("MessageStatusId", MessageStatus.Pending.Id);

            var result = await _context.Messages
                                 .Find(filter)
                                 .ToListAsync();


            //TODO: do a single query on MongoDb
            var test = result.Where(m => m.Notifications.Any(n => n.ApplicationEventName == applicationEventName && (n.NotificationSubscribersPartyIds.Any(sub => listSubscribersId.Any(t1 => sub.Contains(t1)))))).FirstOrDefault();

            return test;
        }
    }
}