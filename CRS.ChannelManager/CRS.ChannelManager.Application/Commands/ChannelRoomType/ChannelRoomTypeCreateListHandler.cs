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
    public class ChannelRoomTypeCreateListHandler : IRequestHandler<ChannelRoomTypeDto.ChannelRoomTypeCreateListDto, List<long>>
    {
        private readonly IChannelRoomTypeService _service;
        public ChannelRoomTypeCreateListHandler(IChannelRoomTypeService service)
        {
            _service = service;
        }

        public async Task<List<long>> Handle(ChannelRoomTypeDto.ChannelRoomTypeCreateListDto request, CancellationToken cancellationToken)
        {
            var handler = _service.Create(request.Items);
            return handler.Select(x => x.Id).ToList();
        }

    }
}
