using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace Lykke.Service.Zcash.SignService.Models.Wallet
{
    public class CreateOkResponse
    {
        public CreateOkResponse((string wif, string address) data) => (PrivateKey, PublicAddress) = data;

        public string PrivateKey    { get; }
        public string PublicAddress { get; }
    }
}
