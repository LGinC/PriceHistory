using Microsoft.Extensions.Hosting;
using PriceHistory.PriceProviders;
using System.Threading;
using System.Threading.Tasks;

namespace PriceHistory
{
    public class Worker : IHostedService
    {
        private readonly IPriceProvider provider;
        private readonly IHost host;
        public Worker(IPriceProvider provider, IHost host)
        {
            this.provider = provider;
            this.host = host;
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            await provider.InitAsync();
            await provider.GetPricesAndNotifyAsync();

            //运行完毕退出
            await host.StopAsync(cancellationToken);
        }

        public async Task StopAsync(CancellationToken cancellationToken)
        {
            await provider.DisposeAsync();
        }
    }
}