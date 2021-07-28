using Microsoft.Extensions.DependencyInjection;
using System;

namespace PriceHistory
{
    public static class HttpClientExtension
    {
        public static IServiceCollection AddWxHttpClient(this IServiceCollection services)
        {
            services.AddHttpClient(NoticeType.QyBot.ToString(), client =>
            {
                client.BaseAddress = new Uri($"https://qyapi.weixin.qq.com");
            });
            return services;
        }

        public static IServiceCollection AddCqHttpClient(this IServiceCollection services, string url)
        {
            services.AddHttpClient(NoticeType.QQBot.ToString(), client =>
            {
                client.BaseAddress = new Uri(url);
            });
            return services;
        }
    }
}
