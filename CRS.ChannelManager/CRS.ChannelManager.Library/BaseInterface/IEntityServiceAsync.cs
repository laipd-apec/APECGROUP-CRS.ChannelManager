using CRS.ChannelManager.Library.Base;
using CRS.ChannelManager.Library.BaseDto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRS.ChannelManager.Library.BaseInterface
{
    public partial interface IEntityServiceAsync
    {

    }

    public partial interface IEntityServiceAsync<Entity, ReqDto, ResDto> : IEntityServiceAsync where Entity : EntityBase where ReqDto : RequestBaseDto where ResDto : ResponseBaseDto
    {
        ResDto GetById(long id);
        ResDto Create(ReqDto request);
        Task<ResDto> CreateAsync(ReqDto request);
        List<ResDto> Create(List<ReqDto> request);
        Task<Object> CreateAsync(List<ReqDto> request);
        ResDto Update(ReqDto request, bool withChildren = false);
        Task<ResDto> UpdateAsync(ReqDto request, bool withChildren = false);
        ResDto UpdateStatus(long id, bool withChildren = false);
        Task<ResDto> UpdateStatusAsync(long id, bool withChildren = false);
        List<ResDto> UpdateStatusList(List<long> id, bool withChildren = false);
        Task<List<ResDto>> UpdateStatusListAsync(List<long> id, bool withChildren = false);
        List<ResDto> Update(List<ReqDto> request, bool withChildren = false);
        Task<List<ResDto>> UpdateAsync(List<ReqDto> request, bool withChildren = false);
        long Delete(long id);
        Task<bool> DeleteAsync(long id);
        long Delete(List<long> ids);
        Task<bool> DeleteAsync(List<long> ids);
        List<ResDto> All();
        List<string> CheckWarningData(ReqDto request);
        Object getNextGenCode(string headCode, long numberFormat);
        //Task<Object> ImportAsync(List<ReqDto> requests, bool overWrite);
    }

    /// <summary>
    /// if entity has Search event use it
    /// </summary>
    public partial interface IExtendEntityServiceAsync<Entity, ReqDto, ResDto, Filter>
        : IEntityServiceAsync<Entity, ReqDto, ResDto>
        where Entity : EntityBase where ReqDto : RequestBaseDto where ResDto : ResponseBaseDto where Filter : FilterDtoBase
    {
        PagedResultBaseDto<List<ResDto>> Search(SearchBaseDto<Filter> search);
        Task<Object> ImportAsync(List<ReqDto> requests, bool overWrite);
        Task<Entity> FindIdByCode(Entity inputEntity, bool isUpdate);
    }
}
