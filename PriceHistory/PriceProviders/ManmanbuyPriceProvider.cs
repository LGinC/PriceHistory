using Microsoft.Extensions.Options;
using Microsoft.Playwright;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace PriceHistory.PriceProviders
{
    public class ManmanbuyPriceProvider : IPriceProvider
    {
        IPlaywright playwright;
        IBrowser browser;
        private readonly INoticeProvider _noticeProvider;
        private readonly IOptionsMonitor<PriceOptions> _options;
        const string LowestPriceUrl = "https://tool.manmanbuy.com/HistoryLowest.aspx?url=";

        public ManmanbuyPriceProvider(INoticeProvider noticeProvider, IOptionsMonitor<PriceOptions> options)
        {
            _noticeProvider = noticeProvider;
            _options = options;
        }

        public async ValueTask DisposeAsync()
        {
            Console.WriteLine("over");
            await browser.DisposeAsync();
            playwright.Dispose();
        }

        public async Task InitAsync()
        {
            playwright = await Playwright.CreateAsync();
            browser = await playwright.Chromium.LaunchAsync(new BrowserTypeLaunchOptions
            {
                Headless = true,//无头模式, 不显示浏览器界面
                SlowMo = 100,
            });
        }

        public Task GetPricesAsync(IEnumerable<string> urls)
        {
            List<Task> tasks = new();
            if (!decimal.TryParse(_options.CurrentValue.NotifyLimitFromLowest, out decimal limit))
            {
                limit = 1.1m;
            }
            foreach (var url in urls)
            {
                Console.WriteLine("开始抓取：" + url);
                tasks.Add(Task.Run(async () =>
                {
                    var page = await browser.NewPageAsync();
                    Console.WriteLine($"{LowestPriceUrl}{HttpUtility.UrlEncode(url)}");
                    await page.GotoAsync($"{LowestPriceUrl}{HttpUtility.UrlEncode(url)}");
                    //设置window.navigator.webdriver为undefine  表示浏览器不是被自动控制
                    await page.EvaluateAsync("Object.defineProperties(navigator, {webdriver:{get:()=>undefined}});");
                    //Console.WriteLine("window.navigator.webdriver: " + await page.EvaluateAsync<bool>("window.navigator.webdriver"));
                    await page.FocusAsync("#sc");
                    await page.ClickAsync("#rectBottom");
                    await page.WaitForSelectorAsync("span.currentprice");
                    decimal.TryParse(await page.TextContentAsync("span.currentprice"), out var currentPrice);
                    decimal.TryParse(await page.TextContentAsync("span.bigwordprice"), out var lowestPrice);
                    var date = await page.TextContentAsync("//*[@id=\"maindiv\"]/div[2]/div[1]/div[2]/div[1]/div/span[2]");
                    var name = await page.TextContentAsync("//*[@id=\"maindiv\"]/div[2]/div[1]/div[2]/div[1]/h1");
                    Console.WriteLine($"{name} \n  {currentPrice}/{lowestPrice}");
                    if (currentPrice <= lowestPrice * limit)//小于史低价格指定倍数  即可入手
                    {
                        await _noticeProvider.GetSender().NotifyAsync(name, currentPrice, lowestPrice, date, url);
                    }
                }));
            }
            return Task.WhenAll(tasks);
        }

        Task GetPricesAsync(Mall mall, IEnumerable<long> skuIds) => GetPricesAsync(PriceHistoryConsts.GetUrls(mall, skuIds));

        public Task GetPricesAndNotifyAsync()
        {
            var options = _options.CurrentValue;
            if(!string.IsNullOrWhiteSpace(options.GoodsUrls))
            {
                return GetPricesAsync(options.GoodsUrls.Split(','));
            }

            if(string.IsNullOrWhiteSpace(options.GoodsIds))
            {
                throw new ArgumentException("GoodsIds cannot be null", nameof(options.GoodsIds));
            }
            //未设置商城类型默认京东
            Mall mall = Mall.JD;
            if(!Enum.TryParse(options.Mall, out mall))
            {
                Console.WriteLine("未设置商城类型 Mall, 默认京东");
            }
            return GetPricesAsync(mall, options.GoodsIds.Split(',').Select(i => long.Parse(i)));
        }
    }
}
