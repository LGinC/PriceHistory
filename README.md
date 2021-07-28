# PriceHistory
历史价格查询对比和通知

## 简介
支持的商城查询
- [x] 京东 JD
- [ ] 天猫 Tmall
- [ ] 淘宝 Taobao
- [ ] 苏宁 Suning

---
支持的通知
- [x] 企业微信群聊机器人 QyBot
- [x] QQ群聊机器人 QQBot
- [ ] 钉钉群聊机器人 DDBot
- [ ] 邮件 Email
- [ ] 短信 SMS

### 配置
appsettings.json内容如下
- Mall: 可选类型有 JD Taobao Tmall Suning, 传入GoodsUrls时可忽略
- GoodsIds: 和GoodsUrls二选一传入 商品sku id, 多个用`,`分割, 如京东的url是https://item.jd.com/2131412321.html  其中的2131412321就是其sku id
- GoodsUrls: 商品url, 多个用`,`分割, 传入此参数时忽略以上两个
- NotifyLimitFromLowest: 通知阈值, 史低价格的倍数 如设置为"1.1" 则当前价格小于等于史低*1.1 时触发提醒

- Notice Type: 通知类型
- Notice Key: 机器人的accessKey
```
{
  "App": {
    "Mall": "",
    "GoodsIds": "",
    "GoodsUrls": "",
    "NotifyLimitFromLowest": ""
  },
  "Notice": {
    "Type": "QyBot",
    "Key": ""
  }
}
```

## 运行方式
### dotnet PriceHistory.dll
修改appsetings.json里的值,然后使用dotnet 命令来运行 
注意 不支持dotnet publish打包后运行,只支持dotnet build后运行

### docker
拉取镜像 `docker pull lginc/price-history-notify`
docker 可以通过设置环境变量的方式来覆盖appsettings.json里的值, 规则是上下级之间通过__来连接,如App.Mall 就是App__Mall
`docker run --rm -e App__Mall='JD' -e App__GoodsIds='1930323' -e Notice__Type='QyBot' -e Notice__Key='xxxxxxx'  lginc/price-history-notify`

docker-compose
```
version: '3'
service:
  price-notice:
    image: lginc/price-history-notify
    environments:
      - App__Mall: JD
      - App__GoodsIds: 13213,412312
      - Notice__Type: QyBot
      - Notice__Key: xxxxxxx
```