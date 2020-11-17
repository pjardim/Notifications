using System.Collections.Generic;
using System.Linq;

namespace Subscribing.Application.Queries.ViewModels
{
    public class MailBoxViewModel
    {
        public MailBoxViewModel()
        {
            MailBoxItems = new List<MailBoxItemViewModel>();
        }

        public string SubscriberPartyId { get; set; }
        public SubscriberViewModel subscriber { get; set; }

        public IEnumerable<MailBoxItemViewModel> MailBoxItems { get; set; }

        public MailBoxItemViewModel MailItem { get; set; }

        public int TotalItens => MailBoxItems.ToList().Count();

        public int TotaUnreadlItens => MailBoxItems.Where(x => x.Read == false).Count();
    }
}