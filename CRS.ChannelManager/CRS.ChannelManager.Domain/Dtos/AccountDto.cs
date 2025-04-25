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
    public class AccountDto
    {
        public class AccountRequestDto : RequestBaseDto
        {
            public long HotelId { get; set; }
            public string Code { get; set; }
            public string Name { get; set; }
            public string? TaxCode { get; set; }
            public string? TaxName { get; set; }
            public string? Address { get; set; }
            public string? Phone { get; set; }
            public string? Email { get; set; }
            public string? Description { get; set; }
            public long MarketSegmentId { get; set; }
            public string? NameUnaccent { get; set; }
            public long? SyncKey { get; set; }
        }

        public class AccountCreateDto : AccountRequestDto, ICommand<long>
        {

        }

        public class AccountUpdateDto : AccountRequestDto, ICommand<long>
        {

        }

        public class AccountGetOneDto : IQuery<AccountResponseDto>
        {
            //[FromQuery]
            public long Id { get; set; }
        }

        public class AccountDeleteDto : IQuery<long>
        {
            public long Id { get; set; }
        }

        public class AccountResponseDto : ResponseBaseDto
        {
            public long HotelId { get; set; }
            public string Code { get; set; }
            public string Name { get; set; }
            public string? TaxCode { get; set; }
            public string? TaxName { get; set; }
            public string? Address { get; set; }
            public string? Phone { get; set; }
            public string? Email { get; set; }
            public string? Description { get; set; }
            public long MarketSegmentId { get; set; }
            public string? NameUnaccent { get; set; }
            public long? SyncKey { get; set; }
        }

        public class AccountSearchDto : SearchBaseDto<AccountSearchDto.AccountFilter>, IQuery<PagedResultBaseDto<List<AccountResponseDto>>>
        {
            public class AccountFilter : FilterDtoBase
            {

            }
        }
    }
}
