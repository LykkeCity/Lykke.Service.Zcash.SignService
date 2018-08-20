using Lykke.Service.Zcash.SignService.Core.Services;
using Lykke.Service.Zcash.SignService.Models.Wallets;
using Microsoft.AspNetCore.Mvc;

namespace Lykke.Service.Zcash.SignService.Controllers
{
    [Route("/api/wallets")]
    public class WalletsController : Controller
    {
        private readonly IWalletService _walletService;

        public WalletsController(IWalletService walletService)
        {
            _walletService = walletService;
        }

        /// <summary>
        /// Creates t-address for Zcash
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public CreateTransparentWalletResponse CreateTransaprentWallet()
        {
            return new CreateTransparentWalletResponse(_walletService.CreateTransparentWallet());
        }
    }
}
