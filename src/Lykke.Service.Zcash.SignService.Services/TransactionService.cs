using System.Threading.Tasks;
using Common.Log;
using Lykke.Service.Zcash.SignService.Core.Domain.Transactions;
using Lykke.Service.Zcash.SignService.Core.Services;
using NBitcoin.RPC;
using Newtonsoft.Json;

namespace Lykke.Service.Zcash.SignService.Services
{
    public class TransactionService : ITransactionService
    {
        private readonly ILog _log;
        private readonly RPCClient _rpcClient;

        public TransactionService(RPCClient rpcClient)
        {
            _rpcClient = rpcClient;
        }

        public async Task<string> Sign(string tx, Utxo[] outputs, string[] keys)
        {
            var result = await _rpcClient.SendCommandAsync(RPCOperations.signrawtransaction, tx, outputs, keys);

            result.ThrowIfError();

            try
            {
                return result.Result.ToObject<SignResult>().hex;
            }
            catch (JsonSerializationException ex)
            {
                await _log.WriteErrorAsync(nameof(Sign), $"Response: {result.ResultString}", ex);
                throw;
            }
        }
    }
}
