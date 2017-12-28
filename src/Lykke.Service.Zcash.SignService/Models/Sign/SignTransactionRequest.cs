using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.Serialization;
using NBitcoin;
using NBitcoin.JsonConverters;
using Newtonsoft.Json;

namespace Lykke.Service.Zcash.SignService.Models.Sign
{
    [DataContract]
    public class SignTransactionRequest : IValidatableObject
    {
        [Required, DataMember] public string[]    PrivateKeys    { get; set; }
        [Required, DataMember] public string      TransactionHex { get; set; }
                               public Transaction Tx             { get; private set; }
                               public ICoin[]     Coins          { get; private set; }
                               public Key[]       Keys           { get; private set; }
                                                        

        [OnDeserialized]
        public void Init(StreamingContext streamingContext = default)
        {
            // leave properties null if deserialization fails, 
            // see Validate for actual validation

            if (Tx == null && Coins == null)
            {
                try
                {
                    (Tx, Coins) = Serializer.ToObject<(Transaction tx, ICoin[] coins)>(TransactionHex);
                }
                catch
                {
                    (Tx, Coins) = (null, null);
                }
            }

            if (Keys == null)
            {
                try
                {
                    Keys = PrivateKeys.Select(k => Key.Parse(k)).ToArray();
                }
                catch
                {
                    Keys = null;
                }
            }
        }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            var result = new List<ValidationResult>();

            Init();

            if (Tx == null || Coins == null || Coins.Length == 0)
            {
                result.Add(new ValidationResult("Invalud transaction data", new[] { nameof(TransactionHex) }));
            }

            if (Keys == null || Keys.Length == 0)
            {
                result.Add(new ValidationResult("Invalud key(s)", new[] { nameof(PrivateKeys) }));
            }

            return result;
        }
    }
}
