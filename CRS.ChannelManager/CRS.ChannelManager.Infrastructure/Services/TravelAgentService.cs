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
    public interface ITravelAgentService : IExtendEntityServiceAsync<TravelAgentEntity, TravelAgentDto.TravelAgentRequestDto, TravelAgentDto.TravelAgentResponseDto, TravelAgentDto.TravelAgentSearchDto.TravelAgentFilter>
    {

    }

    public class TravelAgentService : ExtendedEntityServiceBase<TravelAgentEntity, TravelAgentDto.TravelAgentRequestDto, TravelAgentDto.TravelAgentResponseDto, TravelAgentDto.TravelAgentSearchDto.TravelAgentFilter>, ITravelAgentService
    {
        private readonly ILogger<TravelAgentService> _logger;
        private readonly ITravelAgentRepository _repository;

        public TravelAgentService(ITravelAgentRepository repository, ILogger<TravelAgentService> logger) : base(repository, logger)
        {
            _logger = logger;
            _repository = repository;
        }

    }
}
