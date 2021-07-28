using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace PriceHistory.NoticeProviders
{
    /// <summary>
    /// 企业微信机器人通知
    /// </summary>
    public class QyWxBotNoticeSender : INoticeSender
    {
        public NoticeType Type => NoticeType.QyBot;
        private readonly HttpClient client;
        private readonly IOptionsMonitor<NoticeOptions> _options;

        public QyWxBotNoticeSender(IHttpClientFactory factory, IOptionsMonitor<NoticeOptions> options)
        {
            client = factory.CreateClient(Type.ToString());
            _options = options;
        }

        public async Task NotifyAsync(string name, decimal currentPrice, decimal lowestPrice, string date, string url)
        {
            var r  = await client.PostAsync($"/cgi-bin/webhook/send?key={_options.CurrentValue.Key}", new StringContent(JsonSerializer.Serialize(new
            {
                msgtype = "markdown",
                markdown = new { content= $"# <font color=\"warning\">{(currentPrice<=lowestPrice ? string.Empty : "接近")}史低了</font>\n ### {name}\n史低价格: {lowestPrice}\n当前价格：{currentPrice} \n[{url}]({url})" },
            }))); ;
            var s = await r.Content.ReadAsStringAsync();
            Console.WriteLine(s);
        }
    }

}
