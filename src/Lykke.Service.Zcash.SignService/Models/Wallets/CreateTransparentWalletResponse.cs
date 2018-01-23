using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;
using Newtonsoft.Json;

namespace Lykke.Service.Zcash.SignService.Models.Wallets
{
    public class CreateTransparentWalletResponse
    {
        public CreateTransparentWalletResponse((string wif, string address) data) => (PrivateKey, PublicAddress) = data;

        public string PrivateKey { get; }

        public string PublicAddress { get; }
    }
}
