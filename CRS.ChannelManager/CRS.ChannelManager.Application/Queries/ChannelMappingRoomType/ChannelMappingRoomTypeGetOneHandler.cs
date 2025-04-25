using CRS.ChannelManager.Application.Common.Interfaces;
using CRS.ChannelManager.Domain.Dtos;
using CRS.ChannelManager.Infrastructure.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRS.ChannelManager.Application.Queries.ChannelMappingRoomType
{
    public class ChannelMappingRoomTypeGetOneHandler : IQueryHandler<ChannelMappingRoomTypeDto.ChannelMappingRoomTypeGetOneDto, ChannelMappingRoomTypeDto.ChannelMappingRoomTypeResponseDto>
    {
        private readonly IChannelMappingRoomTypeService _service;
        public ChannelMappingRoomTypeGetOneHandler(IChannelMappingRoomTypeService service)
        {
            _service = service;
        }

        public async Task<ChannelMappingRoomTypeDto.ChannelMappingRoomTypeResponseDto> Handle(ChannelMappingRoomTypeDto.ChannelMappingRoomTypeGetOneDto request, CancellationToken cancellationToken)
        {
            return _service.GetById(request.Id);
        }
    }
}
