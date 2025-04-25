using CRS.ChannelManager.Application.Common.Interfaces;
using CRS.ChannelManager.Domain.Dtos;
using CRS.ChannelManager.Infrastructure.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRS.ChannelManager.Application.Commands.ChannelRoomType
{
    public class ChannelRoomTypeDeleteListHandler : IQueryHandler<ChannelRoomTypeDto.ChannelRoomTypeDeleteListDto, long>
    {
        private readonly IChannelRoomTypeService _service;
        public ChannelRoomTypeDeleteListHandler(IChannelRoomTypeService service)
        {
            _service = service;
        }

        public async Task<long> Handle(ChannelRoomTypeDto.ChannelRoomTypeDeleteListDto request, CancellationToken cancellationToken)
        {
            return _service.Delete(request.Ids);
        }
    }
}
