using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankKazungV2.Shared.Models
{
    public class UserRoles
    {
        [Key]
        public int UserID { get; set; }
        public int RoleID { get; set; }
    }
}
