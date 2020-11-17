using Microsoft.AspNetCore.Identity;

namespace WebMVC.ViewModels
{
    public class ApplicationUser : IdentityUser
    {
        public string Name { get; set; }
        public string PartyId { get; set; }
    }
}