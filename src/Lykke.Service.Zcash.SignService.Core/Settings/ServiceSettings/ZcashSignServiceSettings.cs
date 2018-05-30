using Lykke.SettingsReader.Attributes;

namespace Lykke.Service.Zcash.SignService.Core.Settings.ServiceSettings
{
    public class ZcashSignServiceSettings
    {
        public DbSettings Db { get; set; }
        public string NetworkType { get; set; }

        [Optional]
        public string RpcAuthenticationString { get; set; }

        [Optional]
        public string RpcUrl { get; set; }
    }
}
