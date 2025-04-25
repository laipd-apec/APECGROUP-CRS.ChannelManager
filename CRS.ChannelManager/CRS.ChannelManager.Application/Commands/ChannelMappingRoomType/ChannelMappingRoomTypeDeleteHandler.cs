using CRS.ChannelManager.Application.Common.Interfaces;
using CRS.ChannelManager.Domain.Dtos;
using CRS.ChannelManager.Infrastructure.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRS.ChannelManager.Application.Commands.ChannelMappingRoomType
{
    public class ChannelMappingRoomTypeDeleteHandler : IQueryHandler<ChannelMappingRoomTypeDto.ChannelMappingRoomTypeDeleteDto, long>
    {
        private readonly IChannelMappingRoomTypeService _service;
        public ChannelMappingRoomTypeDeleteHandler(IChannelMappingRoomTypeService service)
        {
            _service = service;
        }

        public async Task<long> Handle(ChannelMappingRoomTypeDto.ChannelMappingRoomTypeDeleteDto request, CancellationToken cancellationToken)
        {
            return _service.Delete(request.Id);
        }
    }
}
