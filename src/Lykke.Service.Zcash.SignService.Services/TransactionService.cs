using Lykke.Service.Zcash.SignService.Core.Services;
using NBitcoin;

namespace Lykke.Service.Zcash.SignService.Services
{
    public class TransactionService : ITransactionService
    {
        public Transaction Sign(Transaction tx, ICoin[] coins, Key[] keys)
        {
            return new TransactionBuilder()
                .AddCoins(coins)
                .AddKeys(keys)
                .SignTransaction(tx);
        }
    }
}
