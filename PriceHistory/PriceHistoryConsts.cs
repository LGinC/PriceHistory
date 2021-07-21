using System;
using System.Collections.Generic;
using System.Linq;

namespace PriceHistory
{
    public enum Mall
    {
        /// <summary>
        /// 京东
        /// </summary>
        JD,
        /// <summary>
        /// 天猫
        /// </summary>
        Tmall,
        /// <summary>
        /// 淘宝
        /// </summary>
        Taobao,
        /// <summary>
        /// 苏宁
        /// </summary>
        Suning
    }

    public enum NoticeType
    {
        /// <summary>
        /// 企业微信群聊机器人
        /// </summary>
        QyBot,
        /// <summary>
        /// 钉钉群聊机器人
        /// </summary>
        DDBot,
        /// <summary>
        /// qq群聊机器人
        /// </summary>
        QQBot,
        /// <summary>
        /// 邮件
        /// </summary>
        Email,
        /// <summary>
        /// 短信
        /// </summary>
        Sms,
    }

    public class PriceHistoryConsts
    {
        /// <summary>
        /// 京东
        /// </summary>
        public const string JD_url = "https://item.jd.com/";
        /// <summary>
        /// 天猫
        /// </summary>
        public const string Tmall_url = "";
        /// <summary>
        /// 淘宝
        /// </summary>
        public const string Taobao_url = "";
        /// <summary>
        /// 苏宁
        /// </summary>
        public const string Suning_url = "";

        public static IEnumerable<string> GetUrls(Mall mall, IEnumerable<long> skuIds) =>
            mall switch
            {
                Mall.JD => skuIds.Select(s => $"{JD_url}{s}.html"),
                Mall.Tmall => skuIds.Select(s => $"{Tmall_url}{s}.html"),
                Mall.Taobao => skuIds.Select(s => $"{Taobao_url}{s}.html"),
                Mall.Suning => skuIds.Select(s => $"{Suning_url}{s}.html"),
                _ => throw new ArgumentException("unsupport shop", nameof(mall))
            };

        public static string GetUrl(Mall mall, long skuId) =>
            mall switch
            {
                Mall.JD => $"{JD_url}{skuId}.html",
                Mall.Tmall => $"{Tmall_url}{skuId}.html",
                Mall.Taobao => $"{Taobao_url}{skuId}.html",
                Mall.Suning => $"{Suning_url}{skuId}.html",
                _ => throw new ArgumentException("unsupport shop", nameof(mall))
            };
    }
}
