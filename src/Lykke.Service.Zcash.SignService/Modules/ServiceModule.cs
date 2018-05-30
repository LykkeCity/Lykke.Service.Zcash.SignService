using Autofac;
using Autofac.Extensions.DependencyInjection;
using Common.Log;
using Lykke.Service.Zcash.SignService.Core.Services;
using Lykke.Service.Zcash.SignService.Core.Settings.ServiceSettings;
using Lykke.Service.Zcash.SignService.Services;
using Lykke.SettingsReader;
using Microsoft.Extensions.DependencyInjection;
using NBitcoin;
using NBitcoin.RPC;
using NBitcoin.Zcash;

namespace Lykke.Service.Zcash.SignService.Modules
{
    public class ServiceModule : Module
    {
        private readonly IReloadingManager<ZcashSignServiceSettings> _settings;
        private readonly ILog _log;
        // NOTE: you can remove it if you don't need to use IServiceCollection extensions to register service specific dependencies
        private readonly IServiceCollection _services;

        public ServiceModule(IReloadingManager<ZcashSignServiceSettings> settings, ILog log)
        {
            _settings = settings;
            _log = log;

            _services = new ServiceCollection();
        }

        protected override void Load(ContainerBuilder builder)
        {
            // TODO: Do not register entire settings in container, pass necessary settings to services which requires them
            // ex:
            //  builder.RegisterType<QuotesPublisher>()
            //      .As<IQuotesPublisher>()
            //      .WithParameter(TypedParameter.From(_settings.CurrentValue.QuotesPublication))

            builder.RegisterInstance(_log)
                .As<ILog>()
                .SingleInstance();

            builder.RegisterType<HealthService>()
                .As<IHealthService>()
                .SingleInstance();

            builder.RegisterType<StartupManager>()
                .As<IStartupManager>();

            builder.RegisterType<ShutdownManager>()
                .As<IShutdownManager>();

            ZcashNetworks.Instance.EnsureRegistered();

            builder.RegisterInstance(Network.GetNetwork(_settings.CurrentValue.NetworkType))
                .As<Network>();

            if (!string.IsNullOrEmpty(_settings.CurrentValue.RpcAuthenticationString) &&
                !string.IsNullOrEmpty(_settings.CurrentValue.RpcUrl))
            {
                builder.RegisterType<RPCClient>()
                    .AsSelf()
                    .WithParameter("authenticationString", _settings.CurrentValue.RpcAuthenticationString)
                    .WithParameter("hostOrUri", _settings.CurrentValue.RpcUrl);

                builder.RegisterType<BlockchainReader>()
                    .As<IBlockchainReader>();
            }

            builder.RegisterType<TransactionService>()
                .As<ITransactionService>();

            builder.RegisterType<WalletService>()
                .As<IWalletService>();

            builder.Populate(_services);
        }
    }
}
