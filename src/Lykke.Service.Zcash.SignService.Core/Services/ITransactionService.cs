using NBitcoin;

namespace Lykke.Service.Zcash.SignService.Core.Services
{
    public interface ITransactionService
    {
        Transaction Sign(Transaction tx, ICoin[] coins, Key[] keys);
    }
}
