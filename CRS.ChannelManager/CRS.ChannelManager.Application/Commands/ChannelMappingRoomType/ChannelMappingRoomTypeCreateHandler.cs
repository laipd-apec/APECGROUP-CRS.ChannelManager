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
    public class ChannelMappingRoomTypeCreateHandler : IRequestHandler<ChannelMappingRoomTypeDto.ChannelMappingRoomTypeCreateDto, long>
    {
        private readonly IChannelMappingRoomTypeService _service;
        public ChannelMappingRoomTypeCreateHandler(IChannelMappingRoomTypeService service)
        {
            _service = service;
        }

        public async Task<long> Handle(ChannelMappingRoomTypeDto.ChannelMappingRoomTypeCreateDto request, CancellationToken cancellationToken)
        {
            var handler = _service.Create(request);
            return handler.Id;
        }

    }
}
