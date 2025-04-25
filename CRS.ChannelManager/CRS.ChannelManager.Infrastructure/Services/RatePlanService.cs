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
    public interface IRatePlanService : IExtendEntityServiceAsync<RatePlanEntity, RatePlanDto.RatePlanRequestDto, RatePlanDto.RatePlanResponseDto, RatePlanDto.RatePlanSearchDto.RatePlanFilter>
    {

    }

    public class RatePlanService : ExtendedEntityServiceBase<RatePlanEntity, RatePlanDto.RatePlanRequestDto, RatePlanDto.RatePlanResponseDto, RatePlanDto.RatePlanSearchDto.RatePlanFilter>, IRatePlanService
    {
        private readonly ILogger<RatePlanService> _logger;
        private readonly IRatePlanRepository _repository;

        public RatePlanService(IRatePlanRepository repository, ILogger<RatePlanService> logger) : base(repository, logger)
        {
            _logger = logger;
            _repository = repository;
        }

    }
}
