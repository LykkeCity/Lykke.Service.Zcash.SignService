using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using Common;
using Lykke.Service.Zcash.SignService.Core.Domain.Transactions;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Newtonsoft.Json;

namespace Lykke.Service.Zcash.SignService.Models.Sign
{
    [DataContract]
    public class SignTransactionRequest : IValidatableObject
    {
        [DataMember]
        [Required]
        [MinLength(1)]
        public string[] PrivateKeys { get; set; }

        [DataMember]
        [Required]
        public string TransactionContext { get; set; }

        [ValidateNever]
        public string Tx { get; private set; }

        [ValidateNever]
        public Output[] Outputs { get; private set; }
                                                        
        [OnDeserialized]
        public void Init(StreamingContext streamingContext = default)
        {
            // leave properties null if deserialization fails, 
            // see Validate for actual validation

            if (Tx == null && Outputs == null)
            {
                try
                {
                    (Tx, Outputs) = JsonConvert.DeserializeObject<(string, Output[])>(TransactionContext.Base64ToString());
                }
                catch
                {
                    (Tx, Outputs) = (null, null);
                }
            }
        }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            var result = new List<ValidationResult>();

            Init();

            if (Tx == null || Outputs == null || Outputs.Length == 0)
            {
                result.Add(new ValidationResult("Invalid transaction data", new[] { nameof(TransactionContext) }));
            }

            return result;
        }
    }
}
