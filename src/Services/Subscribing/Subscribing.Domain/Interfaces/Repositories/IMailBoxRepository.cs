using Notifications.Core.SeedWork;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Subscribing.Domain.Interfaces.Repositories
{
    public interface IMailBoxRepository : IRepository<MailBoxItem>, IRepositoryBase<MailBoxItem>
    {
        //    Task<ApplicationEvent> GetApplicationEventAgragate(Guid applicationEventId);

        Task<IEnumerable<MailBoxItem>> GetEmailBox(string partyId);

        Task<MailBoxItem> GetWithRelatedByIdAsync(Guid messageId, string recipientPartyId);

        Task<MailBoxItem> GetByIdAsync(Guid messageId, string recipientPartyId);

        Task<IEnumerable<MailBoxItem>> GetDirectSentEmails(string senderPartyId);

        Task<IEnumerable<MailBoxItem>> GetDeletedEmails(string senderPartyId);
    }
}