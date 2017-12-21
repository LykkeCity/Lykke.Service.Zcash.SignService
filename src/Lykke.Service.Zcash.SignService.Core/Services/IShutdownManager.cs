using System.Threading.Tasks;

namespace Lykke.Service.Zcash.SignService.Core.Services
{
    public interface IShutdownManager
    {
        Task StopAsync();
    }
}