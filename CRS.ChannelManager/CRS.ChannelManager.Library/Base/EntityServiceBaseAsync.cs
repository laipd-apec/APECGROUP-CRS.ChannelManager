using CRS.ChannelManager.Library.EnumType;
using CRS.ChannelManager.Library.HanderException;
using CRS.ChannelManager.Library.Helpper;
using CRS.ChannelManager.Library.BaseInterface;
using CRS.ChannelManager.Library.Mapper;
using CRS.ChannelManager.Library.BaseDto;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using static CRS.ChannelManager.Library.Constants.CommonConstants;
using Microsoft.EntityFrameworkCore;
using Elasticsearch.Net;
using Nest;
using Microsoft.AspNetCore.Mvc;
using System.Reflection;
using RestSharp.Extensions;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.Extensions.Logging;

namespace CRS.ChannelManager.Library.Base
{
    public abstract class EntityServiceBaseAsync<Entity, ReqDto, ResDto>
        : IEntityServiceAsync<Entity, ReqDto, ResDto> where Entity : EntityBase where ReqDto : RequestBaseDto where ResDto : ResponseBaseDto
    {
        private readonly ILogger? _logger = null;
        protected IAsyncRepository<Entity> _repository { get; set; }
        protected Entity _entity { get; set; }
        protected List<Entity> _entities { get; set; }
        protected Entity _entityOld { get; set; }

        public EntityServiceBaseAsync(IAsyncRepository<Entity> repository, ILogger? logger = null)
        {
            _logger = logger;
            _repository = repository;
        }

        public ResDto GetById(long id)
        {
            _entity = _repository.FindById(id);
            if (_entity == null)
            {
                throw new DomainExceptionBase($"{typeof(Entity).Name} not found object !");
            }
            var dto = MappingExtensions.MapTo<Entity, ResDto>(_entity);
            BeforeGetById(dto);
            return dto;
        }

        public ResDto Create(ReqDto request)
        {
            try
            {
                _entity = MappingExtensions.MapTo<ReqDto, Entity>(request);
                var checkCreate = BeforCreate(request, _entity);
                if (checkCreate.Status == StatusCode.Fail || checkCreate.Status == StatusCode.Waring)
                {
                    var s = JsonSerializer.Serialize(checkCreate.ListCode);
                    //new DomainExceptionBase(checkCreate.ListCode);
                    throw new DomainExceptionBase(checkCreate.ListCode);
                }

                _repository.Insert(_entity);
                _repository.SaveChange();
                AfterCreate(request, _entity);
                return MappingExtensions.MapTo<Entity, ResDto>(_entity);
            }
            catch (DomainExceptionBase ex)
            {
                throw new DomainExceptionBase(ex.ErrorDtos);
            }

        }

        public async Task<ResDto> CreateAsync(ReqDto request)
        {
            _entity = MappingExtensions.MapTo<ReqDto, Entity>(request);
            var checkCreate = BeforCreate(request, _entity);
            if (checkCreate.Status == StatusCode.Fail || checkCreate.Status == StatusCode.Waring)
            {
                //var s = JsonSerializer.Serialize(checkCreate.ListCode);
                //new DomainExceptionBase(checkCreate.ListCode);
                throw new DomainExceptionBase(checkCreate.ListCode);
            }
            await _repository.AddAsync(_entity);
            await _repository.SaveChangeAsync();
            AfterCreate(request, _entity);
            return await Task.FromResult(MappingExtensions.MapTo<Entity, ResDto>(_entity));
        }

        public List<ResDto> Create(List<ReqDto> request)
        {
            _entities = MappingExtensions.MapTo<List<ReqDto>, List<Entity>>(request);
            var checkCreate = BeforCreate(request, _entities);
            if (checkCreate.Status == StatusCode.Fail || checkCreate.Status == StatusCode.Waring)
            {
                throw new DomainExceptionBase(checkCreate.ListCode);
            }
            _repository.Insert(_entities);
            _repository.SaveChange();
            AfterCreate(request, _entities);
            return MappingExtensions.MapTo<List<Entity>, List<ResDto>>(_entities);
        }

        public async Task<Object> CreateAsync(List<ReqDto> requests)
        {
            //_entities = MappingExtensions.MapTo<List<ReqDto>, List<Entity>>(request);
            List<Object> result = new List<Object>();
            foreach (var request in requests)
            {
                _entity = MappingExtensions.MapTo<ReqDto, Entity>(request);
                var checkCreate = BeforCreate(request, _entity);
                if (checkCreate.Status == StatusCode.Fail || checkCreate.Status == StatusCode.Waring)
                {
                    //throw new DomainExceptionBase(checkCreate.ListCode);
                    result.Add(new { entity = MappingExtensions.MapTo<Entity, ResDto>(_entity), error = new DomainExceptionBase(checkCreate.ListCode) });
                }
                else
                {
                    await _repository.AddAsync(_entity);
                    await _repository.SaveChangeAsync();
                    AfterCreate(request, _entity);
                }
            }

            //await _repository.AddAsync(_entities);
            //await _repository.SaveChangeAsync();
            //return await Task.FromResult(MappingExtensions.MapTo<List<Entity>, List<ResDto>>(_entities));
            return await Task.FromResult(result);
        }

        public List<string> CheckWarningData(ReqDto request)
        {
            List<string> warningMessages = new List<string>();
            try
            {
                _entity = MappingExtensions.MapTo<ReqDto, Entity>(request);
                var checkCreate = BeforCreate(request, _entity);
                if (checkCreate.Status == StatusCode.Waring || checkCreate.Status == StatusCode.Fail)
                {
                    //throw new DomainExceptionBase(checkCreate.ListCode);
                    var errors = new DomainExceptionBase(checkCreate.ListCode).ErrorDtos;
                    foreach (var error in errors)
                    {
                        warningMessages.Add(error.Message);
                    }
                }
                if (Attribute.GetCustomAttribute(request.GetType(), typeof(ImportCheckDataViaCodeDto)) != null)
                {
                    BaseStatusDto res;
                    var properties = request.GetType().GetProperties();
                    ImportReferedEntity attribute;
                    foreach (PropertyInfo propertyInfo in properties)
                    {
                        if ((attribute = propertyInfo.GetAttribute<ImportReferedEntity>()) != null)
                        {
                            string codeColumn = "CODE";
                            foreach (PropertyInfo propertyChild in attribute.Entity.GetProperties())
                            {
                                if (propertyChild.GetAttribute<ImportCode>() != null)
                                {
                                    codeColumn = propertyChild.GetAttribute<ColumnAttribute>().Name;
                                }
                            }
                            var value = CommonHelper.GetPropertyValue(request, propertyInfo.Name)?.ToString();
                            if (this is ICodeVerifySupport code && attribute.Entity.GetInterface(nameof(ICodeSupportEntity)) != null)
                            {
                                var result = code.CodeVerify((EntityBase)Activator.CreateInstance(attribute.Entity), value);
                                if (!result.HasValue)
                                {
                                    warningMessages.Add($"Data in {propertyInfo.Name} does not exist");
                                }
                                else if (!result.Value)
                                {
                                    warningMessages.Add($"Data in {propertyInfo.Name} is not activated");
                                }
                            }
                            else
                            {
                                TableAttribute tableAttribute;
                                if ((tableAttribute = attribute.Entity.GetAttribute<TableAttribute>()) != null)
                                {
                                    var codes = _repository.GetImportCodesViaTableName(tableAttribute, codeColumn);
                                    if (!string.IsNullOrEmpty(value) && !codes.Exists(p => p.Item2 == value))
                                    {
                                        warningMessages.Add($"Data in {propertyInfo.Name} does not exist");
                                    }
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                //warningMessages.Add(ex.Message);
            }
            return warningMessages;
        }

        public ResDto Update(ReqDto request, bool withChildren = false)
        {
            _entityOld = _repository.FindOriginalById(request.Id);
            if (_entityOld == null)
            {
                throw new DomainExceptionBase($"{typeof(Entity).Name} not found object !");
            }
            _entity = _repository.TableNoTracking.First(x => x.Id == request.Id);
            var check = BeforUpdate(request, _entity);
            if ((check.Status == StatusCode.Fail || check.Status == StatusCode.Waring) && check.ListCode.Any())
            {
                throw new DomainExceptionBase(check.ListCode);
            }
            //MappingExtensions.SimpleMap<ReqDto, Entity>(request, _entity);
            _entity.CreatedBy = _entityOld.CreatedBy;
            _entity.CreatedDate = _entityOld.CreatedDate;
            if (!string.IsNullOrEmpty(request.Deleted))
            {
                _entity.Deleted = request.Deleted;
                _entity.DeletedDate = DateTime.Now;
            }
            else
            {
                _entity.Deleted = _entityOld.Deleted;
            }
            _repository.Update(_entity, withChildren);
            _repository.SaveChange();
            AfterUpdate(request, _entityOld, _entity);
            return MappingExtensions.MapTo<Entity, ResDto>(_entity);
        }

        public async Task<ResDto> UpdateAsync(ReqDto request, bool withChildren = false)
        {
            _entityOld = _repository.FindOriginalById(request.Id);
            if (_entity == null)
            {
                throw new DomainExceptionBase($"{typeof(Entity).Name} not found object !");
            }
            _entity = _repository.TableNoTracking.First(x => x.Id == request.Id);
            var checkCreate = BeforUpdate(request, _entity);
            //MappingExtensions.SimpleMap<ReqDto, Entity>(request, _entity);

            if (checkCreate.Status == StatusCode.Fail || checkCreate.Status == StatusCode.Waring)
            {
                throw new DomainExceptionBase(checkCreate.ListCode);
            }
            _entity.CreatedBy = _entityOld.CreatedBy;
            _entity.CreatedDate = _entityOld.CreatedDate;
            if (!string.IsNullOrEmpty(request.Deleted))
            {
                _entity.Deleted = request.Deleted;
                _entity.DeletedDate = DateTime.Now;
            }
            else
            {
                _entity.Deleted = _entityOld.Deleted;
            }
            await _repository.Update(_entity, withChildren);
            await _repository.SaveChangeAsync();
            AfterUpdate(request, _entityOld, _entity);
            return await Task.FromResult(MappingExtensions.MapTo<Entity, ResDto>(_entity));
        }

        public List<ResDto> Update(List<ReqDto> request, bool withChildren = false)
        {
            _entities = new List<Entity>();
            MappingExtensions.SimpleMap<List<ReqDto>, List<Entity>>(request, _entities);
            var check = BeforUpdate(request, _entities);
            if (check.Status == StatusCode.Fail || check.Status == StatusCode.Waring)
            {
                throw new DomainExceptionBase(check.ListCode);
            }

            foreach (var item in _entities)
            {
                _entityOld = _repository.FindOriginalById(item.Id);
                _entity = _repository.TableNoTracking.First(x => x.Id == item.Id);
                if (_entity == null)
                {
                    throw new DomainExceptionBase($"{typeof(Entity).Name} not found object !");
                }
                _entity.CreatedBy = _entityOld.CreatedBy;
                _entity.CreatedDate = _entityOld.CreatedDate;
                _entity.Deleted = _entityOld.Deleted;
                //_entities.Add(_entity);
            }
            _repository.Update(_entities, withChildren);
            _repository.SaveChange();
            AfterUpdate(request, _entities);
            return MappingExtensions.MapTo<List<Entity>, List<ResDto>>(_entities);
        }

        public async Task<List<ResDto>> UpdateAsync(List<ReqDto> request, bool withChildren = false)
        {
            _entities = new List<Entity>();
            MappingExtensions.SimpleMap<List<ReqDto>, List<Entity>>(request, _entities);
            foreach (var item in _entities)
            {
                _entityOld = _repository.FindOriginalById(item.Id);
                _entity = _repository.TableNoTracking.First(x => x.Id == item.Id);
                if (_entity == null)
                {
                    throw new Exception($"{typeof(Entity).Name} not found object !");
                }
                item.CreatedBy = _entityOld.CreatedBy;
                item.CreatedDate = _entityOld.CreatedDate;
                item.Deleted = _entityOld.Deleted;
            }
            await _repository.Update(_entities, withChildren);
            await _repository.SaveChangeAsync();
            return await Task.FromResult(MappingExtensions.MapTo<List<Entity>, List<ResDto>>(_entities));
        }

        public ResDto UpdateStatus(long id, bool withChildren = false)
        {
            _entityOld = _repository.FindOriginalById(id);
            _entity = _repository.FindById(id);
            //MappingExtensions.SimpleMap<ReqDto, Entity>(request, _entity);
            _entity.CreatedBy = _entityOld.CreatedBy;
            _entity.CreatedDate = _entityOld.CreatedDate;
            _entity.Deleted = YesNo.YES;
            _entity.DeletedDate = DateTime.Now;
            var checkCreate = BeforUpdateStatus(_entity);
            if (checkCreate.Status == StatusCode.Fail || checkCreate.Status == StatusCode.Waring)
            {
                //var s = JsonSerializer.Serialize(checkCreate.ListCode);
                //new DomainExceptionBase(checkCreate.ListCode);
                throw new DomainExceptionBase(checkCreate.ListCode);
            }
            _repository.Update(_entity, withChildren);
            _repository.SaveChange();
            AfterUpdateStatus(_entityOld);
            return MappingExtensions.MapTo<Entity, ResDto>(_entity);
        }

        public async Task<ResDto> UpdateStatusAsync(long id, bool withChildren = false)
        {
            _entityOld = _repository.FindOriginalById(id);
            _entity = _repository.FindById(id);
            //MappingExtensions.SimpleMap<ReqDto, Entity>(request, _entity);
            _entity.CreatedBy = _entityOld.CreatedBy;
            _entity.CreatedDate = _entityOld.CreatedDate;
            _entity.Deleted = YesNo.YES;
            var checkCreate = BeforUpdateStatus(_entity);
            if (checkCreate.Status == StatusCode.Fail || checkCreate.Status == StatusCode.Waring)
            {
                //var s = JsonSerializer.Serialize(checkCreate.ListCode);
                //new DomainExceptionBase(checkCreate.ListCode);
                throw new DomainExceptionBase(checkCreate.ListCode);
            }
            //BeforUpdate(request, _entity);
            await _repository.Update(_entity, withChildren);
            await _repository.SaveChangeAsync();
            AfterUpdateStatus(_entityOld);
            return await Task.FromResult(MappingExtensions.MapTo<Entity, ResDto>(_entity));
        }

        public List<ResDto> UpdateStatusList(List<long> id, bool withChildren = false)
        {
            _entities = new List<Entity>();
            foreach (var item in id)
            {
                _entityOld = _repository.FindOriginalById(item);
                _entity = _repository.FindById(item);

                _entity.CreatedBy = _entityOld.CreatedBy;
                _entity.CreatedDate = _entityOld.CreatedDate;
                _entity.Deleted = YesNo.YES;
                var checkCreate = BeforUpdateStatus(_entity);
                if (checkCreate.Status == StatusCode.Fail || checkCreate.Status == StatusCode.Waring)
                {
                    //var s = JsonSerializer.Serialize(checkCreate.ListCode);
                    //new DomainExceptionBase(checkCreate.ListCode);
                    throw new DomainExceptionBase(checkCreate.ListCode);
                }
                _entities.Add(_entity);
            }
            //MappingExtensions.SimpleMap<ReqDto, Entity>(request, _entity);

            _repository.Update(_entities, withChildren);
            _repository.SaveChange();
            AfterUpdateStatus(_entityOld);
            return MappingExtensions.MapTo<List<Entity>, List<ResDto>>(_entities);
        }

        public async Task<List<ResDto>> UpdateStatusListAsync(List<long> id, bool withChildren = false)
        {
            _entities = new List<Entity>();
            foreach (var item in id)
            {
                _entityOld = _repository.FindOriginalById(item);
                _entity = _repository.FindById(item);

                _entity.CreatedBy = _entityOld.CreatedBy;
                _entity.CreatedDate = _entityOld.CreatedDate;
                _entity.Deleted = YesNo.YES;
                var checkCreate = BeforUpdateStatus(_entity);
                if (checkCreate.Status == StatusCode.Fail || checkCreate.Status == StatusCode.Waring)
                {
                    //var s = JsonSerializer.Serialize(checkCreate.ListCode);
                    //new DomainExceptionBase(checkCreate.ListCode);
                    throw new DomainExceptionBase(checkCreate.ListCode);
                }
                _entities.Add(_entity);
            }
            //MappingExtensions.SimpleMap<ReqDto, Entity>(request, _entity);

            await _repository.Update(_entities, withChildren);
            await _repository.SaveChangeAsync();
            AfterUpdateStatus(_entityOld);
            return await Task.FromResult(MappingExtensions.MapTo<List<Entity>, List<ResDto>>(_entities));
        }

        public long Delete(long id)
        {
            _entity = _repository.FindById(id);
            if (_entity == null)
            {
                throw new Exception($"{typeof(Entity).Name} not found object !");
            }
            BeforDelete(_entity);
            _repository.Delete(_entity);

            return _repository.SaveChange();
        }

        public Task<bool> DeleteAsync(long id)
        {
            _entity = _repository.FindById(id);
            if (_entity == null)
            {
                throw new Exception($"{typeof(Entity).Name} not found object !");
            }
            BeforDelete(_entity);
            _repository.Delete(_entity);
            _repository.SaveChangeAsync();
            AfterDelete(_entity);
            return Task.FromResult(true);
        }

        public long Delete(List<long> ids)
        {
            if (ids.Count > 0)
            {
                BeforDelete(ids);
                _entities = new List<Entity>();
                foreach (var id in ids)
                {
                    _entity = _repository.FindById(id);
                    _entities.Add(_entity);
                }
                if (_entities.Count == 0)
                {
                    throw new Exception($"{typeof(Entity).Name} not found object !");
                }
                _repository.Delete(_entities);

                return _repository.SaveChange();
            }
            return 0;
        }

        public Task<bool> DeleteAsync(List<long> ids)
        {
            try
            {
                if (ids.Count > 0)
                {
                    _entities = new List<Entity>();
                    foreach (var id in ids)
                    {
                        _entity = _repository.FindById(id);
                        _entities.Add(_entity);
                    }
                    if (_entities.Count == 0)
                    {
                        throw new Exception($"{typeof(Entity).Name} not found object !");
                    }
                    _repository.Delete(_entities);
                    _repository.SaveChangeAsync();
                    return Task.FromResult(true);
                }
                return Task.FromResult(false);
            }
            catch (Exception ex)
            {
                throw new DomainExceptionBase(ex.Message);
            }
        }

        public List<ResDto> All()
        {
            var entitys = _repository.GetAll();
            BeforeGetAll(ref entitys);
            var dto = MappingExtensions.MapTo<List<Entity>, List<ResDto>>(entitys);
            AfterGetAll(ref dto);
            return dto;
        }

        public Object getNextGenCode(string headCode, long numberFormat)
        {
            long nextId = _repository.FindLastId() + 1;
            string nextCode = headCode + nextId.ToString("D" + numberFormat.ToString());
            string data = "{\"ID\" : " + nextId + ", \"CODE\" : \"" + nextCode + "\"}";
            return data;
            //return new { ID = nextId, CODE = nextCode };
        }

        public virtual void BeforeGetById(ResDto dto)
        {

        }

        public virtual void BeforDelete(Entity entity)
        {

        }

        public virtual void BeforDelete(List<long> ids)
        {
            foreach (int id in ids)
            {
                _entity = _repository.FindById(id);
                BeforDelete(_entity);
            }
        }

        public virtual BaseStatusDto BeforCreate(ReqDto dto, Entity entity)
        {
            return new BaseStatusDto(StatusCode.Success, new List<ErrorDto>() { });
        }

        public virtual void AfterCreate(ReqDto dto, Entity entity)
        {

        }

        public virtual BaseStatusDto BeforCreate(List<ReqDto> dto, List<Entity> entity)
        {
            return new BaseStatusDto(StatusCode.Success, new List<ErrorDto>());
        }

        public virtual BaseStatusDto BeforUpdate(ReqDto dto, Entity entity)
        {
            return new BaseStatusDto(StatusCode.Success, new List<ErrorDto>());
        }

        public virtual BaseStatusDto BeforUpdate(List<ReqDto> dto, List<Entity> entity)
        {
            return new BaseStatusDto(StatusCode.Success, new List<ErrorDto>());
        }

        public virtual void AfterCreate(List<ReqDto> dto, List<Entity> entity)
        {

        }

        public virtual void AfterUpdate(List<ReqDto> dto, List<Entity> entity)
        {

        }

        public virtual void AfterUpdate(ReqDto dto, Entity entityOld, Entity entityNew)
        {

        }

        public virtual void AfterDelete(Entity entity)
        {

        }

        public virtual void BeforeGetAll(ref List<Entity> entitys)
        {

        }

        public virtual void AfterGetAll(ref List<ResDto> dto)
        {

        }

        public virtual BaseStatusDto BeforUpdateStatus(Entity entity)
        {
            return new BaseStatusDto(StatusCode.Success, new List<ErrorDto>());
        }

        public virtual void AfterUpdateStatus(Entity entity)
        {

        }

        public virtual void SetValueWhenChange(ReqDto dto)
        {

        }
    }

    /// <summary>
    /// Service Interface
    /// Array must have <see cref="ArrayDynamicUpdateSupport"/>
    /// </summary>
    public interface IDynamicUpdateArraySupport
    {
        /// <summary>
        /// Unlink the reference
        /// </summary>
        /// <param name="entity">Entity</param>
        /// <param name="coModelProperty">Property must be nullable.</param>
        void CleanDirtyArray(object entity, string? coModelProperty);
    }

    /// <summary>
    /// Service Interface
    /// Array must have <see cref="ArrayDynamicUpdateSupport"/>
    /// </summary>
    public interface ICodeVerifySupport
    {
        /// <summary>
        /// CodeVerify
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="code"></param>
        /// <returns></returns>
        bool? CodeVerify<T>(T entity, string code) where T : EntityBase;
    }

}
