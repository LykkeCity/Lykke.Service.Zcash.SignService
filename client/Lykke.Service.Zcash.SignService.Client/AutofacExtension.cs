using System;
using Autofac;
using Common.Log;

namespace Lykke.Service.Zcash.SignService.Client
{
    public static class AutofacExtension
    {
        public static void RegisterZcashSignServiceClient(this ContainerBuilder builder, string serviceUrl, ILog log)
        {
            if (builder == null) throw new ArgumentNullException(nameof(builder));
            if (serviceUrl == null) throw new ArgumentNullException(nameof(serviceUrl));
            if (log == null) throw new ArgumentNullException(nameof(log));
            if (string.IsNullOrWhiteSpace(serviceUrl))
                throw new ArgumentException("Value cannot be null or whitespace.", nameof(serviceUrl));

            builder.RegisterType<ZcashSignServiceClient>()
                .WithParameter("serviceUrl", serviceUrl)
                .As<IZcashSignServiceClient>()
                .SingleInstance();
        }

        public static void RegisterZcashSignServiceClient(this ContainerBuilder builder, ZcashSignServiceClientSettings settings, ILog log)
        {
            builder.RegisterZcashSignServiceClient(settings?.ServiceUrl, log);
        }
    }
}
