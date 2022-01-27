using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankKazungV2.Shared.Models
{
    public enum TransactionTypes { Account = 0, Card = 1 }
    public class Transaction
    {
        public int TransactionID { get; set; }
        public TransactionTypes Type { get; set; }
        public DateTime TransactionDate { get; set; }

        [Column(TypeName = "decimal(19,4)")]
        public decimal Amount { get; set; }

        //If Type = Account/0
        public int? SenderAccountID { get; set; }
        public int? ReciverAccountID { get; set; }
        [MaxLength(256)]
        public string? Message { get; set; } = string.Empty;
        //If Type = Card/0
        public int? SenderCardID { get; set; }
        public int? ReciverCardID { get; set; }
    }
}
