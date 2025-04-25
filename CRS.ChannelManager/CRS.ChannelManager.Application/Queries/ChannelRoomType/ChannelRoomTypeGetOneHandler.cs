using CRS.ChannelManager.Application.Common.Interfaces;
using CRS.ChannelManager.Domain.Dtos;
using CRS.ChannelManager.Infrastructure.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRS.ChannelManager.Application.Queries.ChannelRoomType
{
    public class ChannelRoomTypeGetOneHandler : IQueryHandler<ChannelRoomTypeDto.ChannelRoomTypeGetOneDto, ChannelRoomTypeDto.ChannelRoomTypeResponseDto>
    {
        private readonly IChannelRoomTypeService _service;
        public ChannelRoomTypeGetOneHandler(IChannelRoomTypeService service)
        {
            _service = service;
        }

        public async Task<ChannelRoomTypeDto.ChannelRoomTypeResponseDto> Handle(ChannelRoomTypeDto.ChannelRoomTypeGetOneDto request, CancellationToken cancellationToken)
        {
            return _service.GetById(request.Id);
        }
    }
}
