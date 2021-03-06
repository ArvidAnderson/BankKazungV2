using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankKazungV2.Shared.Models
{
    public class User
    {
        public int UserID { get; set; }
        [MaxLength(50)]
        public string FirstName { get; set; } = string.Empty;
        [MaxLength(50)]
        public string LastName { get; set; } = string.Empty;
        [MaxLength(254)]
        public string Email { get; set; } = string.Empty;
        [Column(TypeName = "varchar(50)")]
        public string Phone { get; set; } = string.Empty;
        public int Age { get; set; }
        public DateTime CreatedAt { get; set; }
        public byte[] Password { get; set; }
        public byte[] Salt { get; set; }
        public List<Card> Cards { get; set; } = new List<Card>();
        public List<Account> Accounts { get; set; } = new List<Account>();
        public List<UserRoles> Roles { get; set; } = new List<UserRoles>();
    }
}
