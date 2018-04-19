using Lykke.SettingsReader.Attributes;

namespace Lykke.Service.Zcash.SignService.Core.Settings.ServiceSettings
{
    public class DbSettings
    {
        [AzureTableCheck]
        public string LogsConnString { get; set; }
    }
}
