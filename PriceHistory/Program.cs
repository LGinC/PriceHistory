using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using PriceHistory.NoticeProviders;
using PriceHistory.PriceProviders;
using System.Threading.Tasks;
namespace PriceHistory
{
    class Program
    {
        public const string ConfigFile = "appsettings.json";

        public static IHostBuilder CreateHostBuilder(string[] args) =>
        Host.CreateDefaultBuilder(args)
            .ConfigureServices((hostContext, services) =>
            {
                services.Configure<PriceOptions>(hostContext.Configuration.GetSection("App"));
                services.Configure<NoticeOptions>(hostContext.Configuration.GetSection("Notice"));
                services.AddTransient<IPriceProvider, ManmanbuyPriceProvider>();
                services.AddTransient<INoticeSender, QyWxBotNoticeSender>();
                services.AddTransient<INoticeProvider, NoticeProvider>();
                services.AddWxHttpClient();

                services.AddHostedService<Worker>();
            });

        static async Task Main(string[] args)
        {
            await CreateHostBuilder(args).Build().RunAsync();
        }
    }
}
