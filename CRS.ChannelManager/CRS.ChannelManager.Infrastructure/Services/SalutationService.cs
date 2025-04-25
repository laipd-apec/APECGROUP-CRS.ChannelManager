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
    public interface ISalutationService : IExtendEntityServiceAsync<SalutationEntity, SalutationDto.SalutationRequestDto, SalutationDto.SalutationResponseDto, SalutationDto.SalutationSearchDto.SalutationFilter>
    {

    }

    public class SalutationService : ExtendedEntityServiceBase<SalutationEntity, SalutationDto.SalutationRequestDto, SalutationDto.SalutationResponseDto, SalutationDto.SalutationSearchDto.SalutationFilter>, ISalutationService
    {
        private readonly ILogger<SalutationService> _logger;
        private readonly ISalutationRepository _repository;

        public SalutationService(ISalutationRepository repository, ILogger<SalutationService> logger) : base(repository, logger)
        {
            _logger = logger;
            _repository = repository;
        }

    }
}
