using CRS.ChannelManager.Domain.Dtos;
using CRS.ChannelManager.Domain.Entities;
using CRS.ChannelManager.Infrastructure.Repositories;
using CRS.ChannelManager.Library.Base;
using CRS.ChannelManager.Library.BaseInterface;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRS.ChannelManager.Infrastructure.Services
{
    public interface IChannelService : IExtendEntityServiceAsync<ChannelEntity, ChannelDto.ChannelRequestDto, ChannelDto.ChannelResponseDto, ChannelDto.ChannelSearchDto.ChannelFilter>
    {

    }

    public class ChannelService : ExtendedEntityServiceBase<ChannelEntity, ChannelDto.ChannelRequestDto, ChannelDto.ChannelResponseDto, ChannelDto.ChannelSearchDto.ChannelFilter>, IChannelService
    {
        private readonly ILogger<ChannelService> _logger;
        private readonly IChannelRepository _repository;

        public ChannelService(IChannelRepository repository, ILogger<ChannelService> logger) : base(repository, logger)
        {
            _logger = logger;
            _repository = repository;
        }

    }
}
