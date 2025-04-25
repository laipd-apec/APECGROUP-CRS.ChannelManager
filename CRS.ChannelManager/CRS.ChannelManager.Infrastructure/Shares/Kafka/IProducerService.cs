using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRS.ChannelManager.Infrastructure.Shares.Kafka
{
    public interface IProducerService
    {
        Task ProduceAsync(string topic, string message);
    }
}
