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
    public class CountryDto
    {
        public class CountryRequestDto : RequestBaseDto
        {
            public string Code { get; set; }
            public string Name { get; set; }
            public string? Description { get; set; }
            public long? SyncKey { get; set; }
            //public string? Status { get; set; } = ActiveStatus.Active.ToEnumMemberString();
        }

        public class CountryCreateDto : CountryRequestDto, ICommand<long>
        {

        }

        public class CountryUpdateDto : CountryRequestDto, ICommand<long>
        {

        }

        public class CountryGetOneDto : IQuery<CountryResponseDto>
        {
            //[FromQuery]
            public long Id { get; set; }
        }

        public class CountryDeleteDto : IQuery<long>
        {
            public long Id { get; set; }
        }

        public class CountryResponseDto : ResponseBaseDto
        {
            public string Code { get; set; }
            public string Name { get; set; }
            public string? Description { get; set; }
            public long? SyncKey { get; set; }
            //public string? Status { get; set; } = ActiveStatus.Active.ToEnumMemberString();
        }

        public class CountrySearchDto : SearchBaseDto<CountrySearchDto.CountryFilter>, IQuery<PagedResultBaseDto<List<CountryResponseDto>>>
        {
            public class CountryFilter : FilterDtoBase
            {

            }
        }
    }
}
