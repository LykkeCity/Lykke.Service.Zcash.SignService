using System;
using System.Threading.Tasks;
using Lykke.Service.Zcash.SignService.Core.Domain.Transactions;

namespace Lykke.Service.Zcash.SignService.Core.Services
{
    public interface ITransactionService
    {
        Task<string> Sign(string tx, Utxo[] outputs, string[] keys);
    }
}
