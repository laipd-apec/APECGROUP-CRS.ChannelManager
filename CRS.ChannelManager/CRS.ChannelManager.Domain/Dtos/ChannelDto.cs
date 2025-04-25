using CRS.ChannelManager.Domain.Dtos.Interfaces;
using CRS.ChannelManager.Library.BaseDto;
using CRS.ChannelManager.Library.BaseEnum;
using CRS.ChannelManager.Library.Utils;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRS.ChannelManager.Domain.Dtos
{
    public class ChannelDto
    {
        public class ChannelRequestDto : RequestBaseDto
        {
            public string Code { get; set; }
            public string Name { get; set; }
            public string? Description { get; set; }
            public long? SyncKey { get; set; }
            //public string? Status { get; set; } = ActiveStatus.Active.ToEnumMemberString();
        }

        public class ChannelCreateDto : ChannelRequestDto, ICommand<long>
        {

        }

        public class ChannelUpdateDto : ChannelRequestDto, ICommand<long>
        {

        }

        public class ChannelGetOneDto : IQuery<ChannelResponseDto>
        {
            //[FromQuery]
            public long Id { get; set; }
        }

        public class ChannelDeleteDto : IQuery<long>
        {
            public long Id { get; set; }
        }

        public class ChannelResponseDto : ResponseBaseDto
        {
            public string Code { get; set; }
            public string Name { get; set; }
            public string? Description { get; set; }
            public long? SyncKey { get; set; }
            //public string? Status { get; set; } = ActiveStatus.Active.ToEnumMemberString();
        }

        public class ChannelSearchDto : SearchBaseDto<ChannelSearchDto.ChannelFilter>, IQuery<PagedResultBaseDto<List<ChannelResponseDto>>>
        {
            public class ChannelFilter : FilterDtoBase
            {

            }
        }
    }
}
