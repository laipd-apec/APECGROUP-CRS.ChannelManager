using CRS.ChannelManager.Domain.Dtos;
using CRS.ChannelManager.Domain.Entities;
using CRS.ChannelManager.Infrastructure.Repositories;
using CRS.ChannelManager.Library.Base;
using CRS.ChannelManager.Library.BaseDto;
using CRS.ChannelManager.Library.BaseInterface;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace CRS.ChannelManager.Infrastructure.Services
{
    public interface IChannelMappingRoomTypeService : IExtendEntityServiceAsync<ChannelMappingRoomTypeEntity, ChannelMappingRoomTypeDto.ChannelMappingRoomTypeRequestDto, ChannelMappingRoomTypeDto.ChannelMappingRoomTypeResponseDto, ChannelMappingRoomTypeDto.ChannelMappingRoomTypeSearchDto.ChannelMappingRoomTypeFilter>
    {

    }

    public class ChannelMappingRoomTypeService : ExtendedEntityServiceBase<ChannelMappingRoomTypeEntity, ChannelMappingRoomTypeDto.ChannelMappingRoomTypeRequestDto, ChannelMappingRoomTypeDto.ChannelMappingRoomTypeResponseDto, ChannelMappingRoomTypeDto.ChannelMappingRoomTypeSearchDto.ChannelMappingRoomTypeFilter>, IChannelMappingRoomTypeService
    {
        private readonly ILogger<ChannelMappingRoomTypeService> _logger;
        private readonly IChannelMappingRoomTypeRepository _repository;
        private readonly IHttpContextAccessor _httpContext;
        private readonly CRSChannelManagerUnitOfWork _unitOfWork;

        public ChannelMappingRoomTypeService(IChannelMappingRoomTypeRepository repository
            , ILogger<ChannelMappingRoomTypeService> logger
            , IHttpContextAccessor httpContext
            , CRSChannelManagerUnitOfWork unitOfWork) : base(repository, logger)
        {
            _logger = logger;
            _repository = repository;
            _httpContext = httpContext;
            _unitOfWork = unitOfWork;
        }

        public override BaseStatusDto BeforCreate(ChannelMappingRoomTypeDto.ChannelMappingRoomTypeRequestDto dto, ChannelMappingRoomTypeEntity entity)
        {
            var hotel = _unitOfWork.iHotelRepository.FindBySyncKey(dto.HotelId).Result;
            if (hotel == null)
            {
                throw new DomainExceptionBase("Not found hotel");
            }
            dto.HotelId = hotel.Id;
            entity.HotelId = hotel.Id;

            var channel = _unitOfWork.iChannelRepository.FindBySyncKey(dto.ChannelId).Result;
            if (channel == null)
            {
                throw new DomainExceptionBase("Not found channel");
            }
            dto.ChannelId = channel.Id;
            entity.ChannelId = channel.Id;

            var product = _unitOfWork.iProductRepository.FindBySyncKey(dto.ProductId).Result;
            if (product == null)
            {
                throw new DomainExceptionBase("Not found product");
            }
            dto.ProductId = product.Id;
            entity.ProductId = product.Id;

            var roomType = _unitOfWork.iRoomTypeRepository.FindBySyncKey(dto.RoomTypeId).Result;
            if (roomType == null)
            {
                throw new DomainExceptionBase("Not found room type");
            }
            dto.RoomTypeId = roomType.Id;
            entity.RoomTypeId = roomType.Id;

            var channelRoomType = _unitOfWork.iChannelRoomTypeRepository.FindById(dto.ChannelRoomTypeId);
            if (channelRoomType == null)
            {
                throw new DomainExceptionBase("Not found channel room type");
            }

            var account = _unitOfWork.iAccountRepository.FindBySyncKey(dto.AccountId).Result;
            if (account == null)
            {
                throw new DomainExceptionBase("Not found account");
            }
            dto.AccountId = account.Id;
            entity.AccountId = account.Id;

            var packagePlan = _unitOfWork.iPackagePlanRepository.FindBySyncKey(dto.PackagePlanId).Result;
            if (packagePlan == null)
            {
                throw new DomainExceptionBase("Not found package plan");
            }
            dto.PackagePlanId = packagePlan.Id;
            entity.PackagePlanId = packagePlan.Id;
            var lstError = new List<ErrorDto>();

            // check duplicate mapping
            var checkDuplicate = _repository.TableNoTracking
                                            .FirstOrDefault(x => x.HotelId == entity.HotelId
                                            && x.ChannelId == entity.ChannelId
                                            && x.ProductId == entity.ProductId
                                            && x.RoomTypeId == entity.RoomTypeId
                                            && x.ChannelRoomTypeId == entity.ChannelRoomTypeId
                                            && x.AccountId == entity.AccountId
                                            && x.PackagePlanId == entity.PackagePlanId);
            if (checkDuplicate != null)
            {
                lstError.Add(new ErrorDto("Duplicate Data"));
            }
            if (lstError.Any())
            {
                return new BaseStatusDto(StatusCode.Fail, lstError);
            }
            return base.BeforCreate(dto, entity);
        }

        public override void AfterCreate(ChannelMappingRoomTypeDto.ChannelMappingRoomTypeRequestDto dto, ChannelMappingRoomTypeEntity entity)
        {
            base.AfterCreate(dto, entity);
        }

        public override BaseStatusDto BeforCreate(List<ChannelMappingRoomTypeDto.ChannelMappingRoomTypeRequestDto> dto, List<ChannelMappingRoomTypeEntity> entity)
        {
            foreach (var item in entity)
            {
                var hotel = _unitOfWork.iHotelRepository.FindBySyncKey(item.HotelId).Result;
                if (hotel == null)
                {
                    throw new DomainExceptionBase("Not found hotel");
                }
                item.HotelId = hotel.Id;

                var channel = _unitOfWork.iChannelRepository.FindBySyncKey(item.ChannelId).Result;
                if (channel == null)
                {
                    throw new DomainExceptionBase("Not found channel");
                }
                item.ChannelId = channel.Id;

                var product = _unitOfWork.iProductRepository.FindBySyncKey(item.ProductId).Result;
                if (product == null)
                {
                    throw new DomainExceptionBase("Not found product");
                }
                item.ProductId = product.Id;

                var roomType = _unitOfWork.iRoomTypeRepository.FindBySyncKey(item.RoomTypeId).Result;
                if (roomType == null)
                {
                    throw new DomainExceptionBase("Not found room type");
                }
                item.RoomTypeId = roomType.Id;

                var channelRoomType = _unitOfWork.iChannelRoomTypeRepository.FindById(item.ChannelRoomTypeId);
                if (channelRoomType == null)
                {
                    throw new DomainExceptionBase("Not found channel room type");
                }

                var account = _unitOfWork.iAccountRepository.FindBySyncKey(item.AccountId).Result;
                if (account == null)
                {
                    throw new DomainExceptionBase("Not found account");
                }
                item.AccountId = account.Id;

                var packagePlan = _unitOfWork.iPackagePlanRepository.FindBySyncKey(item.PackagePlanId).Result;
                if (packagePlan == null)
                {
                    throw new DomainExceptionBase("Not found package plan");
                }
                item.PackagePlanId = packagePlan.Id;

                var lstError = new List<ErrorDto>();

                // check duplicate mapping
                var checkDuplicate = _repository.TableNoTracking
                                                .FirstOrDefault(x => x.HotelId == item.HotelId
                                                && x.ChannelId == item.ChannelId
                                                && x.ProductId == item.ProductId
                                                && x.RoomTypeId == item.RoomTypeId
                                                && x.ChannelRoomTypeId == item.ChannelRoomTypeId
                                                && x.AccountId == item.AccountId
                                                && x.PackagePlanId == item.PackagePlanId);
                if (checkDuplicate != null)
                {
                    lstError.Add(new ErrorDto("Duplicate Data"));
                }
                if (lstError.Any())
                {
                    return new BaseStatusDto(StatusCode.Fail, lstError);
                }
            }
            return base.BeforCreate(dto, entity);
        }

        public override BaseStatusDto BeforUpdate(ChannelMappingRoomTypeDto.ChannelMappingRoomTypeRequestDto dto, ChannelMappingRoomTypeEntity entity)
        {
            var hotel = _unitOfWork.iHotelRepository.FindBySyncKey(dto.HotelId).Result;
            if (hotel == null)
            {
                throw new DomainExceptionBase("Not found hotel");
            }
            dto.HotelId = hotel.Id;
            entity.HotelId = hotel.Id;

            var channel = _unitOfWork.iChannelRepository.FindBySyncKey(dto.ChannelId).Result;
            if (channel == null)
            {
                throw new DomainExceptionBase("Not found channel");
            }
            dto.ChannelId = channel.Id;
            entity.ChannelId = channel.Id;

            var product = _unitOfWork.iProductRepository.FindBySyncKey(dto.ProductId).Result;
            if (product == null)
            {
                throw new DomainExceptionBase("Not found product");
            }
            dto.ProductId = product.Id;
            entity.ProductId = product.Id;

            var roomType = _unitOfWork.iRoomTypeRepository.FindBySyncKey(dto.RoomTypeId).Result;
            if (roomType == null)
            {
                throw new DomainExceptionBase("Not found room type");
            }
            dto.RoomTypeId = roomType.Id;
            entity.RoomTypeId = roomType.Id;

            var channelRoomType = _unitOfWork.iChannelRoomTypeRepository.FindById(dto.ChannelRoomTypeId);
            if (channelRoomType == null)
            {
                throw new DomainExceptionBase("Not found channel room type");
            }

            var account = _unitOfWork.iAccountRepository.FindBySyncKey(dto.AccountId).Result;
            if (account == null)
            {
                throw new DomainExceptionBase("Not found account");
            }
            dto.AccountId = account.Id;
            entity.AccountId = account.Id;

            var packagePlan = _unitOfWork.iPackagePlanRepository.FindBySyncKey(dto.PackagePlanId).Result;
            if (packagePlan == null)
            {
                throw new DomainExceptionBase("Not found package plan");
            }
            dto.PackagePlanId = packagePlan.Id;
            entity.PackagePlanId = packagePlan.Id;
            var lstError = new List<ErrorDto>();

            // check duplicate mapping
            var checkDuplicate = _repository.TableNoTracking
                                            .FirstOrDefault(x => x.Id != entity.Id
                                            && x.HotelId == entity.HotelId
                                            && x.ChannelId == entity.ChannelId
                                            && x.ProductId == entity.ProductId
                                            && x.RoomTypeId == entity.RoomTypeId
                                            && x.ChannelRoomTypeId == entity.ChannelRoomTypeId
                                            && x.AccountId == entity.AccountId
                                            && x.PackagePlanId == entity.PackagePlanId);
            if (checkDuplicate != null)
            {
                lstError.Add(new ErrorDto("Duplicate Data"));
            }
            if (lstError.Any())
            {
                return new BaseStatusDto(StatusCode.Fail, lstError);
            }
            return base.BeforUpdate(dto, entity);
        }

        public override void AfterUpdate(ChannelMappingRoomTypeDto.ChannelMappingRoomTypeRequestDto dto, ChannelMappingRoomTypeEntity entity, ChannelMappingRoomTypeEntity entityNew)
        {

            base.AfterUpdate(dto, entity, entityNew);
        }

        public override BaseStatusDto BeforUpdate(List<ChannelMappingRoomTypeDto.ChannelMappingRoomTypeRequestDto> dto, List<ChannelMappingRoomTypeEntity> entity)
        {
            foreach (var item in entity)
            {
                var hotel = _unitOfWork.iHotelRepository.FindBySyncKey(item.HotelId).Result;
                if (hotel == null)
                {
                    throw new DomainExceptionBase("Not found hotel");
                }
                item.HotelId = hotel.Id;

                var channel = _unitOfWork.iChannelRepository.FindBySyncKey(item.ChannelId).Result;
                if (channel == null)
                {
                    throw new DomainExceptionBase("Not found channel");
                }
                item.ChannelId = channel.Id;

                var product = _unitOfWork.iProductRepository.FindBySyncKey(item.ProductId).Result;
                if (product == null)
                {
                    throw new DomainExceptionBase("Not found product");
                }
                item.ProductId = product.Id;

                var roomType = _unitOfWork.iRoomTypeRepository.FindBySyncKey(item.RoomTypeId).Result;
                if (roomType == null)
                {
                    throw new DomainExceptionBase("Not found room type");
                }
                item.RoomTypeId = roomType.Id;

                var channelRoomType = _unitOfWork.iChannelRoomTypeRepository.FindById(item.ChannelRoomTypeId);
                if (channelRoomType == null)
                {
                    throw new DomainExceptionBase("Not found channel room type");
                }

                var account = _unitOfWork.iAccountRepository.FindBySyncKey(item.AccountId).Result;
                if (account == null)
                {
                    throw new DomainExceptionBase("Not found account");
                }
                item.AccountId = account.Id;

                var packagePlan = _unitOfWork.iPackagePlanRepository.FindBySyncKey(item.PackagePlanId).Result;
                if (packagePlan == null)
                {
                    throw new DomainExceptionBase("Not found package plan");
                }
                item.PackagePlanId = packagePlan.Id;

                var lstError = new List<ErrorDto>();

                // check duplicate mapping
                var checkDuplicate = _repository.TableNoTracking
                                                .FirstOrDefault(x => x.Id != item.Id
                                                && x.HotelId == item.HotelId
                                                && x.ChannelId == item.ChannelId
                                                && x.ProductId == item.ProductId
                                                && x.RoomTypeId == item.RoomTypeId
                                                && x.ChannelRoomTypeId == item.ChannelRoomTypeId
                                                && x.AccountId == item.AccountId
                                                && x.PackagePlanId == item.PackagePlanId);
                if (checkDuplicate != null)
                {
                    lstError.Add(new ErrorDto("Duplicate Data"));
                }
                if (lstError.Any())
                {
                    return new BaseStatusDto(StatusCode.Fail, lstError);
                }
            }
            return base.BeforUpdate(dto, entity);
        }

        public override void AfterUpdate(List<ChannelMappingRoomTypeDto.ChannelMappingRoomTypeRequestDto> dto, List<ChannelMappingRoomTypeEntity> entity)
        {
            base.AfterUpdate(dto, entity);
        }

        protected override IQueryable<ChannelMappingRoomTypeEntity> ExtendSearchQuery(IQueryable<ChannelMappingRoomTypeEntity> query, ChannelMappingRoomTypeDto.ChannelMappingRoomTypeSearchDto.ChannelMappingRoomTypeFilter filter)
        {
            query = query.Include(x => x.ChannelRoomType)
                        .Include(x => x.Hotel)
                        .Include(x => x.Channel)
                        .Include(x => x.Product)
                        .Include(x => x.RoomType)
                        .Include(x => x.Account)
                        .Include(x => x.PackagePlan);

            return base.ExtendSearchQuery(query, filter);
        }
    }
}
