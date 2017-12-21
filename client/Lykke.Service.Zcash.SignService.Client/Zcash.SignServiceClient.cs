using System;
using Common.Log;

namespace Lykke.Service.Zcash.SignService.Client
{
    public class Zcash.SignServiceClient : IZcash.SignServiceClient, IDisposable
    {
        private readonly ILog _log;

        public Zcash.SignServiceClient(string serviceUrl, ILog log)
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
