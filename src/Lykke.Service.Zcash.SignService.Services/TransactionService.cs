using System;
using System.Collections.Generic;
using System.Linq;
using Lykke.Service.Zcash.SignService.Core.Services;
using NBitcoin;
using NBitcoin.Policy;

namespace Lykke.Service.Zcash.SignService.Services
{
    public class TransactionService : ITransactionService
    {
        public string Sign(Transaction tx, ICoin[] coins, Key[] keys)
        {
            var builder = new TransactionBuilder().AddCoins(coins).AddKeys(keys);

            var signed = builder.SignTransaction(tx);

            // check transaction sign only due to differences between BTC and ZEC fee calculation
            if (!builder
                .SetTransactionPolicy(new StandardTransactionPolicy { CheckFee = false })
                .Verify(signed, out var errors))
            {
                throw new InvalidOperationException($"Invalid transaction sign: {string.Join("; ", errors.Select(e => e.ToString()))}");
            }

            return signed.ToHex();
        }
    }
}
