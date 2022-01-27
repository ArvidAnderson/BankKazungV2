using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankKazungV2.Shared.Models
{
    public enum CardTypes { Debit = 0, Credit = 1 }
    public class Card
    {
        public int CardID { get; set; }
        public int UserID { get; set; }
        [MaxLength(50)]
        public string Name { get; set; } = string.Empty;
        public CardTypes Type { get; set; }

        [Column(TypeName = "decimal(19,4)")]
        public decimal Balance { get; set; }

        [Column(TypeName = "decimal(19,4)")]
        public decimal CreditLimit { get; set; }

        public DateTime CreatedAt { get; set; }
        public List<Transaction> Transactions { get; set; } = new List<Transaction>();
    }
}
