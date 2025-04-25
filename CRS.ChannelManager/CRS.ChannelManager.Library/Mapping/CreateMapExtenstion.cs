using AutoMapper;
using CRS.ChannelManager.Library.BaseDto;
using CRS.ChannelManager.Library.BaseEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRS.ChannelManager.Library.Mapper
{
    public class CreateMapExtenstion : AutoMapper.Profile, IMapperProfile
    {
        /// <summary>
        /// Order of this mapper implementation
        /// </summary>
        public int Order => 0;

        public CreateMapExtenstion()
        {
            CreateMap<AuditDto.AuditRequestDto, AuditEntity>().IgnoreNoMap();
            CreateMap<AuditEntity, AuditDto.AuditResponseDto>().IgnoreNoMap();

            CreateMap<MessageDto.MessageRequestDto, MessageEntity>().IgnoreNoMap();
            CreateMap<MessageEntity, MessageDto.MessageResponseDto>().IgnoreNoMap();

            CreateMap<RequestResponseLogDto.RequestResponseLogRequestDto, RequestResponseLogEntity>().IgnoreNoMap();
            CreateMap<RequestResponseLogEntity, RequestResponseLogDto.RequestResponseLogResponseDto>().IgnoreNoMap();
        }
    }
}
