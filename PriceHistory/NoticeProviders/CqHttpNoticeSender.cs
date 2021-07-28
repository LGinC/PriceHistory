using Microsoft.Extensions.Options;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace PriceHistory.NoticeProviders
{
    public class CqHttpNoticeSender : INoticeSender
    {
        public NoticeType Type => NoticeType.QQBot;
        private readonly HttpClient client;
        private readonly IOptionsMonitor<NoticeOptions> _options;

        public CqHttpNoticeSender(IHttpClientFactory factory, IOptionsMonitor<NoticeOptions> options)
        {
            _options = options;
            client = factory.CreateClient(Type.ToString());
        }

        public async Task NotifyAsync(string name, decimal currentPrice, decimal lowestPrice, string date, string url)
        {
            var s = new StringContent(JsonSerializer.Serialize(new
            {
                user_id = _options.CurrentValue.Target,
                message = $"{(currentPrice <= lowestPrice ? string.Empty : "接近")}史低了\n{name}\n史低价格: {lowestPrice}\n当前价格：{currentPrice} \n{url}"
            }), Encoding.UTF8, "application/json");
            var t = await client.PostAsync($"/send_private_msg?access_token={_options.CurrentValue.Key}", s);
            System.Console.WriteLine(await t.Content.ReadAsStringAsync());
        }
    }
}
