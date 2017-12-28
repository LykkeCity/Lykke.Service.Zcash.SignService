using System;
using Common.Log;

namespace Lykke.Service.Zcash.SignService.Client
{
    public class ZcashSignServiceClient : IZcashSignServiceClient, IDisposable
    {
        private readonly ILog _log;

        public ZcashSignServiceClient(string serviceUrl, ILog log)
        {
            _log = log;
        }

        public void Dispose()
        {
            //if (_service == null)
            //    return;
            //_service.Dispose();
            //_service = null;
        }
    }
}
