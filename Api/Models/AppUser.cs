using Microsoft.AspNetCore.Identity;

namespace Api.Models
{
    public class AppUser : IdentityUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public ICollection <OrderHeader> OrderHeaders { get; set; }
        public ShoppingCart Cart { get; set; }
    }
}