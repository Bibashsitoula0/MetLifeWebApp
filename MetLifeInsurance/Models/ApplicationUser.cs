using Microsoft.AspNetCore.Identity;

namespace MetLifeInsurance.Models
{
    public class ApplicationUser : IdentityUser
    {
        public bool is_active { get; set; }
    }
}
