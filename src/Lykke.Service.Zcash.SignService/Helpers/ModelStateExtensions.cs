using Common;
using Lykke.Service.Zcash.SignService.Core.Domain.Transactions;
using Lykke.Service.Zcash.SignService.Core.Services;
using Lykke.Service.Zcash.SignService.Models.Sign;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Newtonsoft.Json;

namespace Lykke.Service.Zcash.SignService.Helpers
{
    public static class ModelStateExtensions
    {
        public static bool IsValidRequest(this ModelStateDictionary self,
            SignTransactionRequest request,
            ITransactionService transactionService,
            out string tx,
            out Utxo[] spentOutputs,
            out uint? branchId)
        {
            tx = null;
            spentOutputs = null;
            branchId = null;

            if (!self.IsValid)
            {
                return false;
            }

            try
            {
                (tx, spentOutputs, branchId) = JsonConvert.DeserializeObject<(string, Utxo[], uint?)>(request.TransactionContext.Base64ToString());
            }
            catch
            {
                self.AddModelError(
                    nameof(SignTransactionRequest.TransactionContext),
                    "Invalid transaction context");
            }

            if (!string.IsNullOrEmpty(tx) && !transactionService.ValidateNotSignedTransactionAsync(tx).Result)
            {
                self.AddModelError(
                    nameof(SignTransactionRequest.TransactionContext),
                    "Invalid not signed transaction data");
            }

            return self.IsValid;
        }

    }
}
