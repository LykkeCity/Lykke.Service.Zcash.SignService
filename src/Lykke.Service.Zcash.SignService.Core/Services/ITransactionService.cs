using System;
using System.Collections.Generic;
using System.Text;
using NBitcoin;

namespace Lykke.Service.Zcash.SignService.Core.Services
{
    public interface ITransactionService
    {
        string Sign(Transaction tx, ICoin[] coins, Key[] keys);
    }
}
