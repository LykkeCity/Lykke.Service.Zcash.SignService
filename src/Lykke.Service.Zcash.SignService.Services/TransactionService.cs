using System;
using System.Linq;
using System.Threading.Tasks;
using Common.Log;
using Lykke.Service.Zcash.SignService.Core.Domain.Transactions;
using Lykke.Service.Zcash.SignService.Core.Services;
using NBitcoin.RPC;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Lykke.Service.Zcash.SignService.Services
{
    public class TransactionService : ITransactionService
    {
        private readonly ILog _log;
        private readonly RPCClient _rpcClient;

        public TransactionService(ILog log, RPCClient rpcClient)
        {
            _log = log;
            _rpcClient = rpcClient;
        }

        public async Task<string> SignAsync(string tx, Utxo[] outputs, string[] keys)
        {
            var result = await SendRpcAsync<SignResult>(RPCOperations.signrawtransaction, tx, JArray.FromObject(outputs), keys);

            if (result.Complete)
            {
                return result.Hex;
            }
            else
            {
                throw new InvalidOperationException("Sign is not complete");
            }
        }

        public async Task<bool> ValidateNotSignedTransactionAsync(string transaction)
        {
            var result = await SendRpcAsync<RawTransaction>(RPCOperations.decoderawtransaction, transaction)
                .ConfigureAwait(false);

            return result != null &&
                result.Vin.All(vin => vin.ScriptSig == null || (string.IsNullOrEmpty(vin.ScriptSig.Asm) && string.IsNullOrEmpty(vin.ScriptSig.Hex)));
        }

        public async Task<T> SendRpcAsync<T>(RPCOperations command, params object[] parameters)
        {
            var result = await _rpcClient.SendCommandAsync(command, parameters)
                .ConfigureAwait(false);

            result.ThrowIfError();

            // starting from Overwinter update NBitcoin can not deserialize Zcash transaparent transactions,
            // as well as it has never been able to work with shielded Zcash transactions,
            // that's why custom models are used widely instead of built-in NBitcoin commands;
            // additionaly in case of exception we save context to investigate later:

            try
            {
                return result.Result.ToObject<T>();
            }
            catch (JsonSerializationException jex)
            {
                await _log.WriteErrorAsync(nameof(SendRpcAsync), $"Command: {command}, Response: {result.ResultString}", jex)
                    .ConfigureAwait(false);

                throw;
            }
        }
    }
}
