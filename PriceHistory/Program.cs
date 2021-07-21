using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PriceHistory.NoticeProviders;
using PriceHistory.PriceProviders;
using System.Threading.Tasks;
namespace PriceHistory
{
    class Program
    {
        public const string ConfigFile = "appsettings.json";

        static async Task Main(string[] args)
        {
            var services = new ServiceCollection();
            var env = System.Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
            var builder = new ConfigurationBuilder()
                .AddJsonFile(ConfigFile, false, true)
                .AddEnvironmentVariables();
            if (!string.IsNullOrEmpty(env))
            {
                builder.AddJsonFile($"appsettings.{env}.json");
            }
            var config = builder.Build();
            services.AddSingleton(config);
            services.Configure<PriceOptions>(config.GetSection("App"));
            services.Configure<NoticeOptions>(config.GetSection("Notice"));
            services.AddTransient<IPriceProvider, ManmanbuyPriceProvider>();
            services.AddTransient<INoticeSender, QyWxBotNoticeSender>();
            services.AddScoped<INoticeProvider, NoticeProvider>();
            services.AddWxHttpClient();
            var provider = services.BuildServiceProvider();

            await using var priceProvider = provider.GetService<IPriceProvider>();
            await priceProvider.InitAsync();
            await priceProvider.GetPricesAndNotifyAsync();
        }
    }
}
