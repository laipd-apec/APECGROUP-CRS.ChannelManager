using CRS.ChannelManager.Domain.Dtos.Interfaces;
using CRS.ChannelManager.Library.BaseDto;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRS.ChannelManager.Domain.Dtos
{
    public class IdentificationTypeDto
    {
        public class IdentificationTypeRequestDto : RequestBaseDto
        {
            public string Code { get; set; }
            public string Name { get; set; }
            public string? Description { get; set; }
            public string? DocumentConfig { get; set; }
            public long? SyncKey { get; set; }
        }

        public class IdentificationTypeCreateDto : IdentificationTypeRequestDto, ICommand<long>
        {

        }

        public class IdentificationTypeUpdateDto : IdentificationTypeRequestDto, ICommand<long>
        {

        }

        public class IdentificationTypeGetOneDto : IQuery<IdentificationTypeResponseDto>
        {
            //[FromQuery]
            public long Id { get; set; }
        }

        public class IdentificationTypeDeleteDto : IQuery<long>
        {
            public long Id { get; set; }
        }

        public class IdentificationTypeResponseDto : ResponseBaseDto
        {
            public string Code { get; set; }
            public string Name { get; set; }
            public string? Description { get; set; }
            public string? DocumentConfig { get; set; }
            public long? SyncKey { get; set; }
        }

        public class IdentificationTypeSearchDto : SearchBaseDto<IdentificationTypeSearchDto.IdentificationTypeFilter>, IQuery<PagedResultBaseDto<List<IdentificationTypeResponseDto>>>
        {
            public class IdentificationTypeFilter : FilterDtoBase
            {

            }
        }
    }
}
