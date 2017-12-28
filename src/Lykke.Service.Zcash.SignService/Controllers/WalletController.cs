using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using Lykke.Service.Zcash.SignService.Core.Services;
using Lykke.Service.Zcash.SignService.Models.Wallet;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Lykke.Service.Zcash.SignService.Controllers
{
    [Route("api/wallets")]
    public class WalletController : Controller
    {
        private readonly IWalletService _walletService;

        public WalletController(IWalletService walletService)
        {
            _walletService = walletService;
        }

        /// <summary>
        /// Creates t-address for Zcash
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public CreateOkResponse Create()
        {
            return new CreateOkResponse(_walletService.CreateTransparentWallet());
        }
    }
}
