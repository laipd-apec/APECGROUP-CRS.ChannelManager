using CRS.ChannelManager.Domain.Dtos;
using CRS.ChannelManager.Infrastructure.Services;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRS.ChannelManager.Application.Commands.ChannelRoomType
{
    public class ChannelRoomTypeCreateHandler : IRequestHandler<ChannelRoomTypeDto.ChannelRoomTypeCreateDto, long>
    {
        private readonly IChannelRoomTypeService _service;
        public ChannelRoomTypeCreateHandler(IChannelRoomTypeService service)
        {
            _service = service;
        }

        public async Task<long> Handle(ChannelRoomTypeDto.ChannelRoomTypeCreateDto request, CancellationToken cancellationToken)
        {
            var handler = _service.Create(request);
            return handler.Id;
        }

    }
}
