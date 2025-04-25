using Microsoft.Extensions.Hosting;
using System.Threading;

namespace CRS.ChannelManager.Infrastructure.Shares.Kafka
{
    public interface IConsumerService : IHostedService
    {
        Task ProcessKafkaMessage(CancellationToken stoppingToken);
    }
}