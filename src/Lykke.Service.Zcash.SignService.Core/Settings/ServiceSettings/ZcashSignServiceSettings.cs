namespace Lykke.Service.Zcash.SignService.Core.Settings.ServiceSettings
{
    public class ZcashSignServiceSettings
    {
        public DbSettings Db { get; set; }
        public string NetworkType { get; set; }
        public string RpcAuthenticationString { get; set; }
        public string RpcUrl { get; set; }
    }
}
