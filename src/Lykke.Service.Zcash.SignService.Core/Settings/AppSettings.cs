using Lykke.Service.Zcash.SignService.Core.Settings.ServiceSettings;
using Lykke.Service.Zcash.SignService.Core.Settings.SlackNotifications;

namespace Lykke.Service.Zcash.SignService.Core.Settings
{
    public class AppSettings
    {
        public ZcashSignServiceSettings ZcashSignService { get; set; }
        public SlackNotificationsSettings SlackNotifications { get; set; }
    }
}
