using System;
using System.Threading.Tasks;
using Lykke.Service.Zcash.SignService.Core.Domain.Transactions;

namespace Lykke.Service.Zcash.SignService.Core.Services
{
    public interface ITransactionService
    {
        Task<string> SignAsync(string tx, Utxo[] outputs, string[] keys, uint? branchId = null);
        Task<bool> ValidateNotSignedTransactionAsync(string tx);
    }
}
