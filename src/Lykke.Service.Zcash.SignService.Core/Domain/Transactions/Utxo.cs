using Newtonsoft.Json;

namespace Lykke.Service.Zcash.SignService.Core.Domain.Transactions
{
    public class Utxo
    {
        [JsonProperty("txid")]
        public string TxId { get; set; }

        [JsonProperty("vout")] 
        public uint Vout { get; set; }

        [JsonProperty("scriptPubKey")]
        public string ScriptPubKey { get; set; }

        [JsonProperty("redeemScript")]
        public string RedeemScript { get; set; }

        // contrary to https://bitcoin.org/en/developer-reference#signrawtransaction amount is required for proper signing,
        // without amount you'll get invalid sign and "mandatory-script-verify-flag-failed" error on sending,
        // see https://github.com/bitcoin/bitcoin/issues/12429 for details
        // that's actual for Zcash as Bitcoin clone

        [JsonProperty("amount")]
        public decimal Amount { get; set; }
    }
}
