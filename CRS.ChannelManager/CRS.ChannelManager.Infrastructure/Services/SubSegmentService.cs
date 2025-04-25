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
    public interface ISubSegmentService : IExtendEntityServiceAsync<SubSegmentEntity, SubSegmentDto.SubSegmentRequestDto, SubSegmentDto.SubSegmentResponseDto, SubSegmentDto.SubSegmentSearchDto.SubSegmentFilter>
    {

    }

    public class SubSegmentService : ExtendedEntityServiceBase<SubSegmentEntity, SubSegmentDto.SubSegmentRequestDto, SubSegmentDto.SubSegmentResponseDto, SubSegmentDto.SubSegmentSearchDto.SubSegmentFilter>, ISubSegmentService
    {
        private readonly ILogger<SubSegmentService> _logger;
        private readonly ISubSegmentRepository _repository;

        public SubSegmentService(ISubSegmentRepository repository, ILogger<SubSegmentService> logger) : base(repository, logger)
        {
            _logger = logger;
            _repository = repository;
        }

    }
}
