using System;
using System.Collections.Generic;
using System.Text;

namespace Lykke.Service.Zcash.SignService.Models.Sign
{
    public class SignTransactionOkResponse
    {
        public SignTransactionOkResponse(string hex) => SignedTransaction = hex;

        public string SignedTransaction { get; }
    }
}
