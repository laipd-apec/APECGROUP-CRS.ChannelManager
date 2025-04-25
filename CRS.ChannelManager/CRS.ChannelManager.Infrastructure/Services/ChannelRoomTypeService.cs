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
using System.Text;
using System.Threading.Tasks;

namespace CRS.ChannelManager.Infrastructure.Services
{
    public interface IChannelRoomTypeService : IExtendEntityServiceAsync<ChannelRoomTypeEntity, ChannelRoomTypeDto.ChannelRoomTypeRequestDto, ChannelRoomTypeDto.ChannelRoomTypeResponseDto, ChannelRoomTypeDto.ChannelRoomTypeSearchDto.ChannelRoomTypeFilter>
    {

    }

    public class ChannelRoomTypeService : ExtendedEntityServiceBase<ChannelRoomTypeEntity, ChannelRoomTypeDto.ChannelRoomTypeRequestDto, ChannelRoomTypeDto.ChannelRoomTypeResponseDto, ChannelRoomTypeDto.ChannelRoomTypeSearchDto.ChannelRoomTypeFilter>, IChannelRoomTypeService
    {
        private readonly ILogger<ChannelRoomTypeService> _logger;
        private readonly IChannelRoomTypeRepository _repository;
        private readonly IHttpContextAccessor _httpContext;
        private readonly CRSChannelManagerUnitOfWork _unitOfWork;

        public ChannelRoomTypeService(
            IChannelRoomTypeRepository repository
            , ILogger<ChannelRoomTypeService> logger
            , IHttpContextAccessor httpContext
            , CRSChannelManagerUnitOfWork unitOfWork) : base(repository, logger)
        {
            _logger = logger;
            _repository = repository;
            _httpContext = httpContext;
            _unitOfWork = unitOfWork;
        }

        public override BaseStatusDto BeforCreate(ChannelRoomTypeDto.ChannelRoomTypeRequestDto dto, ChannelRoomTypeEntity entity)
        {
            var hotel = _unitOfWork.iHotelRepository.FindBySyncKey(dto.HotelId).Result;
            if (hotel == null)
            {
                throw new DomainExceptionBase("Not found hotel");
            }
            dto.HotelId = hotel.Id;
            entity.HotelId = hotel.Id;
            return base.BeforCreate(dto, entity);
        }

        public override void AfterCreate(ChannelRoomTypeDto.ChannelRoomTypeRequestDto dto, ChannelRoomTypeEntity entity)
        {
            base.AfterCreate(dto, entity);
        }

        public override BaseStatusDto BeforCreate(List<ChannelRoomTypeDto.ChannelRoomTypeRequestDto> dto, List<ChannelRoomTypeEntity> entity)
        {
            foreach (var item in entity)
            {
                var hotel = _unitOfWork.iHotelRepository.FindBySyncKey(item.HotelId).Result;
                if (hotel == null)
                {
                    throw new DomainExceptionBase("Not found hotel");
                }
                item.HotelId = hotel.Id;
            }
            return base.BeforCreate(dto, entity);
        }

        public override void AfterCreate(List<ChannelRoomTypeDto.ChannelRoomTypeRequestDto> dto, List<ChannelRoomTypeEntity> entity)
        {
            base.AfterCreate(dto, entity);
        }

        public override BaseStatusDto BeforUpdate(ChannelRoomTypeDto.ChannelRoomTypeRequestDto dto, ChannelRoomTypeEntity entity)
        {
            var hotel = _unitOfWork.iHotelRepository.FindBySyncKey(dto.HotelId).Result;
            if (hotel == null)
            {
                throw new DomainExceptionBase("Not found hotel");
            }
            dto.HotelId = hotel.Id;
            entity.HotelId = hotel.Id;
            return base.BeforUpdate(dto, entity);
        }

        public override void AfterUpdate(ChannelRoomTypeDto.ChannelRoomTypeRequestDto dto, ChannelRoomTypeEntity entity, ChannelRoomTypeEntity entityNew)
        {

            base.AfterUpdate(dto, entity, entityNew);
        }

        public override BaseStatusDto BeforUpdate(List<ChannelRoomTypeDto.ChannelRoomTypeRequestDto> dto, List<ChannelRoomTypeEntity> entity)
        {
            foreach (var item in entity)
            {
                var hotel = _unitOfWork.iHotelRepository.FindBySyncKey(item.HotelId).Result;
                if (hotel == null)
                {
                    throw new DomainExceptionBase("Not found hotel");
                }
                item.HotelId = hotel.Id;
            }
            return base.BeforUpdate(dto, entity);
        }

        public override void AfterUpdate(List<ChannelRoomTypeDto.ChannelRoomTypeRequestDto> dto, List<ChannelRoomTypeEntity> entity)
        {

            base.AfterUpdate(dto, entity);
        }

        protected override IQueryable<ChannelRoomTypeEntity> ExtendSearchQuery(IQueryable<ChannelRoomTypeEntity> query, ChannelRoomTypeDto.ChannelRoomTypeSearchDto.ChannelRoomTypeFilter filter)
        {
            query = query.Include(x => x.Hotel).Include(x => x.ChannelMappingRoomTypes);
            return base.ExtendSearchQuery(query, filter);
        }
    }
}
