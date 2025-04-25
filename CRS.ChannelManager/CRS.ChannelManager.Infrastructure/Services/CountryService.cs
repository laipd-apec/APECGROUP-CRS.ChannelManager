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
    public interface ICountryService : IExtendEntityServiceAsync<CountryEntity, CountryDto.CountryRequestDto, CountryDto.CountryResponseDto, CountryDto.CountrySearchDto.CountryFilter>
    {

    }

    public class CountryService : ExtendedEntityServiceBase<CountryEntity, CountryDto.CountryRequestDto, CountryDto.CountryResponseDto, CountryDto.CountrySearchDto.CountryFilter>, ICountryService
    {
        private readonly ILogger<CountryService> _logger;
        private readonly ICountryRepository _repository;

        public CountryService(ICountryRepository repository, ILogger<CountryService> logger) : base(repository, logger)
        {
            _logger = logger;
            _repository = repository;
        }

    }
}
