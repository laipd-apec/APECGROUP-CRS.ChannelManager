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
    public class ChannelRoomTypeUpdateHandler : IRequestHandler<ChannelRoomTypeDto.ChannelRoomTypeUpdateDto, long>
    {
        private readonly IChannelRoomTypeService _service;
        public ChannelRoomTypeUpdateHandler(IChannelRoomTypeService service)
        {
            _service = service;
        }

        public async Task<long> Handle(ChannelRoomTypeDto.ChannelRoomTypeUpdateDto request, CancellationToken cancellationToken)
        {
            var handler = _service.Update(request);
            return handler.Id;
        }

    }
}
