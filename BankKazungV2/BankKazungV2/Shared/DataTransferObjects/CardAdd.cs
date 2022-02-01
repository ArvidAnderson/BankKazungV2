using BankKazungV2.Shared.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankKazungV2.Shared.DataTransferObjects
{
    public class CardAdd
    {
        public string Name { get; set; } = string.Empty;
        public CardTypes Type { get; set; }
    }
}
