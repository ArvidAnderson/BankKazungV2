using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankKazungV2.Shared.Models
{
    public class Account
    {
        public int AccountID { get; set; }
        public int UserID { get; set; }
        [MaxLength(50)]
        public string Name { get; set; } = string.Empty;
        [Column(TypeName = "decimal(19,4)")]
        public decimal Balance { get; set; }
        public DateTime CreatedAt { get; set; }
        public List<Transaction> Transactions { get; set; } = new List<Transaction>();
    }
}
