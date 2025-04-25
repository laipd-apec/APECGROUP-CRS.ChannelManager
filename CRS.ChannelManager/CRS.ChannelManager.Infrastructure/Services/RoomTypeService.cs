using CRS.ChannelManager.Domain.Dtos;
using CRS.ChannelManager.Domain.Entities;
using CRS.ChannelManager.Infrastructure.Repositories;
using CRS.ChannelManager.Library.Base;
using CRS.ChannelManager.Library.BaseInterface;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRS.ChannelManager.Infrastructure.Services
{
    public interface IRoomTypeService : IExtendEntityServiceAsync<RoomTypeEntity, RoomTypeDto.RoomTypeRequestDto, RoomTypeDto.RoomTypeResponseDto, RoomTypeDto.RoomTypeSearchDto.RoomTypeFilter>
    {

    }

    public class RoomTypeService : ExtendedEntityServiceBase<RoomTypeEntity, RoomTypeDto.RoomTypeRequestDto, RoomTypeDto.RoomTypeResponseDto, RoomTypeDto.RoomTypeSearchDto.RoomTypeFilter>, IRoomTypeService
    {
        private readonly ILogger<RoomTypeService> _logger;
        private readonly IRoomTypeRepository _repository;

        public RoomTypeService(IRoomTypeRepository repository, ILogger<RoomTypeService> logger) : base(repository, logger)
        {
            _logger = logger;
            _repository = repository;
        }

    }
}
