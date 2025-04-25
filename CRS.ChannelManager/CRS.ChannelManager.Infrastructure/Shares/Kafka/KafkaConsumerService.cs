using Confluent.Kafka;
using CRS.ChannelManager.Domain.Dtos;
using CRS.ChannelManager.Domain.Enums;
using CRS.ChannelManager.Library.Base;
using CRS.ChannelManager.Library.BaseEnum;
using CRS.ChannelManager.Library.BaseInterface;
using CRS.ChannelManager.Library.Constants;
using CRS.ChannelManager.Library.Mapping;
using CRS.ChannelManager.Library.Utils;
using HandlebarsDotNet.ValueProviders;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Reflection;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using static Confluent.Kafka.ConfigPropertyNames;
using static CRS.ChannelManager.Domain.Dtos.KafkaDto;
using static CRS.ChannelManager.Library.BaseDto.UtilsDto;

namespace CRS.ChannelManager.Infrastructure.Shares.Kafka
{
    public class KafkaConsumerService : BackgroundService, IConsumerService
    {
        private readonly IConsumer<Ignore, string> _consumer;
        private readonly ILogger<KafkaConsumerService> _logger;
        private readonly IConfiguration _configuration;
        private readonly IOptions<ConfigSettingDto.kafkaConsumerConfig> _kafkaConfig;
        private readonly IServiceScopeFactory _serviceScopeFactory;

        // KafKa configuration
        private readonly string _bootstrapServers;
        private readonly List<string> _topics;
        private readonly string _groupId;
        private readonly string _clientId;
        private readonly string env = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "DEV";
        public KafkaConsumerService(
            IConfiguration configuration
            , ILogger<KafkaConsumerService> logger
            , IOptions<ConfigSettingDto.kafkaConsumerConfig> kafkaConfig
            , IServiceScopeFactory serviceScopeFactory
            )
        {

            _logger = logger;
            _configuration = configuration;
            _kafkaConfig = kafkaConfig;
            _bootstrapServers = _kafkaConfig.Value.BootstrapServers;
            _groupId = $"{env}-{_kafkaConfig.Value.GroupId}";
            _clientId = $"{env}-{_kafkaConfig.Value.ClientId}";
            _topics = _kafkaConfig.Value.Topics;
            _serviceScopeFactory = serviceScopeFactory;

            var consumerConfig = new ConsumerConfig
            {
                BootstrapServers = _bootstrapServers,
                GroupId = _groupId,
                ClientId = _clientId,
                SessionTimeoutMs = 60000, // Tăng thời gian chờ phiên
                AutoOffsetReset = AutoOffsetReset.Earliest, // lắng nghe lại từ đầu
                EnableAutoCommit = false,
                ApiVersionRequest = true,
                SecurityProtocol = SecurityProtocol.Plaintext,
            };
            _consumer = new ConsumerBuilder<Ignore, string>(consumerConfig).Build();
            _logger.LogInformation($"Connect Kafka: {_bootstrapServers}, GroupId {_groupId}, ClientId {_clientId}");
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            await Task.Run(async () =>
            {
                try
                {
                    int timeProcessKafka = 1000;
                    _consumer.Subscribe(_topics);
                    while (!stoppingToken.IsCancellationRequested)
                    {
                        await ProcessKafkaMessage(stoppingToken);
                        await Task.Delay(timeProcessKafka, stoppingToken);
                    }
                }
                catch (ConsumeException ex)
                {
                    _logger.LogError($"Kafka consume error: {ex.Error.Reason}");
                }
                catch (Exception ex)
                {
                    _logger.LogError($"Execute Received message: {ex.Message}");
                }
                finally
                {
                    _consumer.Close();
                }
            }, stoppingToken);
        }

        public Task ProcessKafkaMessage(CancellationToken stoppingToken)
        {
            string messageJson = string.Empty;
            if (_consumer != null)
            {
                var consumeResult = _consumer.Consume(stoppingToken);
                try
                {
                    var topic = consumeResult.Topic ?? "None";
                    messageJson = consumeResult.Message.Value;
                    if (!string.IsNullOrEmpty(messageJson))
                    {
                        switch (topic)
                        {
                            case EKafkaTopicType.SyncMasterData:
                                SyncMasterData(messageJson);
                                break;
                            //case EKafkaTopicType.BookingResult:
                            //    BookingResult(messageJson);
                            //    break;
                            default:
                                break;
                        }
                    }
                    _logger.LogInformation($"Topic {topic} Received message: {messageJson}");
                    _consumer.Commit(consumeResult);
                }
                catch (ConsumeException e)
                {
                    _logger.LogInformation($"Received message: {messageJson}");
                    _logger.LogError($"Consume error: {e.Error.Reason}");
                }
                catch (Exception ex)
                {
                    _logger.LogInformation($"Received message: {messageJson}");
                    _logger.LogError($"Error processing Kafka message: {ex.Message}");
                }
                finally
                {
                    //_consumer.Commit(consumeResult);
                    //_consumer.Commit();
                }
            }
            return Task.CompletedTask;
        }

        private async void SyncMasterData(string message)
        {
            try
            {
                //message = "{\"action\":\"hotel.items.update\",\"data\":{\"id\":4,\"status\":\"draft\",\"hotel_id\":15,\"sort\":null,\"created_by\":\"1cde4339-5b22-42d2-a8a4-f9fd8425ac3c\",\"created_date\":\"2024-10-07T03:24:43.817Z\",\"modified_by\":\"1cde4339-5b22-42d2-a8a4-f9fd8425ac3c\",\"modified_date\":\"2024-10-07T04:20:46.022Z\",\"full_name\":\"test 678\",\"short_name\":\"test 678 \",\"description\":null,\"contact_phone\":null,\"address\":null,\"website\":null,\"location\":null,\"star_rating\":null,\"review_rate\":null,\"hotel_type\":null,\"checkin_time\":null,\"checkout_time\":null,\"images\":null,\"code\":\"Test\",\"province\":null,\"district\":null,\"amenities\":[],\"places\":[]}}";
                //message = "{\"action\":\"hotel.items.delete\",\"data\":[4]}";
                var kafkaRes = JsonConvert.DeserializeObject<KafkaDto.KafkaResponseDto>(message);
                if (kafkaRes != null && !string.IsNullOrEmpty(kafkaRes.NameObject))
                {
                    var dateTimeNow = TimeZoneInfo.ConvertTime(DateTime.Now, TimeZoneInfo.FindSystemTimeZoneById(CommonConstants.TimeZoneValue.VnPlut7));
                    string actionType = kafkaRes.ActionType;
                    using (var scope = _serviceScopeFactory.CreateScope())
                    {
                        var dbContext = scope.ServiceProvider.GetRequiredService<IDbContext>();

                        string nameEntity = $"{kafkaRes.NameObject}{EKafkaObjectType.KeyEndObject}";
                        Type? targetType = TypeFinder.FindType(nameEntity);
                        if (targetType != null)
                        {
                            if (actionType == EKafkaActionType.Delete || actionType == EKafkaActionType.Remove)
                            {
                                targetType = typeof(List<long>);
                            }
                            Type genericType = typeof(KafkaResponseDto<>).MakeGenericType(targetType);
                            string nameRepository = $"{kafkaRes.NameObject}{EKafkaObjectType.KeyEndRepository}";
                            var findTypeRepository = TypeFinder.FindType(nameRepository, EKafkaObjectType.FolderRepository);
                            var convertMessage = JsonConvert.DeserializeObject(message, genericType);
                            object? repository = null;
                            if (findTypeRepository != null && genericType != null)
                            {
                                repository = ActivatorUtilities.CreateInstance(scope.ServiceProvider, findTypeRepository);
                                var methodFind = findTypeRepository.GetMethods().FirstOrDefault(x => x.Name == EKafkaMethodType.FindByPropertySyncKey);
                                PropertyInfo? dataProperty = genericType.GetProperty(EKafkaObjectType.NameGetObject);
                                MethodInfo? methodInfo = null;
                                //var instance = Activator.CreateInstance(genericType);
                                //var dataInstance = Activator.CreateInstance(targetType);
                                //dataProperty?.SetValue(instance, dataInstance);

                                //// Lấy giá trị của thuộc tính Data
                                //var value = dataProperty?.GetValue(instance);
                                var value = dataProperty?.GetValue(convertMessage);
                                string deleteStatus = YesNoEnum.No.ToEnumMemberString();
                                MethodInfo? methodGetValueFromJson = typeof(JsonToEntityMapper).GetMethod(EKafkaObjectType.NameGetValueFromJson);
                                if (methodGetValueFromJson != null)
                                {
                                    MethodInfo? genericMethodGetValueFromJson = methodGetValueFromJson.MakeGenericMethod(targetType);
                                    if (genericMethodGetValueFromJson != null)
                                    {
                                        object? valueStatus = genericMethodGetValueFromJson.Invoke(null, new object[] { JsonConvert.SerializeObject(kafkaRes.Data), "status" });
                                        if (valueStatus != null)
                                        {
                                            if (valueStatus.ToString() != EMasterDataStatus.Published.ToEnumMemberString())
                                            {
                                                deleteStatus = YesNoEnum.Yes.ToEnumMemberString();
                                            }
                                        }
                                    }
                                }
                                // Map JSON to entity using the generic method
                                if (actionType == EKafkaActionType.Create || actionType == EKafkaActionType.Update)
                                {
                                    MethodInfo? methodMapJson = typeof(JsonToEntityMapper).GetMethod(EKafkaObjectType.NameMapJson);
                                    if (methodMapJson != null)
                                    {
                                        MethodInfo? genericMethodMapJson = methodMapJson.MakeGenericMethod(targetType);
                                        if (genericMethodMapJson != null)
                                        {
                                            object? entity = genericMethodMapJson.Invoke(null, new object[] { JsonConvert.SerializeObject(kafkaRes.Data) });
                                            value = entity;
                                        }
                                    }
                                    if (methodFind != null && value != null)
                                    {
                                        var idProperty = value.GetType().GetProperty(EKafkaObjectType.NameGetId);
                                        var deletedProperty = value.GetType().GetProperty(EKafkaObjectType.NameDeleted);
                                        var deletedDateProperty = value.GetType().GetProperty(EKafkaObjectType.NameDeletedDate);
                                        if (idProperty != null)
                                        {
                                            var idValue = idProperty.GetValue(value);
                                            var data = methodFind.Invoke(repository, new object[] { EKafkaObjectType.NameGetSyncKeyEntity, idValue }) as Task;
                                            if (data != null)
                                            {
                                                await data;
                                                var resultProperty = data.GetType().GetProperty(EKafkaObjectType.NameGetResultTask);
                                                if (resultProperty != null)
                                                {
                                                    var result = resultProperty?.GetValue(data);
                                                    var syncKeyProperty = result?.GetType().GetProperty(EKafkaObjectType.NameGetId);
                                                    if (syncKeyProperty != null)
                                                    {
                                                        actionType = EKafkaActionType.Update;
                                                        var syncKeyValue = syncKeyProperty.GetValue(result);
                                                        if (syncKeyValue != null)
                                                        {
                                                            idProperty.SetValue(value, syncKeyValue);
                                                        }
                                                    }
                                                    else
                                                    {
                                                        actionType = EKafkaActionType.Create;
                                                        idProperty.SetValue(value, 0);
                                                    }
                                                }
                                            }
                                            else
                                            {
                                                actionType = EKafkaActionType.Create;
                                                idProperty.SetValue(value, 0);
                                            }
                                            var syncKeyValueProperty = value.GetType().GetProperty(EKafkaObjectType.NameGetSyncKeyEntity);
                                            if (syncKeyValueProperty != null)
                                            {
                                                syncKeyValueProperty.SetValue(value, idValue);
                                            }
                                        }
                                        if (deletedProperty != null)
                                        {
                                            deletedProperty.SetValue(value, deleteStatus);

                                        }
                                        if (deletedDateProperty != null)
                                        {
                                            if (deleteStatus == YesNoEnum.Yes.ToEnumMemberString())
                                            {
                                                deletedDateProperty.SetValue(value, dateTimeNow);
                                            }
                                            else
                                            {
                                                deletedDateProperty.SetValue(value, null);
                                            }
                                        }
                                    }
                                }
                                else if (actionType == EKafkaActionType.Delete || actionType == EKafkaActionType.Remove)
                                {
                                    List<long>? idKeys = value as List<long>;
                                    List<long>? idEntitys = new List<long>();

                                    if (idKeys != null && idKeys.Any())
                                    {
                                        if (methodFind != null)
                                        {
                                            foreach (var idKey in idKeys)
                                            {
                                                var data = methodFind.Invoke(repository, new object[] { EKafkaObjectType.NameGetSyncKeyEntity, idKey }) as Task;
                                                if (data != null)
                                                {
                                                    await data;
                                                    var resultProperty = data.GetType().GetProperty(EKafkaObjectType.NameGetResultTask);
                                                    var result = resultProperty?.GetValue(data);
                                                    if (result != null)
                                                    {
                                                        var idProperty = result.GetType().GetProperty(EKafkaObjectType.NameGetId);
                                                        if (idProperty != null)
                                                        {
                                                            var idValue = idProperty.GetValue(result);
                                                            idEntitys.Add(idValue == null ? 0 : long.Parse(idValue.ToString()));
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                    }

                                    value = idEntitys;
                                }
                                var param = new object[] { value };
                                switch (actionType)
                                {
                                    case EKafkaActionType.Create:
                                        methodInfo = findTypeRepository.GetMethods().FirstOrDefault(x => x.Name == EKafkaMethodType.Insert && x.GetParameters().Any(t => t.ParameterType == targetType));
                                        break;
                                    case EKafkaActionType.Update:
                                        methodInfo = findTypeRepository.GetMethods().FirstOrDefault(x => x.Name == EKafkaMethodType.Update && x.GetParameters().Any(t => t.ParameterType == targetType));
                                        param = new object[] { value, false };
                                        break;
                                    case EKafkaActionType.Delete:
                                        if (value != null)
                                        {
                                            methodInfo = findTypeRepository.GetMethods().FirstOrDefault(x => x.Name == EKafkaMethodType.Delete && x.GetParameters().Any(t => t.ParameterType == targetType));
                                        }
                                        break;
                                    case EKafkaActionType.Remove:
                                        methodInfo = findTypeRepository.GetMethods().FirstOrDefault(x => x.Name == EKafkaMethodType.Remove && x.GetParameters().Any(t => t.ParameterType == targetType));
                                        break;
                                    default:
                                        break;
                                }

                                MethodInfo saveMethod = findTypeRepository.GetMethods().First(x => x.Name == EKafkaMethodType.SaveChange);
                                try
                                {
                                    if (dataProperty != null && targetType != null && methodInfo != null)
                                    {
                                        methodInfo.Invoke(repository, param);
                                        saveMethod.Invoke(repository, new object[] { });
                                    }
                                }
                                catch (Exception ex)
                                {
                                    _logger.LogError($"Error processing Kafka message: savechange {ex.InnerException}");
                                }

                            }
                        }
                        else
                        {
                            _logger.LogError($"Error processing Kafka message: Not found entity {nameEntity}");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"SyncMasterData => Error processing Kafka message: {ex.Message}");
            }
            finally
            {
                Task.CompletedTask.Wait();
            }
        }

        private void BookingResult(string message)
        {
            // check data type object or list object
            bool isJsonObject = true;
            var checkType = JsonConvert.DeserializeObject<KafkaDto.KafkaResponseDto<dynamic>>(message);
            if (checkType != null)
            {
                isJsonObject = checkType.Data is JObject;
            }

        }

    }

}
