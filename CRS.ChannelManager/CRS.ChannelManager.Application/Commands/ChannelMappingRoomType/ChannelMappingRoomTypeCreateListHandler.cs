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
    public class ChannelMappingRoomTypeCreateListHandler : IRequestHandler<ChannelMappingRoomTypeDto.ChannelMappingRoomTypeCreateListDto, List<long>>
    {
        private readonly IChannelMappingRoomTypeService _service;
        public ChannelMappingRoomTypeCreateListHandler(IChannelMappingRoomTypeService service)
        {
            _service = service;
        }

        public async Task<List<long>> Handle(ChannelMappingRoomTypeDto.ChannelMappingRoomTypeCreateListDto request, CancellationToken cancellationToken)
        {
            var handler = _service.Create(request.Items);
            return handler.Select(x => x.Id).ToList();
        }

    }
}
