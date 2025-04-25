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
    public interface IHotelService : IExtendEntityServiceAsync<HotelEntity, HotelDto.HotelRequestDto, HotelDto.HotelResponseDto, HotelDto.HotelSearchDto.HotelFilter>
    {

    }

    public class HotelService : ExtendedEntityServiceBase<HotelEntity, HotelDto.HotelRequestDto, HotelDto.HotelResponseDto, HotelDto.HotelSearchDto.HotelFilter>, IHotelService
    {
        private readonly ILogger<HotelService> _logger;
        private readonly IHotelRepository _repository;

        public HotelService(IHotelRepository repository, ILogger<HotelService> logger) : base(repository, logger)
        {
            _logger = logger;
            _repository = repository;
        }

    }
}
