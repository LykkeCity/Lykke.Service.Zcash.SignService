using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using Common;
using Lykke.Service.Zcash.SignService.Core.Domain.Transactions;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Newtonsoft.Json;

namespace Lykke.Service.Zcash.SignService.Models.Sign
{
    public class SignTransactionRequest
    {
        [Required]
        [MinLength(1)]
        public string[] PrivateKeys { get; set; }

        [Required]
        public string TransactionContext { get; set; }
    }
}
