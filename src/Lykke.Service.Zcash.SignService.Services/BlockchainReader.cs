using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Common.Log;
using Lykke.Service.Zcash.SignService.Core.Domain.Transactions;
using NBitcoin.RPC;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Lykke.Service.Zcash.SignService.Services
{
    public class BlockchainReader : IBlockchainReader
    {
        private readonly RPCClient _rpcClient;

        public BlockchainReader(RPCClient rpcClient)
        {
            _rpcClient = rpcClient;
        }

        public async Task<RawTransaction> DecodeRawTransaction(string tx)
        {
            return await SendRpcAsync<RawTransaction>(RPCOperations.decoderawtransaction, tx); 
        }

        public async Task<SignResult> SignRawTransaction(string tx, Utxo[] outputs, string[] keys)
        {
            return await SendRpcAsync<SignResult>(RPCOperations.signrawtransaction, tx, JArray.FromObject(outputs), keys);
        }

        public async Task<T> SendRpcAsync<T>(RPCOperations command, params object[] parameters)
        {
            var result = await _rpcClient.SendCommandAsync(command, parameters)
                .ConfigureAwait(false);

            // starting from Overwinter update NBitcoin can not deserialize Zcash transaparent transactions,
            // as well as it has never been able to work with shielded Zcash transactions,
            // that's why custom models are used widely instead of built-in NBitcoin commands;
            // additionaly in case of exception we save context to investigate later:

            return result.Result.ToObject<T>();
        }
    }
}
