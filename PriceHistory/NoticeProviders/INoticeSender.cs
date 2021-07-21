using System.Threading.Tasks;

namespace PriceHistory.NoticeProviders
{
    public interface INoticeSender
    {
        NoticeType Type { get; }

        /// <summary>
        /// 商品价格变动通知
        /// </summary>
        /// <param name="name">商品名</param>
        /// <param name="currentPrice">当前价格</param>
        /// <param name="lowestPrice">史低价格</param>
        /// <param name="date">史低日期</param>
        /// <param name="url">商品链接</param>
        /// <returns></returns>
        Task NotifyAsync(string name, decimal currentPrice, decimal lowestPrice, string date, string url);
    }
}
