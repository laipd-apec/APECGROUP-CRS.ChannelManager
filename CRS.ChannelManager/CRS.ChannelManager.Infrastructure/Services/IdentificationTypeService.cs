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
    public interface IIdentificationTypeService : IExtendEntityServiceAsync<IdentificationTypeEntity, IdentificationTypeDto.IdentificationTypeRequestDto, IdentificationTypeDto.IdentificationTypeResponseDto, IdentificationTypeDto.IdentificationTypeSearchDto.IdentificationTypeFilter>
    {

    }

    public class IdentificationTypeService : ExtendedEntityServiceBase<IdentificationTypeEntity, IdentificationTypeDto.IdentificationTypeRequestDto, IdentificationTypeDto.IdentificationTypeResponseDto, IdentificationTypeDto.IdentificationTypeSearchDto.IdentificationTypeFilter>, IIdentificationTypeService
    {
        private readonly ILogger<IdentificationTypeService> _logger;
        private readonly IIdentificationTypeRepository _repository;

        public IdentificationTypeService(IIdentificationTypeRepository repository, ILogger<IdentificationTypeService> logger) : base(repository, logger)
        {
            _logger = logger;
            _repository = repository;
        }

    }
}
