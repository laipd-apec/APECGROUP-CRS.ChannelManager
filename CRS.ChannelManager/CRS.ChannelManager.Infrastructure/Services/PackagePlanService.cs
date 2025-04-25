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
    public interface IPackagePlanService : IExtendEntityServiceAsync<PackagePlanEntity, PackagePlanDto.PackagePlanRequestDto, PackagePlanDto.PackagePlanResponseDto, PackagePlanDto.PackagePlanSearchDto.PackagePlanFilter>
    {

    }

    public class PackagePlanService : ExtendedEntityServiceBase<PackagePlanEntity, PackagePlanDto.PackagePlanRequestDto, PackagePlanDto.PackagePlanResponseDto, PackagePlanDto.PackagePlanSearchDto.PackagePlanFilter>, IPackagePlanService
    {
        private readonly ILogger<PackagePlanService> _logger;
        private readonly IPackagePlanRepository _repository;

        public PackagePlanService(IPackagePlanRepository repository, ILogger<PackagePlanService> logger) : base(repository, logger)
        {
            _logger = logger;
            _repository = repository;
        }

    }
}
