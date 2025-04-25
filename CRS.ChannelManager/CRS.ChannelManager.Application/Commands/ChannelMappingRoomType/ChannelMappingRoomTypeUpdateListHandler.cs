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
    public class ChannelMappingRoomTypeUpdateListHandler : IRequestHandler<ChannelMappingRoomTypeDto.ChannelMappingRoomTypeUpdateListDto, List<long>>
    {
        private readonly IChannelMappingRoomTypeService _service;
        public ChannelMappingRoomTypeUpdateListHandler(IChannelMappingRoomTypeService service)
        {
            _service = service;
        }

        public async Task<List<long>> Handle(ChannelMappingRoomTypeDto.ChannelMappingRoomTypeUpdateListDto request, CancellationToken cancellationToken)
        {
            var handler = _service.Update(request.Items);
            return handler.Select(x => x.Id).ToList();
        }

    }
}
