using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using PriceHistory.NoticeProviders;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace PriceHistory
{
    public interface INoticeProvider
    {
        INoticeSender GetSender();
    }

    public class NoticeProvider : INoticeProvider
    {
        private readonly IOptionsMonitor<NoticeOptions> _options;
        private readonly IServiceProvider _serviceProvider;

        public NoticeProvider(IOptionsMonitor<NoticeOptions> options, IServiceProvider serviceProvider)
        {
            _options = options;
            _serviceProvider = serviceProvider;
        }

        public INoticeSender GetSender()
        {
            NoticeType type = NoticeType.QyBot;
            Enum.TryParse(_options.CurrentValue.Type, out type);
            return _serviceProvider.GetServices<INoticeSender>().FirstOrDefault(s => s.Type == type);
        }
    }
}
