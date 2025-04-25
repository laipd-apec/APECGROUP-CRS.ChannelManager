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
    public class SalutationDto
    {
        public class SalutationRequestDto : RequestBaseDto
        {
            public string Code { get; set; }
            public string Name { get; set; }
            public long? SyncKey { get; set; }
        }

        public class SalutationCreateDto : SalutationRequestDto, ICommand<long>
        {

        }

        public class SalutationUpdateDto : SalutationRequestDto, ICommand<long>
        {

        }

        public class SalutationGetOneDto : IQuery<SalutationResponseDto>
        {
            //[FromQuery]
            public long Id { get; set; }
        }

        public class SalutationDeleteDto : IQuery<long>
        {
            public long Id { get; set; }
        }

        public class SalutationResponseDto : ResponseBaseDto
        {
            public string Code { get; set; }
            public string Name { get; set; }
            public long? SyncKey { get; set; }
        }

        public class SalutationSearchDto : SearchBaseDto<SalutationSearchDto.SalutationFilter>, IQuery<PagedResultBaseDto<List<SalutationResponseDto>>>
        {
            public class SalutationFilter : FilterDtoBase
            {

            }
        }
    }
}
