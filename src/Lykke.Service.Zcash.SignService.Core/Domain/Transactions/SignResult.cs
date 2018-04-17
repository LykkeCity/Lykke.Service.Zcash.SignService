using System;
using System.Collections.Generic;
using System.Text;

namespace Lykke.Service.Zcash.SignService.Core.Domain.Transactions
{
    public class SignResult
    {
        public string hex { get; set; }
        public bool complete { get; set; }
    }
}
