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
    public interface IMarketSegmentService : IExtendEntityServiceAsync<MarketSegmentEntity, MarketSegmentDto.MarketSegmentRequestDto, MarketSegmentDto.MarketSegmentResponseDto, MarketSegmentDto.MarketSegmentSearchDto.MarketSegmentFilter>
    {

    }

    public class MarketSegmentService : ExtendedEntityServiceBase<MarketSegmentEntity, MarketSegmentDto.MarketSegmentRequestDto, MarketSegmentDto.MarketSegmentResponseDto, MarketSegmentDto.MarketSegmentSearchDto.MarketSegmentFilter>, IMarketSegmentService
    {
        private readonly ILogger<MarketSegmentService> _logger;
        private readonly IMarketSegmentRepository _repository;

        public MarketSegmentService(IMarketSegmentRepository repository, ILogger<MarketSegmentService> logger) : base(repository, logger)
        {
            _logger = logger;
            _repository = repository;
        }

    }
}
