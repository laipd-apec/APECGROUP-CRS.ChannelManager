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
    public class ProductDto
    {
        public class ProductRequestDto : RequestBaseDto
        {
            public string Code { get; set; }
            public string Name { get; set; }
            public string? Description { get; set; }
            public long? SyncKey { get; set; }
            public string? HotelId { get; set; }
            //public string? Status { get; set; } = ActiveStatus.Active.ToEnumMemberString();
        }

        public class ProductCreateDto : ProductRequestDto, ICommand<long>
        {

        }

        public class ProductUpdateDto : ProductRequestDto, ICommand<long>
        {

        }

        public class ProductGetOneDto : IQuery<ProductResponseDto>
        {
            //[FromQuery]
            public long Id { get; set; }
        }

        public class ProductDeleteDto : IQuery<long>
        {
            public long Id { get; set; }
        }

        public class ProductResponseDto : ResponseBaseDto
        {
            public string Code { get; set; }
            public string Name { get; set; }
            public string? Description { get; set; }
            public long? SyncKey { get; set; }
            public string? HotelId { get; set; }
            //public string? Status { get; set; } = ActiveStatus.Active.ToEnumMemberString();
        }

        public class ProductSearchDto : SearchBaseDto<ProductSearchDto.ProductFilter>, IQuery<PagedResultBaseDto<List<ProductResponseDto>>>
        {
            public class ProductFilter : FilterDtoBase
            {

            }
        }
    }
}
