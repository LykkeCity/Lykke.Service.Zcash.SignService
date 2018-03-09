using Lykke.Service.Zcash.SignService.Core.Services;
using NBitcoin;

namespace Lykke.Service.Zcash.SignService.Services
{
    public class WalletService : IWalletService
    {
        private Network _network;

        public WalletService(Network network)
        {
            _network = network;
        }

        public (string wif, string address) CreateTransparentWallet()
        {
            var key = new Key();
            var wif = key.GetWif(_network);
            var address = wif.GetAddress();

            return (wif.ToWif(), address.ToString());
        }
    }
}
