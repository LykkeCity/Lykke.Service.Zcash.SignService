using System;
using System.Linq;
using System.Threading.Tasks;
using Common.Log;
using Lykke.Service.Zcash.SignService.Core.Domain.Transactions;
using Lykke.Service.Zcash.SignService.Core.Services;
using NBitcoin;
using NBitcoin.DataEncoders;
using NBitcoin.RPC;
using NBitcoin.Zcash;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Lykke.Service.Zcash.SignService.Services
{
    public class TransactionService : ITransactionService
    {
        private readonly IBlockchainReader _blockchainReader;
        private readonly bool _isLocal;

        public TransactionService(IBlockchainReader blockchainReader = null)
        {
            _blockchainReader = blockchainReader;
            _isLocal = _blockchainReader == null;
        }

        public async Task<string> SignAsync(string tx, Utxo[] outputs, string[] keys)
        {
            if (_isLocal)
            {
                return SignLocally(tx, outputs, keys);
            }
            else
            {
                return await SignRemotelyAsync(tx, outputs, keys);
            }
        }

        public async Task<bool> ValidateNotSignedTransactionAsync(string tx)
        {
            if (string.IsNullOrEmpty(tx))
            {
                return false;
            }

            try
            {
                if (_isLocal)
                {
                    return ValidateNotSignedTransactionLocally(tx);
                }
                else
                {
                    return await ValidateNotSignedTransactionRemotelyAsync(tx);
                }
            }
            catch
            {
                return false;
            }
        }

        private string SignLocally(string tx, Utxo[] outputs, string[] keys)
        {
            var transaction = new ZcashTransaction(tx);
            var privateKeys = keys.Select(k => Key.Parse(k)).ToArray();
            var coins = outputs
                .Select(x => new Coin(uint256.Parse(x.TxId), x.Vout, Money.Coins(x.Amount), new Script(Encoders.Hex.DecodeData(x.ScriptPubKey))))
                .ToArray();

            transaction.Sign(privateKeys, coins);

            return transaction.ToHex(); 
        }

        private async Task<string> SignRemotelyAsync(string tx, Utxo[] outputs, string[] keys)
        {
            var result = await _blockchainReader.SignRawTransaction(tx, outputs, keys);

            if (result.Complete)
            {
                return result.Hex;
            }
            else
            {
                throw new InvalidOperationException("Sign is not complete");
            }
        }

        private bool ValidateNotSignedTransactionLocally(string tx)
        {
            return new ZcashTransaction(tx).Inputs.All(vin => vin.ScriptSig == null || vin.ScriptSig.Length == 0);
        }

        private async Task<bool> ValidateNotSignedTransactionRemotelyAsync(string tx)
        {
            var result = await _blockchainReader.DecodeRawTransaction(tx);

            return result != null &&
                result.Vin.All(vin => vin.ScriptSig == null || (string.IsNullOrEmpty(vin.ScriptSig.Asm) && string.IsNullOrEmpty(vin.ScriptSig.Hex)));
        }
    }
}
