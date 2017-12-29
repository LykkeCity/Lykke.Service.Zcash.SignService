using System;
using System.Collections.Generic;
using System.Text;

namespace Lykke.Service.Zcash.SignService.Models.Sign
{
    public class SignTransactionResponse
    {
        public SignTransactionResponse(string hex) => SignedTransaction = hex;

        public string SignedTransaction { get; }
    }
}
