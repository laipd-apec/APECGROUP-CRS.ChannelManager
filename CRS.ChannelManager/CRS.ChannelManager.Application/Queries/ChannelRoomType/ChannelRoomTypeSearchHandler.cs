using CRS.ChannelManager.Application.Common.Interfaces;
using CRS.ChannelManager.Domain.Dtos;
using CRS.ChannelManager.Infrastructure.Services;
using CRS.ChannelManager.Library.BaseDto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRS.ChannelManager.Application.Queries.ChannelRoomType
{
    public class ChannelRoomTypeSearchHandler : IQueryHandler<ChannelRoomTypeDto.ChannelRoomTypeSearchDto, PagedResultBaseDto<List<ChannelRoomTypeDto.ChannelRoomTypeResponseDto>>>
    {

        private readonly IChannelRoomTypeService _service;
        public ChannelRoomTypeSearchHandler(IChannelRoomTypeService service)
        {
            _service = service;
        }

        public async Task<PagedResultBaseDto<List<ChannelRoomTypeDto.ChannelRoomTypeResponseDto>>> Handle(ChannelRoomTypeDto.ChannelRoomTypeSearchDto request, CancellationToken cancellationToken)
        {
            return _service.Search(request);
        }
    }
}
