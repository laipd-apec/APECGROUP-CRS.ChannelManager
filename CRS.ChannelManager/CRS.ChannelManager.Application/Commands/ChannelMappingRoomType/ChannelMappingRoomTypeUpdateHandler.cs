using CRS.ChannelManager.Domain.Dtos;
using CRS.ChannelManager.Infrastructure.Services;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRS.ChannelManager.Application.Commands.ChannelMappingRoomType
{
    public class ChannelMappingRoomTypeUpdateHandler : IRequestHandler<ChannelMappingRoomTypeDto.ChannelMappingRoomTypeUpdateDto, long>
    {
        private readonly IChannelMappingRoomTypeService _service;
        public ChannelMappingRoomTypeUpdateHandler(IChannelMappingRoomTypeService service)
        {
            _service = service;
        }

        public async Task<long> Handle(ChannelMappingRoomTypeDto.ChannelMappingRoomTypeUpdateDto request, CancellationToken cancellationToken)
        {
            var handler = _service.Update(request);
            return handler.Id;
        }

    }
}
