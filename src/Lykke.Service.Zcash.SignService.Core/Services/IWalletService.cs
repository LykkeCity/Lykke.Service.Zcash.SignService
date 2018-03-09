using System;
using System.Collections.Generic;
using System.Text;

namespace Lykke.Service.Zcash.SignService.Core.Services
{
    public interface IWalletService
    {
        (string wif, string address) CreateTransparentWallet();
    }
}
