using Subscribing.Domain;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Subscribing.Application.Queries.ViewModels
{
    public class SubscriberViewModel
    {
        [DisplayName("Party Id")]
        [Required]
        public string SubscriberPartyId { get; set; }

        [DataType(DataType.EmailAddress, ErrorMessage = "E-mail is not valid")]
        public string Email { get; set; }

        [StringLength(100, MinimumLength = 2, ErrorMessage = "Name must be between 2 and 100 char")]
        public string Name { get; set; }

        [DisplayName("Subscriber Groups")]
        public ICollection<SubscriberGroupSubscriber> SubscriberGroupSubscriber { get; set; }

        public ICollection<SubscriberApplicationEvent> SubscriberApplicationEvents { get; set; }

        public ICollection<MailBoxItem> RecipientMailBoxItems { get; set; }
    }
}