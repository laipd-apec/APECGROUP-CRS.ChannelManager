using CRS.ChannelManager.Infrastructure.Repositories;
using CRS.ChannelManager.Infrastructure.Services;
using CRS.ChannelManager.Infrastructure.Shares.Kafka;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRS.ChannelManager.Infrastructure
{
    public static class InfrastructureExtensions
    {
        public static void DependencyInjection(this IServiceCollection services)
        {
            services.AddTransient<CRSChannelManagerUnitOfWork, CRSChannelManagerUnitOfWork>();
            services.AddTransient<IProducerService, KafKaProducerService>();
            services.AddTransient<IConsumerService, KafkaConsumerService>();
            services.AddScoped<IAccountRepository, AccountRepository>();
            services.AddScoped<IAccountService, AccountService>();
            services.AddScoped<IChannelRepository, ChannelRepository>();
            services.AddScoped<IChannelService, ChannelService>();
            services.AddScoped<ICountryRepository, CountryRepository>();
            services.AddScoped<ICountryService, CountryService>();
            services.AddScoped<IHotelRepository, HotelRepository>();
            services.AddScoped<IHotelService, HotelService>();
            services.AddScoped<IIdentificationTypeRepository, IdentificationTypeRepository>();
            services.AddScoped<IIdentificationTypeService, IdentificationTypeService>();
            services.AddScoped<IMarketSegmentRepository, MarketSegmentRepository>();
            services.AddScoped<IMarketSegmentService, MarketSegmentService>();
            services.AddScoped<IPackagePlanRepository, PackagePlanRepository>();
            services.AddScoped<IPackagePlanService, PackagePlanService>();
            services.AddScoped<IPackageRepository, PackageRepository>();
            services.AddScoped<IPackageService, PackageService>();
            services.AddScoped<IProductRepository, ProductRepository>();
            services.AddScoped<IProductService, ProductService>();
            services.AddScoped<IRatePlanRepository, RatePlanRepository>();
            services.AddScoped<IRatePlanService, RatePlanService>();
            services.AddScoped<IRoomTypeRepository, RoomTypeRepository>();
            services.AddScoped<IRoomTypeService, RoomTypeService>();
            services.AddScoped<ISalutationRepository, SalutationRepository>();
            services.AddScoped<ISalutationService, SalutationService>();
            services.AddScoped<ISubSegmentRepository, SubSegmentRepository>();
            services.AddScoped<ISubSegmentService, SubSegmentService>();
            services.AddScoped<ITravelAgentRepository, TravelAgentRepository>();
            services.AddScoped<ITravelAgentService, TravelAgentService>();
            services.AddScoped<IChannelRoomTypeRepository, ChannelRoomTypeRepository>();
            services.AddScoped<IChannelRoomTypeService, ChannelRoomTypeService>();
            services.AddScoped<IChannelMappingRoomTypeRepository, ChannelMappingRoomTypeRepository>();
            services.AddScoped<IChannelMappingRoomTypeService, ChannelMappingRoomTypeService>();
        }
    }
}