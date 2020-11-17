using Microsoft.EntityFrameworkCore;
using Notifications.Core.SeedWork;
using Subscribing.Domain;
using Subscribing.Domain.Interfaces.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Subscribing.Infrastructure.Repository
{
    public class MailBoxRepository : RepositoryBase<MailBoxItem>, IMailBoxRepository
    {
        private readonly SubscriberContext _context;

        public MailBoxRepository(SubscriberContext context) : base(context)
        {
            _context = context;
        }

        public IUnitOfWork UnitOfWork => _context;

        public async Task<IEnumerable<MailBoxItem>> GetEmailBox(string recipientPartyId)
        {
            return await _context.MailBox
                .Include(m => m.Recipient)
                .Where(x => x.RecipientPartyId == recipientPartyId && x.Deleted == false && x.Excluded == false)
                .OrderByDescending(x => x.CreatedDate).ToListAsync();
        }

        public async Task<IEnumerable<MailBoxItem>> GetDirectSentEmails(string senderPartyId)
        {
            return await _context.MailBox
                .Include(m => m.Recipient)
                .Where(x => x.SenderPartyIds == senderPartyId && x.DirectEmail == true && x.Deleted == false && x.Excluded == false)
                .OrderByDescending(x => x.CreatedDate).ToListAsync();
        }

        public async Task<IEnumerable<MailBoxItem>> GetDeletedEmails(string recipientPartyId)
        {
            return await _context.MailBox
                .Include(m => m.Recipient)
                .Where(x => x.RecipientPartyId == recipientPartyId && x.Deleted == true && x.Excluded == false)
                .OrderByDescending(x => x.CreatedDate).ToListAsync();
        }

        public async Task<MailBoxItem> GetWithRelatedByIdAsync(Guid messageId, string recipientPartyId)
        {
            return await _context.MailBox
                .Include(m => m.Recipient)
                .FirstAsync(x => x.MessageId == messageId && x.RecipientPartyId == recipientPartyId);
        }

        public async Task<MailBoxItem> GetByIdAsync(Guid messageId, string recipientPartyId)
        {
            return await _context.MailBox
                .FirstAsync(x => x.MessageId == messageId && x.RecipientPartyId == recipientPartyId);
        }
    }
}