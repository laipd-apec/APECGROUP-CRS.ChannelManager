using Confluent.Kafka;
using CRS.ChannelManager.Domain.Dtos;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static CRS.ChannelManager.Domain.Dtos.ConfigSettingDto;

namespace CRS.ChannelManager.Infrastructure.Shares.Kafka
{
    public class KafKaProducerService : IProducerService
    {
        private readonly IConfiguration _configuration;
        private readonly IProducer<Null, string> _producer;
        private readonly ILogger<KafkaConsumerService> _logger;
        private readonly IOptions<ConfigSettingDto.kafkaProducerConfig> _kafkaConfig;

        // KafKa configuration
        private readonly string _bootstrapServers;
        private readonly List<string> _topics;
        private readonly string _groupId;
        private readonly string _clientId;

        public KafKaProducerService(IConfiguration configuration
            , ILogger<KafkaConsumerService> logger
            , IOptions<ConfigSettingDto.kafkaProducerConfig> kafkaConfig)
        {
            _configuration = configuration;
            _logger = logger;
            _kafkaConfig = kafkaConfig;
            _bootstrapServers = kafkaConfig.Value.BootstrapServers;
            _groupId = kafkaConfig.Value.GroupId;
            _clientId = kafkaConfig.Value.ClientId;

            var producerconfig = new ProducerConfig
            {
                BootstrapServers = _bootstrapServers,
                //ClientId = _clientId,
            };
            _producer = new ProducerBuilder<Null, string>(producerconfig).Build();
            _logger.LogInformation($"Connect Kafka: {_bootstrapServers}, GroupId {_groupId}, ClientId {_clientId}");
        }

        public async Task ProduceAsync(string topic, string message)
        {
            var kafkaMessage = new Message<Null, string> { Value = message };
            try
            {
                var result = await _producer.ProduceAsync(topic, kafkaMessage);
                _logger.LogInformation($"Send to topic {topic}, Send message {message}");
                _logger.LogInformation($"Message sent to {result.TopicPartitionOffset}");
            }
            catch (ProduceException<Null, string> e)
            {
                _logger.LogError($"Failed to deliver message: {e.Message} [{e.Error.Code}]");
                // Optionally, implement retry logic here
            }
        }

    }
}
