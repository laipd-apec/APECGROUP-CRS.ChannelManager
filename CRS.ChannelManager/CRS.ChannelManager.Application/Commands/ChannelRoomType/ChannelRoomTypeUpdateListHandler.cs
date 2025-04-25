using CRS.ChannelManager.Domain.Dtos;
using CRS.ChannelManager.Infrastructure.Services;
using CRS.ChannelManager.Library.Mapper;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRS.ChannelManager.Application.Commands.ChannelRoomType
{
    public class ChannelRoomTypeUpdateListHandler : IRequestHandler<ChannelRoomTypeDto.ChannelRoomTypeUpdateListDto, List<long>>
    {
        private readonly IChannelRoomTypeService _service;
        public ChannelRoomTypeUpdateListHandler(IChannelRoomTypeService service)
        {
            _service = service;
        }

        public async Task<List<long>> Handle(ChannelRoomTypeDto.ChannelRoomTypeUpdateListDto request, CancellationToken cancellationToken)
        {
            var handler = _service.Update(request.Items);
            return handler.Select(x => x.Id).ToList();
        }

    }
}
