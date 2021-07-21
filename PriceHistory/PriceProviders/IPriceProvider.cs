using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PriceHistory.PriceProviders
{
    public interface IPriceProvider : IAsyncDisposable
    {
        /// <summary>
        /// 异步初始化
        /// </summary>
        /// <returns></returns>
        Task InitAsync();

        /// <summary>
        /// 查询指定url商品价格, 当前价格在史低价格*limit 以下则触发通知
        /// </summary>
        /// <param name="urls"></param>
        /// <param name="limit"></param>
        /// <returns></returns>
        Task GetPricesAsync(IEnumerable<string> urls);

        /// <summary>
        /// 查询指定商城的指定sku列表商品价格 当前价格在史低价格*limit 以下则触发通知
        /// </summary>
        /// <param name="mall"></param>
        /// <param name="skuIds"></param>
        /// <param name="limit"></param>
        /// <returns></returns>
        Task GetPricesAsync(Mall mall, IEnumerable<long> skuIds) => GetPricesAsync(PriceHistoryConsts.GetUrls(mall, skuIds));

        /// <summary>
        /// 查询历史价格并通知
        /// </summary>
        /// <returns></returns>
        Task GetPricesAndNotifyAsync();
    }
}
