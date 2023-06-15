 using Dapper.Contrib.Extensions;
using System.ComponentModel.DataAnnotations;

namespace MetLifeInsurance.Models
{
    [Table("Users")]
    public class RegisterUser
	{
        [Dapper.Contrib.Extensions.Key]
        public long Id { get; set; }
        public string Email { get; set; }
        public string UserName { get; set; }
        public string Role { get; set; }
        public bool is_active { get; set; }

    }
}
