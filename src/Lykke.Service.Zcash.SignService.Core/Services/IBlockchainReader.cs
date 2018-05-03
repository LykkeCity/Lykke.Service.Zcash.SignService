using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Lykke.Service.Zcash.SignService.Core.Domain.Transactions;

namespace Lykke.Service.Zcash.SignService.Services
{
    public interface IBlockchainReader
    {
        Task<RawTransaction> DecodeRawTransaction(string tx);
        Task<SignResult> SignRawTransaction(string tx, Utxo[] outputs, string[] keys);
    }
}
