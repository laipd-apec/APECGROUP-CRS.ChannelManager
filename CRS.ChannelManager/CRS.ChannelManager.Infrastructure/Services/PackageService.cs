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
    public interface IPackageService : IExtendEntityServiceAsync<PackageEntity, PackageDto.PackageRequestDto, PackageDto.PackageResponseDto, PackageDto.PackageSearchDto.PackageFilter>
    {

    }

    public class PackageService : ExtendedEntityServiceBase<PackageEntity, PackageDto.PackageRequestDto, PackageDto.PackageResponseDto, PackageDto.PackageSearchDto.PackageFilter>, IPackageService
    {
        private readonly ILogger<PackageService> _logger;
        private readonly IPackageRepository _repository;

        public PackageService(IPackageRepository repository, ILogger<PackageService> logger) : base(repository, logger)
        {
            _logger = logger;
            _repository = repository;
        }

    }
}
