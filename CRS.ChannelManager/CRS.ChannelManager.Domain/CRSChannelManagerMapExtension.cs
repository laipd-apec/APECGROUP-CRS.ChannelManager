using CRS.ChannelManager.Domain.Dtos;
using CRS.ChannelManager.Domain.Entities;
using CRS.ChannelManager.Library.Mapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRS.ChannelManager.Domain
{
    public class CRSChannelManagerMapExtension : CreateMapExtenstion
    {
        public virtual int Order => 1;
        public CRSChannelManagerMapExtension()
        {
            CreateMap<AccountDto.AccountRequestDto, AccountEntity>().IgnoreNoChangeValue();
            CreateMap<AccountDto.AccountCreateDto, AccountEntity>().IgnoreNoChangeValue();
            CreateMap<AccountDto.AccountUpdateDto, AccountEntity>().IgnoreNoChangeValue();
            CreateMap<AccountEntity, AccountDto.AccountResponseDto>();

            CreateMap<ChannelDto.ChannelRequestDto, ChannelEntity>().IgnoreNoChangeValue();
            CreateMap<ChannelDto.ChannelCreateDto, ChannelEntity>().IgnoreNoChangeValue();
            CreateMap<ChannelDto.ChannelUpdateDto, ChannelEntity>().IgnoreNoChangeValue();
            CreateMap<ChannelEntity, ChannelDto.ChannelResponseDto>();

            CreateMap<CountryDto.CountryRequestDto, CountryEntity>().IgnoreNoChangeValue();
            CreateMap<CountryDto.CountryCreateDto, CountryEntity>().IgnoreNoChangeValue();
            CreateMap<CountryDto.CountryUpdateDto, CountryEntity>().IgnoreNoChangeValue();
            CreateMap<CountryEntity, CountryDto.CountryResponseDto>();

            CreateMap<HotelDto.HotelRequestDto, HotelEntity>().IgnoreNoChangeValue();
            CreateMap<HotelDto.HotelCreateDto, HotelEntity>().IgnoreNoChangeValue();
            CreateMap<HotelDto.HotelUpdateDto, HotelEntity>().IgnoreNoChangeValue();
            CreateMap<HotelEntity, HotelDto.HotelResponseDto>();

            CreateMap<IdentificationTypeDto.IdentificationTypeRequestDto, IdentificationTypeEntity>().IgnoreNoChangeValue();
            CreateMap<IdentificationTypeDto.IdentificationTypeCreateDto, IdentificationTypeEntity>().IgnoreNoChangeValue();
            CreateMap<IdentificationTypeDto.IdentificationTypeUpdateDto, IdentificationTypeEntity>().IgnoreNoChangeValue();
            CreateMap<IdentificationTypeEntity, IdentificationTypeDto.IdentificationTypeResponseDto>();

            CreateMap<MarketSegmentDto.MarketSegmentRequestDto, MarketSegmentEntity>().IgnoreNoChangeValue();
            CreateMap<MarketSegmentDto.MarketSegmentCreateDto, MarketSegmentEntity>().IgnoreNoChangeValue();
            CreateMap<MarketSegmentDto.MarketSegmentUpdateDto, MarketSegmentEntity>().IgnoreNoChangeValue();
            CreateMap<MarketSegmentEntity, MarketSegmentDto.MarketSegmentResponseDto>();

            CreateMap<PackageDto.PackageRequestDto, PackageEntity>().IgnoreNoChangeValue();
            CreateMap<PackageDto.PackageCreateDto, PackageEntity>().IgnoreNoChangeValue();
            CreateMap<PackageDto.PackageUpdateDto, PackageEntity>().IgnoreNoChangeValue();
            CreateMap<PackageEntity, PackageDto.PackageResponseDto>();

            CreateMap<PackagePlanDto.PackagePlanRequestDto, PackagePlanEntity>().IgnoreNoChangeValue();
            CreateMap<PackagePlanDto.PackagePlanCreateDto, PackagePlanEntity>().IgnoreNoChangeValue();
            CreateMap<PackagePlanDto.PackagePlanUpdateDto, PackagePlanEntity>().IgnoreNoChangeValue();
            CreateMap<PackagePlanEntity, PackagePlanDto.PackagePlanResponseDto>();

            CreateMap<ProductDto.ProductRequestDto, ProductEntity>().IgnoreNoChangeValue();
            CreateMap<ProductDto.ProductCreateDto, ProductEntity>().IgnoreNoChangeValue();
            CreateMap<ProductDto.ProductUpdateDto, ProductEntity>().IgnoreNoChangeValue();
            CreateMap<ProductEntity, ProductDto.ProductResponseDto>();

            CreateMap<RoomTypeDto.RoomTypeRequestDto, RoomTypeEntity>().IgnoreNoChangeValue();
            CreateMap<RoomTypeDto.RoomTypeCreateDto, RoomTypeEntity>().IgnoreNoChangeValue();
            CreateMap<RoomTypeDto.RoomTypeUpdateDto, RoomTypeEntity>().IgnoreNoChangeValue();
            CreateMap<RoomTypeEntity, RoomTypeDto.RoomTypeResponseDto>();

            CreateMap<RatePlanDto.RatePlanRequestDto, RatePlanEntity>().IgnoreNoChangeValue();
            CreateMap<RatePlanDto.RatePlanCreateDto, RatePlanEntity>().IgnoreNoChangeValue();
            CreateMap<RatePlanDto.RatePlanUpdateDto, RatePlanEntity>().IgnoreNoChangeValue();
            CreateMap<RatePlanEntity, RatePlanDto.RatePlanResponseDto>();

            CreateMap<SalutationDto.SalutationRequestDto, SalutationEntity>().IgnoreNoChangeValue();
            CreateMap<SalutationDto.SalutationCreateDto, SalutationEntity>().IgnoreNoChangeValue();
            CreateMap<SalutationDto.SalutationUpdateDto, SalutationEntity>().IgnoreNoChangeValue();
            CreateMap<SalutationEntity, SalutationDto.SalutationResponseDto>();

            CreateMap<ServiceDto.ServiceRequestDto, ServiceEntity>().IgnoreNoChangeValue();
            CreateMap<ServiceDto.ServiceCreateDto, ServiceEntity>().IgnoreNoChangeValue();
            CreateMap<ServiceDto.ServiceUpdateDto, ServiceEntity>().IgnoreNoChangeValue();
            CreateMap<ServiceEntity, ServiceDto.ServiceResponseDto>();

            CreateMap<SubSegmentDto.SubSegmentRequestDto, SubSegmentEntity>().IgnoreNoChangeValue();
            CreateMap<SubSegmentDto.SubSegmentCreateDto, SubSegmentEntity>().IgnoreNoChangeValue();
            CreateMap<SubSegmentDto.SubSegmentUpdateDto, SubSegmentEntity>().IgnoreNoChangeValue();
            CreateMap<SubSegmentEntity, SubSegmentDto.SubSegmentResponseDto>();

            CreateMap<TravelAgentDto.TravelAgentRequestDto, TravelAgentEntity>().IgnoreNoChangeValue();
            CreateMap<TravelAgentDto.TravelAgentCreateDto, TravelAgentEntity>().IgnoreNoChangeValue();
            CreateMap<TravelAgentDto.TravelAgentUpdateDto, TravelAgentEntity>().IgnoreNoChangeValue();
            CreateMap<TravelAgentEntity, TravelAgentDto.TravelAgentResponseDto>();

            CreateMap<ChannelRoomTypeDto.ChannelRoomTypeRequestDto, ChannelRoomTypeEntity>().IgnoreNoChangeValue();
            CreateMap<ChannelRoomTypeDto.ChannelRoomTypeCreateDto, ChannelRoomTypeEntity>().IgnoreNoChangeValue();
            CreateMap<ChannelRoomTypeDto.ChannelRoomTypeUpdateDto, ChannelRoomTypeEntity>().IgnoreNoChangeValue();
            CreateMap<ChannelRoomTypeEntity, ChannelRoomTypeDto.ChannelRoomTypeResponseDto>();

            CreateMap<ChannelMappingRoomTypeDto.ChannelMappingRoomTypeRequestDto, ChannelMappingRoomTypeEntity>().IgnoreNoChangeValue();
            CreateMap<ChannelMappingRoomTypeDto.ChannelMappingRoomTypeCreateDto, ChannelMappingRoomTypeEntity>().IgnoreNoChangeValue();
            CreateMap<ChannelMappingRoomTypeDto.ChannelMappingRoomTypeUpdateDto, ChannelMappingRoomTypeEntity>().IgnoreNoChangeValue();
            CreateMap<ChannelMappingRoomTypeEntity, ChannelMappingRoomTypeDto.ChannelMappingRoomTypeResponseDto>();
        }
    }
}
