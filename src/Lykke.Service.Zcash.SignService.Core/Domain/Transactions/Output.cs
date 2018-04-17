using System;
using System.Collections.Generic;
using System.Text;

namespace Lykke.Service.Zcash.SignService.Core.Domain.Transactions
{
    public class Output
    {
        public string txid { get; set; }
        public uint vout { get; set; }
        public string scriptPubKey { get; set; }
        public string redeemScript { get; set; }
    }
}
