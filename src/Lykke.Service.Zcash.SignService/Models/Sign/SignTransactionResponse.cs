using Common;
using NBitcoin;
using NBitcoin.JsonConverters;

namespace Lykke.Service.Zcash.SignService.Models.Sign
{
    public class SignTransactionResponse
    {
        public SignTransactionResponse(Transaction tx, ICoin[] coins) => SignedTransaction = Serializer.ToString((tx, coins)).ToBase64();

        public string SignedTransaction { get; }
    }
}
