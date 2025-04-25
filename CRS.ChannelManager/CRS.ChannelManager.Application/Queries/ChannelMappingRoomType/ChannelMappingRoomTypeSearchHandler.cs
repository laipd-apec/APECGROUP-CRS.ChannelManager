using CRS.ChannelManager.Application.Common.Interfaces;
using CRS.ChannelManager.Domain.Dtos;
using CRS.ChannelManager.Infrastructure.Services;
using CRS.ChannelManager.Library.BaseDto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRS.ChannelManager.Application.Queries.ChannelMappingRoomType
{
    public class ChannelMappingRoomTypeSearchHandler : IQueryHandler<ChannelMappingRoomTypeDto.ChannelMappingRoomTypeSearchDto, PagedResultBaseDto<List<ChannelMappingRoomTypeDto.ChannelMappingRoomTypeResponseDto>>>
    {

        private readonly IChannelMappingRoomTypeService _service;
        public ChannelMappingRoomTypeSearchHandler(IChannelMappingRoomTypeService service)
        {
            _service = service;
        }

        public async Task<PagedResultBaseDto<List<ChannelMappingRoomTypeDto.ChannelMappingRoomTypeResponseDto>>> Handle(ChannelMappingRoomTypeDto.ChannelMappingRoomTypeSearchDto request, CancellationToken cancellationToken)
        {
            return _service.Search(request);
        }
    }
}
