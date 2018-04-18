using System;
using System.Collections.Generic;
using System.Text;

namespace Lykke.Service.Zcash.SignService.Core.Domain.Transactions
{
    public class Utxo
    {
        public string TxId { get; set; }
        public uint Vout { get; set; }
        public string ScriptPubKey { get; set; }
    }
}
