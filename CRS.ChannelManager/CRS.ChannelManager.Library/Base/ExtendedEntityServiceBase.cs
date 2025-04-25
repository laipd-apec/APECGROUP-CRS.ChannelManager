using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using CRS.ChannelManager.Library.BaseDto;
using CRS.ChannelManager.Library.BaseEnum;
using CRS.ChannelManager.Library.BaseInterface;
using CRS.ChannelManager.Library.Mapper;
using CRS.ChannelManager.Library.Utils;
using System.Collections;
using System.Globalization;
using System.Linq.Expressions;
using System.Reflection;
using CRS.ChannelManager.Library.Helpper;
using System.Text.Json;
using System.ComponentModel.DataAnnotations;
using Elasticsearch.Net;
using RestSharp.Extensions;
using System.ComponentModel.DataAnnotations.Schema;
using static CRS.ChannelManager.Library.BaseDto.UtilsDto;
using Nest;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
using MediatR;
using static CRS.ChannelManager.Library.Base.ExtendedAttribute;
using AutoMapper.Internal;
using System;
using System.Net.WebSockets;
using Microsoft.Extensions.Logging;
using System.Linq;
using CRS.ChannelManager.Library.Constants;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace CRS.ChannelManager.Library.Base
{
    public abstract class ExtendedEntityServiceBase<Entity, ReqDto, ResDto, Filter> :
        EntityServiceBaseAsync<Entity, ReqDto, ResDto>, IExtendEntityServiceAsync<Entity, ReqDto, ResDto, Filter>
        where Entity : EntityBase where ReqDto : RequestBaseDto where ResDto : ResponseBaseDto where Filter : FilterDtoBase
    {
        private readonly ILogger? _logger = null;

        public EventHandler<EntityBase> OnMissingItemEvent;

        public ExtendedEntityServiceBase(IAsyncRepository<Entity> repository, ILogger? logger = null) : base(repository, logger)
        {
            _logger = logger;
        }

        public bool Activate(int id, bool isActivate)
        {
            var data = this._repository.FindById(id);
            if (data != null && data is IStatusSupportEntity entity)
            {
                entity.Status = isActivate ? "A" : "I";
                _repository.Update(data);
                _repository.SaveChange();
                return true;
            }
            return false;
        }

        public PagedResultBaseDto<List<ResDto>> Search(SearchBaseDto<Filter> search)
        {
            var query = _repository.TableNoTracking;
            if (search != null && search.Filter != null)
            {
                query = ExtendSearchQuery(query, search.Filter);
            }
            if (search != null && search.Filter != null && search.Filter.FilterGroup != null && search.Filter.FilterGroup.Any())
            {
                query = FilterBase.FilterService.GetEntities(query, search.Filter.FilterGroup);
            }
            if (search != null && search.Filter != null && search.Filter.MoreFilterGroup != null)
            {
                query = FilterBase.FilterService.GetEntities(query, search.Filter.MoreFilterGroup);
            }
            if (search != null && search.Filter != null && search.Filter.Sort != null && !string.IsNullOrEmpty(search.Filter.Sort))
            {
                query = SortByField(query, search.Filter.Sort);
            }
            else
            {
                query = query.OrderByDescending(t => t.CreatedDate);
            }
            if (search != null && search.Filter != null && !string.IsNullOrEmpty(search.Filter.TextSearch))
            {
                query = ExecuteQueryFilter(query, search.Filter.TextSearch);
            }
            if (this._repository is RepositoryBase<Entity> baseRepo)
            {
                query = (IQueryable<Entity>)baseRepo.ExtendQuery(query);
            }
            //if (objectList != null)
            //{
            //    query=query.Where(t => t.Contains())
            //}
            var rowCount = query.Count();
            if (search != null && search.Pagination == null || (search != null && search.Pagination != null && search.Pagination.IsAll))
            {
                search.Pagination = new PageSearchDto()
                {
                    PageIndex = 1,
                    PageSize = rowCount,
                    IsAll = true,
                };
            }
            if (search != null && search.Pagination != null)
            {
                var skip = (search.Pagination.PageIndex - 1) * search.Pagination.PageSize;
                query = query.Skip(skip).Take(search.Pagination.PageSize);
            }
            PagedResultBaseDto<List<ResDto>> result = new PagedResultBaseDto<List<ResDto>>();
            if (query.Count() == 0)
            {
                result.Result = new List<ResDto>();
                result.Pagination = new PaginationDto(search.Pagination.PageIndex, search.Pagination.PageSize, rowCount);
                return result;
            }
            //var entities = query.ToList();
            var entities = query.AsSplitQuery().ToList();
            result.Result = MappingExtensions.MapTo<List<Entity>, List<ResDto>>(entities);
            if (search != null && search.Pagination != null)
            {
                result.Pagination = new PaginationDto(search.Pagination.PageIndex, search.Pagination.PageSize, rowCount);
            }
            AfterSearch(result);
            return result;
        }

        //public static IQueryable<Entity> AdvanceSearchQuery(IQueryable<Entity> query, Filter filter)
        //{
        //    var parser = new JsonExpressionParser();
        //    JsonDocument jsonDocument = JsonDocument.Parse(filter.AdvanceSearch);
        //    var predicate = parser.ParsePredicateOf<Entity>(jsonDocument);
        //    return query.Where(predicate).AsQueryable();
        //}

        protected virtual IQueryable<Entity> ExtendSearchQuery(IQueryable<Entity> query, Filter filter) => query;

        public static IQueryable<Entity> QuerySearchLikeAny(IQueryable<Entity> query, string text, List<PropertyInfo> properties)
        {
            var parameter = Expression.Parameter(typeof(Entity));

            var body = properties.Select(property => Expression.Call(
                Expression.Call(
                Expression.PropertyOrField(parameter, property.Name),
                "ToUpper", null),
                "Contains", null,
                Expression.Constant(text.ToUpper()))).Aggregate<MethodCallExpression, Expression>(null, (current, call) => current != null ? Expression.OrElse(current, call) : (Expression)call);

            return query.Where(Expression.Lambda<Func<Entity, bool>>(body, parameter));
        }

        protected static IQueryable<T> SortByField<T>(IQueryable<T> source, string sortText)
        {
            try
            {
                var type = typeof(T);
                object objSort = JsonConvert.DeserializeObject(sortText);
                JObject jObject = JObject.Parse(sortText);
                string propName = string.Empty;
                string prodValue = string.Empty;
                bool isThenSort = false;
                string method = SortMethod.OrderBy.ToEnumMemberString();
                IOrderedQueryable<T> ordered = source as IOrderedQueryable<T>;
                if (ordered != null && ordered.ToQueryString().Contains("Order", StringComparison.OrdinalIgnoreCase))
                {
                    isThenSort = true;
                }
                if (jObject.Children().Any())
                {
                    foreach (var item in jObject.Children())
                    {
                        Type typeItem = item.GetType();
                        PropertyInfo[] propsItem = typeItem.GetProperties();
                        if (propsItem.Where(t => t.Name.ToLower().Equals("name")).Any())
                        {
                            propName = propsItem.Where(t => t.Name.ToLower().Equals("name")).FirstOrDefault().GetValue(item).ToString().ToUpper();
                        }
                        if (propsItem.Where(t => t.Name.ToLower().Equals("value")).Any())
                        {
                            prodValue = propsItem.Where(t => t.Name.ToLower().Equals("value")).FirstOrDefault().GetValue(item).ToString();
                        }
                        if (!string.IsNullOrEmpty(propName))
                        {
                            var parameter = Expression.Parameter(type, "p");
                            string objectName = string.Empty;
                            string fieldName = string.Empty;
                            var property = type.GetProperties().Where(t => t.Name.Equals(propName, StringComparison.OrdinalIgnoreCase)).FirstOrDefault();
                            if (propName.IndexOf(".") > 0)
                            {
                                objectName = propName.Split(".")[0];
                                fieldName = propName.Split(".")[1];
                                property = type.GetProperties().Where(t => t.Name.Equals(objectName, StringComparison.OrdinalIgnoreCase)).FirstOrDefault();
                            }
                            if (property != null)
                            {
                                //if (isThenSort)
                                //{
                                //    method = !string.IsNullOrEmpty(prodValue) && prodValue.Contains(SortValue.DESC.ToEnumMemberString(), StringComparison.OrdinalIgnoreCase) ? SortMethod.ThenByDescending.ToEnumMemberString() : SortMethod.ThenBy.ToEnumMemberString();
                                //}
                                //else
                                //{
                                //    method = !string.IsNullOrEmpty(prodValue) && prodValue.Contains(SortValue.DESC.ToEnumMemberString(), StringComparison.OrdinalIgnoreCase) ? SortMethod.OrderByDescending.ToEnumMemberString() : SortMethod.OrderBy.ToEnumMemberString();
                                //}
                                method = !string.IsNullOrEmpty(prodValue) && prodValue.Contains(SortValue.DESC.ToEnumMemberString(), StringComparison.OrdinalIgnoreCase) ? SortMethod.OrderByDescending.ToEnumMemberString() : SortMethod.OrderBy.ToEnumMemberString();

                                var propertyAccess = Expression.MakeMemberAccess(parameter, property);
                                var argument = Expression.Lambda(propertyAccess, parameter);
                                var typeArr = new[] { type, property.PropertyType };
                                Expression[] argumentArr = new[] { argument };
                                if (!string.IsNullOrEmpty(objectName) && !string.IsNullOrEmpty(fieldName))
                                {
                                    var child = Expression.Property(parameter, objectName);
                                    var propertyChild = property.GetType().GetProperties().Where(t => t.Name.Equals(fieldName, StringComparison.OrdinalIgnoreCase)).FirstOrDefault();
                                    //var propertyAccessChild = Expression.MakeMemberAccess(propertyAccess, propertyChild);
                                    propertyAccess = Expression.Property(child, fieldName);
                                    var argumentChild = Expression.Lambda(propertyAccess, parameter);
                                    argumentArr.Append(argumentChild);
                                    typeArr.Append(propertyChild.PropertyType);
                                }
                                var resultExp = Expression.Call(typeof(Queryable), method, typeArr, source.Expression, argument);
                                isThenSort = true;
                                source = source.Provider.CreateQuery<T>(resultExp);
                            }
                        }
                    }
                }
                return source;
            }
            catch (Exception ex)
            {
                return source;
            }
        }

        protected static IQueryable<T> QuerySearchField<T>(IQueryable<T> source, string searchText)
        {
            try
            {
                var type = typeof(T);
                object objSort = JsonConvert.DeserializeObject(searchText);
                JObject jObject = JObject.Parse(searchText);
                string propName = string.Empty;
                string prodValue = string.Empty;
                string fieldName = string.Empty;
                string fieldChildName = string.Empty;
                string method = SortMethod.OrderBy.ToEnumMemberString();
                var parameter = Expression.Parameter(typeof(Entity));

                if (jObject.Children().Any())
                {
                    foreach (var item in jObject.Children())
                    {
                        Type typeItem = item.GetType();
                        PropertyInfo[] propsItem = typeItem.GetProperties();
                        if (propsItem != null)
                        {
                            if (propsItem != null && propsItem.Where(t => t.Name.ToLower().Equals("name")).Any())
                            {
                                propName = propsItem.Where(t => t.Name.ToLower().Equals("name")).FirstOrDefault().GetValue(item).ToString();
                            }
                            var checkPropName = propName.Split(".");
                            if (checkPropName.Any() && checkPropName.Length > 1)
                            {
                                if (checkPropName.Length == 2)
                                {
                                    propName = checkPropName[0];
                                    fieldName = checkPropName[1];
                                }
                                else if (checkPropName.Length == 3)
                                {
                                    propName = checkPropName[0];
                                    fieldName = checkPropName[1];
                                    fieldChildName = checkPropName[2];
                                }
                            }
                            var property = type.GetProperties().Where(t => t.Name.Equals(propName, StringComparison.OrdinalIgnoreCase)).FirstOrDefault();

                            if (property != null)
                            {
                                if (propsItem.Where(t => t.Name.ToLower().Equals("value")).Any())
                                {
                                    prodValue = propsItem.Where(t => t.Name.ToLower().Equals("value")).FirstOrDefault().GetValue(item).ToString();
                                }
                                if (!string.IsNullOrEmpty(propName) && !string.IsNullOrEmpty(prodValue))
                                {
                                    var arg = Expression.Parameter(typeof(T), "p");
                                    if (property.PropertyType.Name.Contains(nameof(ICollection)) || property.Name.Contains(nameof(IList)))
                                    {
                                        if (property.DeclaringType != null)
                                        {
                                            var propertyChild = property.DeclaringType.GetProperties().Where(t => t.Name.Equals(fieldName, StringComparison.OrdinalIgnoreCase)).FirstOrDefault();
                                            if (propertyChild != null)
                                            {
                                                var propertyChildChild = propertyChild.PropertyType.GetProperties().Where(t => t.Name.Equals(fieldChildName, StringComparison.OrdinalIgnoreCase)).FirstOrDefault();
                                                if (propertyChildChild != null)
                                                {
                                                    if (propertyChildChild.PropertyType == typeof(DateTime))
                                                    {
                                                        string[] dateFormats = new[] { "yyyy-MM-dd", "MM-dd-yyyy", "MM-dd-yyyyHH:mm:ss", "yyyy-MM-ddHH:mm:ss" };
                                                        CultureInfo provider = new CultureInfo("en-US");
                                                        DateTime date = DateTime.ParseExact(prodValue, dateFormats, provider,
                                                        DateTimeStyles.AdjustToUniversal);
                                                        DateTime dateApter = date.AddDays(-1);
                                                        DateTime dateBefor = date.AddDays(1);
                                                        var body = Expression.And(
                                                                                     Expression.LessThanOrEqual(Expression.PropertyOrField(arg, property.Name), Expression.Constant(dateBefor)),
                                                                                     Expression.GreaterThanOrEqual(Expression.PropertyOrField(arg, property.Name), Expression.Constant(dateApter))
                                                                                 );
                                                        var predicate = Expression.Lambda<Func<T, bool>>(body, arg);
                                                        source = source.Where(predicate);
                                                    }
                                                    else
                                                    {
                                                        var checkType = FilterType.Any.ToEnumMemberString();
                                                        string methodValue = FilterType.Contains.ToEnumMemberString();

                                                        var parameterChild = Expression.Parameter(propertyChild.PropertyType);
                                                        var argChild = Expression.Parameter(propertyChild.PropertyType, "o");
                                                        var child = Expression.Property(parameter, propName);
                                                        var childChild = Expression.Property(parameterChild, propertyChildChild.Name);
                                                        var bodyChild = Expression.Call(Expression.Property(argChild, propertyChildChild.Name), methodValue, null, Expression.Constant(prodValue.ToUpper()));
                                                        //var callChildChild = Expression.Call(Expression.Property(parameterChild, propertyChildChild.Name),);

                                                        Expression body;
                                                        if (propertyChild.PropertyType == typeof(bool))
                                                        {
                                                            MethodInfo methodNew = typeof(string).GetMethod(methodValue, new[] { typeof(string) });
                                                            MethodCallExpression result = Expression.Call(childChild, methodNew, Expression.Constant(Boolean.Parse(prodValue)));
                                                            methodValue = FilterType.Equals.ToEnumMemberString();
                                                            body = Expression.Call(Expression.Property(arg, property.Name), methodValue, null, Expression.Constant(Boolean.Parse(prodValue)));
                                                        }
                                                        else
                                                        {
                                                            MethodInfo methodNew = typeof(string).GetMethod(checkType, new[] { typeof(string) });
                                                            var field = Expression.Call(Expression.Property(child, fieldName), methodNew, childChild);
                                                            MethodCallExpression result = Expression.Call(childChild, methodNew, Expression.Constant(Boolean.Parse(prodValue)));
                                                            body = Expression.Call(field, FilterType.Contains.ToEnumMemberString(), null, Expression.Constant(prodValue.ToUpper()));
                                                        }
                                                        var lambda = Expression.Lambda<Func<T, bool>>(body, parameter);
                                                        source = source.Where(lambda);
                                                    }
                                                }
                                                else
                                                {
                                                    Expression body;
                                                    var checkType = FilterType.Any.ToEnumMemberString();
                                                    string methodValue = FilterType.Contains.ToEnumMemberString();

                                                    var parameterChild = Expression.Parameter(propertyChild.PropertyType);
                                                    var argChild = Expression.Parameter(propertyChild.PropertyType, "o");
                                                    var child = Expression.Property(parameter, propName);
                                                    MethodInfo methodNew = typeof(string).GetMethod(checkType, new[] { typeof(string) });
                                                    var field = Expression.Call(Expression.Property(child, fieldName), methodNew, child);
                                                    body = Expression.Call(field, FilterType.Contains.ToEnumMemberString(), null, Expression.Constant(prodValue.ToUpper()));
                                                    var lambda = Expression.Lambda<Func<T, bool>>(body, parameter);
                                                    source = source.Where(lambda);
                                                }
                                            }
                                        }
                                    }
                                    else if (property.PropertyType.Name.ToUpper() == (propName + "Entity").ToUpper())
                                    {
                                        var propertyChild = property.PropertyType.GetProperties().Where(t => t.Name.Equals(fieldName, StringComparison.OrdinalIgnoreCase)).FirstOrDefault();
                                        if (propertyChild != null)
                                        {
                                            if (propertyChild.PropertyType == typeof(DateTime))
                                            {
                                                string[] dateFormats = new[] { "yyyy-MM-dd", "MM-dd-yyyy", "MM-dd-yyyyHH:mm:ss", "yyyy-MM-ddHH:mm:ss" };
                                                CultureInfo provider = new CultureInfo("en-US");
                                                DateTime date = DateTime.ParseExact(prodValue, dateFormats, provider,
                                                DateTimeStyles.AdjustToUniversal);
                                                DateTime dateApter = date.AddDays(-1);
                                                DateTime dateBefor = date.AddDays(1);
                                                var body = Expression.And(
                                                                             Expression.LessThanOrEqual(Expression.PropertyOrField(arg, property.Name), Expression.Constant(dateBefor)),
                                                                             Expression.GreaterThanOrEqual(Expression.PropertyOrField(arg, property.Name), Expression.Constant(dateApter))
                                                                         );
                                                var predicate = Expression.Lambda<Func<T, bool>>(body, arg);
                                                source = source.Where(predicate);
                                            }
                                            else
                                            {
                                                var checkType = "ToString";
                                                string methodValue = FilterType.Contains.ToEnumMemberString();
                                                if (propertyChild.PropertyType == typeof(string))
                                                {
                                                    checkType = "ToUpper";

                                                }
                                                else if (propertyChild.PropertyType == typeof(bool))
                                                {
                                                    checkType = "";
                                                }
                                                var child = Expression.Property(parameter, propName);
                                                var field = Expression.Call(Expression.Property(child, fieldName), checkType, null);
                                                Expression body;
                                                if (propertyChild.PropertyType == typeof(bool))
                                                {
                                                    methodValue = FilterType.Equals.ToEnumMemberString();
                                                    body = Expression.Call(Expression.Property(arg, property.Name), methodValue, null, Expression.Constant(Boolean.Parse(prodValue)));
                                                }
                                                else
                                                {
                                                    body = Expression.Call(field, FilterType.Contains.ToEnumMemberString(), null, Expression.Constant(prodValue.ToUpper()));
                                                }
                                                var lambda = Expression.Lambda<Func<T, bool>>(body, parameter);
                                                source = source.Where(lambda);
                                            }
                                        }
                                    }
                                    else
                                    {
                                        if (property.PropertyType == typeof(DateTime))
                                        {
                                            string[] dateFormats = new[] { "yyyy-MM-dd", "MM-dd-yyyy", "MM-dd-yyyyHH:mm:ss", "yyyy-MM-ddHH:mm:ss" };
                                            CultureInfo provider = new CultureInfo("en-US");
                                            DateTime date = DateTime.ParseExact(prodValue, dateFormats, provider,
                                            DateTimeStyles.AdjustToUniversal);
                                            DateTime dateApter = date.AddDays(-1);
                                            DateTime dateBefor = date.AddDays(1);
                                            var body = Expression.And(
                                                                         Expression.LessThanOrEqual(Expression.PropertyOrField(arg, property.Name), Expression.Constant(dateBefor)),
                                                                         Expression.GreaterThanOrEqual(Expression.PropertyOrField(arg, property.Name), Expression.Constant(dateApter))
                                                                     );
                                            var predicate = Expression.Lambda<Func<T, bool>>(body, arg);
                                            source = source.Where(predicate);
                                        }
                                        else
                                        {
                                            var checkType = "ToString";
                                            string methodValue = FilterType.Contains.ToEnumMemberString();
                                            if (property.PropertyType == typeof(string))
                                            {
                                                checkType = "ToUpper";

                                            }
                                            Expression body;
                                            if (property.PropertyType == typeof(bool))
                                            {
                                                methodValue = FilterType.Equals.ToEnumMemberString();
                                                body = Expression.Call(Expression.Property(arg, property.Name), methodValue, null, Expression.Constant(Boolean.Parse(prodValue)));
                                            }
                                            else
                                            {
                                                body = Expression.Call(Expression.Call(Expression.Property(arg, property.Name), checkType, null),
                                                                     methodValue, null, Expression.Constant(prodValue.ToUpper()));
                                            }

                                            var predicate = Expression.Lambda<Func<T, bool>>(body, arg);
                                            source = source.Where(predicate);

                                        }
                                    }
                                }
                            }
                        }
                    }
                }
                return source;
            }
            catch (Exception ex)
            {
                return source;
            }
        }

        protected static IQueryable<T> ExecuteQueryFilter<T>(IQueryable<T> queryable, string query)
        {
            // If the incoming request is empty, skip the search
            if (string.IsNullOrEmpty(query))
            {
                return queryable;
            }

            // We get all properties with type of string marked with our attribute
            var properties = typeof(T).GetProperties()
                .Where(p => p.PropertyType == typeof(string) &&
                            p.GetCustomAttributes(typeof(SearchableAttribute), true).FirstOrDefault() != null)
                .Select(x => x.Name).ToList();

            // If there are no such properties, skip the search
            if (!properties.Any())
            {
                return queryable;
            }

            // Get our generic object
            ParameterExpression entity = Expression.Parameter(typeof(T), "entity");

            // Get the Like Method from EF.Functions
            var efLikeMethod = typeof(DbFunctionsExtensions).GetMethod("Like",
                BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic,
                null,
                new[] { typeof(DbFunctions), typeof(string), typeof(string) },
                null);

            // We make a pattern for the search
            var pattern = Expression.Constant($"%{query}%", typeof(string));

            // Here we will collect a single search request for all properties
            Expression body = Expression.Constant(false);

            foreach (var propertyName in properties)
            {
                // Get property from our object
                var property = Expression.Property(entity, propertyName);

                // Сall the method with all the required arguments
                Expression expr = Expression.Call(efLikeMethod, Expression.Property(null, typeof(EF), nameof(EF.Functions)), property, pattern);

                // Add to the main request
                body = Expression.OrElse(body, expr);
            }

            // Compose and pass the expression to Where
            var expression = Expression.Lambda<Func<T, bool>>(body, entity);
            return queryable.Where(expression);
        }

        private static Func<T, bool> CreateFilter<T>(string propName, FilterType filterType, object val)
        {
            var item = Expression.Parameter(typeof(T), "x");
            var propEx = Expression.Property(item, propName);
            var valEx = Expression.Constant(val);

            Expression compareEx = null;

            switch (filterType)
            {
                case FilterType.LessOrEquals:
                    compareEx = Expression.LessThanOrEqual(propEx, valEx);
                    break;

                case FilterType.Less:
                    compareEx = Expression.LessThan(propEx, valEx);
                    break;

                case FilterType.Equals:
                    compareEx = Expression.Equal(propEx, valEx);
                    break;

                case FilterType.Greater:
                    compareEx = Expression.GreaterThan(propEx, valEx);
                    break;

                case FilterType.GreaterOrEquals:
                    compareEx = Expression.GreaterThanOrEqual(propEx, valEx);
                    break;

                default:
                    throw new Exception($"Unknown FilterType '{filterType}' on property '{propName}'!");
            }

            var lambda = Expression.Lambda<Func<T, bool>>(compareEx, item);

            Func<T, bool> filter = lambda.Compile();

            return filter;
        }

        public virtual void AfterSearch(PagedResultBaseDto<List<ResDto>> pagedResultDto) { }

        public virtual void BeforeGetAll(ref List<Entity> entitys) { }

        public override BaseStatusDto BeforCreate(ReqDto dto, Entity entity)
        {
            RefineRequestDto(dto, true);
            MappingExtensions.SimpleMap<ReqDto, Entity>(dto, _entity);
            return ValidateEntity(entity) ?? base.BeforCreate(dto, entity);
        }

        public override BaseStatusDto BeforUpdate(ReqDto dto, Entity entity)
        {
            RefineRequestDto(dto);
            MappingExtensions.SimpleMap<ReqDto, Entity>(dto, _entity);
            return ValidateEntity(entity, true) ?? base.BeforUpdate(dto, entity);
        }

        public override void AfterUpdate(ReqDto dto, Entity entity, Entity entityNew)
        {
            var dynamicUpdateArraySupport = this as IDynamicUpdateArraySupport;
            var enumerableEntityProperties = (entity.GetType().GetProperties().Where(t => t.PropertyType.GetInterfaces().Contains(typeof(IEnumerable))
            && t.PropertyType != typeof(string))).ToList();
            for (int i = 0; i < enumerableEntityProperties.Count; i++)
            {
                try
                {
                    var enumerableEntityProperty = enumerableEntityProperties[i];
                    IEnumerable entities = (IEnumerable)CRS.ChannelManager.Library.Helpper.CommonHelper.GetPropertyValue(entity, enumerableEntityProperty.Name);
                    IEnumerable beforEntities = (IEnumerable)CRS.ChannelManager.Library.Helpper.CommonHelper.GetPropertyValue(_entity, enumerableEntityProperty.Name);
                    if (entities != null)
                    {
                        foreach (EntityBase item in entities)
                        {
                            bool exist = false;
                            foreach (EntityBase beforItem in beforEntities)
                            {
                                if (item.Id.Equals(beforItem.Id))
                                {
                                    exist = true; break;
                                }
                            }
                            if (!exist)
                            {
                                ArrayDynamicUpdateSupport attribute = null;
                                if (dynamicUpdateArraySupport != null &&
                                    (attribute = enumerableEntityProperty.GetCustomAttribute<ArrayDynamicUpdateSupport>()) != null)
                                {
                                    dynamicUpdateArraySupport.CleanDirtyArray(item, attribute.PropertyName);
                                }
                                else
                                {
                                    this.OnMissingItemEvent?.Invoke(entity, item);
                                }
                            }
                        }
                    }
                }
                catch (Exception)
                {

                    continue;
                }
            }
            MappingExtensions.SimpleMap<ReqDto, Entity>(dto, _entity);
            base.AfterUpdate(dto, entity, entityNew);
        }

        private void RefineRequestDto(ReqDto dto, bool isCreate = false)
        {
            foreach (PropertyInfo propertyInfo in dto.GetType().GetProperties())
            {
                if (propertyInfo.GetAttribute<UpperCaseData>() != null)
                {
                    var intputText = propertyInfo.GetValue(dto)?.ToString();
                    propertyInfo.SetValue(dto, intputText?.ToUpper());
                }
                else if (propertyInfo.GetAttribute<UniqueFormatValidate>() != null)
                {
                    string format = propertyInfo?.GetAttribute<UniqueFormatValidate>()?.Format ?? string.Empty;
                    GenerateUniqueFormat(dto, propertyInfo, format, isCreate);
                }
            }
            SetValueWhenChange(dto);
        }

        public override void SetValueWhenChange(ReqDto dto)
        {

        }

        private BaseStatusDto ValidateEntity(Entity inputEntity, bool isUpdate = false)
        {
            BaseStatusDto res;
            var validationResults = new List<ValidationResult>();
            try
            {
                if (!Validator.TryValidateObject(inputEntity, new ValidationContext(inputEntity), validationResults, true))
                {
                    var ignorePropertiesList = inputEntity.GetType().GetProperties().Where(p => p.GetCustomAttribute<IgnoreValidate>() != null).ToList();

                    foreach (var validationResult in validationResults)
                    {
                        foreach (var item in validationResult.MemberNames)
                        {
                            //HARD CODE FIX LATER
                            if (item.Equals("CreatedBy"))
                            {
                                validationResults.Remove(validationResult);
                                goto END_CHECK;
                            }
                        }
                    }

                END_CHECK:;
                    if (validationResults.Count > 0)
                    {
                        List<ErrorDto> erros = new List<ErrorDto>();
                        foreach (var item in validationResults)
                        {
                            erros.Add(new ErrorDto((int)BaseEnum.ErrorCode.INVALID, item.ErrorMessage));
                        }
                        return new BaseStatusDto
                        {
                            Status = StatusCode.Fail,
                            ListCode = erros
                        };
                    }
                }
            }
            catch (Exception ex)
            {
                throw new DomainExceptionBase(ex.Message);
            }

            foreach (PropertyInfo propertyInfo in inputEntity.GetType().GetProperties())
            {
                if (propertyInfo.GetAttribute<UniqueValidate>() != null)
                {
                    res = ValidateUnique(inputEntity, propertyInfo, isUpdate);
                    if (res != null)
                    {
                        return res;
                    }
                }
                else if (propertyInfo.GetAttribute<UniqueFormatValidate>() != null)
                {
                    string format = propertyInfo.GetAttribute<UniqueFormatValidate>()?.Format ?? string.Empty;
                    //ValidateUniqueFormat(inputEntity, propertyInfo, format, isUpdate);
                }
            }
            return null;
        }

        private BaseStatusDto ValidateUnique(Entity inputEntity, PropertyInfo propertyInfo, bool isUpdate = false)
        {
            var query = _repository.TableNoTracking;
            object input = inputEntity.GetType().GetProperty(propertyInfo.Name).GetValue(inputEntity);
            AdvanceSearch.Rule rule = new AdvanceSearch.Rule() { @operator = JsonExpressionParser.Equal, type = JsonExpressionParser.StringStr, field = propertyInfo.Name, value = input.ToString() };
            var _rules = new List<AdvanceSearch.Rule> { rule };
            if (isUpdate)
            {
                _rules.Add(new AdvanceSearch.Rule() { @operator = JsonExpressionParser.NotEqual, type = JsonExpressionParser.Int32, field = nameof(inputEntity.Id), value = inputEntity.Id });
            }

            AdvanceSearch advanceSearch = new AdvanceSearch() { condition = _rules.Count > 1 ? JsonExpressionParser.And : string.Empty, rules = _rules };
            string filter = JsonConvert.SerializeObject(advanceSearch);
            var parser = new JsonExpressionParser();
            JsonDocument jsonDocument = JsonDocument.Parse(filter);
            var predicate = parser.ParsePredicateOf<Entity>(jsonDocument);
            bool isExists = query.Where(predicate).AsQueryable().Count() != 0;

            if (isExists)
            {
                return new BaseStatusDto
                {
                    Status = StatusCode.Fail,
                    ListCode = new List<ErrorDto> { new ErrorDto((int)BaseEnum.ErrorCode.DUPLICATE_RECORD, $"The {propertyInfo.Name} inputted already exists.") }
                };
            }
            return null;
        }

        private void GenerateUniqueFormat(ReqDto dto, PropertyInfo propertyInfo, string format, bool isCreate = true)
        {
            var query = _repository.Table;
            long seq = 1;
            string value = format;
            if (isCreate)
            {
                if (query.Count() > 0)
                {
                    seq = query.Count() + 1;
                }
                DateTime dtNow = DateTime.Now;
                var arrFormat = value.Split('+');
                List<string> values = new List<string>();
                for (int i = 0; i < arrFormat.Length; i++)
                {
                    if (arrFormat[i].Contains(UniqueFormatConstants.SEQ_DATE))
                    {
                        var arrSEQ = arrFormat[i].Split("_");
                        if (arrSEQ.Length > 1)
                        {
                            int numberLen = int.Parse(arrSEQ.Last());
                            var querySQE = _repository.Table.Where(x => x.CreatedDate.Date == dtNow.Date).Count();
                            if (querySQE == 0)
                            {
                                querySQE = 1;
                            }
                            else
                            {
                                querySQE++;
                            }
                            values.Add(querySQE.ToString().PadLeft(numberLen, '0'));
                        }
                        else
                        {
                            values.Add(seq.ToString());
                        }
                    }
                    else if (arrFormat[i].Contains(UniqueFormatConstants.SEQ))
                    {
                        var arrSEQ = arrFormat[i].Split("_");
                        if (arrSEQ.Length > 1)
                        {
                            int numberLen = int.Parse(arrSEQ.Last());
                            values.Add(seq.ToString().PadLeft(numberLen, '0'));
                        }
                        else
                        {
                            values.Add(seq.ToString());
                        }
                    }
                    else if (arrFormat[i].Contains(UniqueFormatConstants.PR_DATE_YYMMDD))
                    {
                        values.Add(arrFormat[i].Replace(UniqueFormatConstants.PR_DATE_YYMMDD, dtNow.ToString(UniqueFormatConstants.FM_DATE_YYMMDD)));
                    }
                    else if (arrFormat[i].Contains(UniqueFormatConstants.FM_DATE_YYYYMMDD))
                    {
                        values.Add(arrFormat[i].Replace(UniqueFormatConstants.FM_DATE_YYYYMMDD, dtNow.ToString(UniqueFormatConstants.FM_DATE_YYYYMMDD)));
                    }
                    else
                    {
                        values.Add(arrFormat[i]);
                    }
                }
                value = string.Join(string.Empty, values);
            }
            else
            {
                var findObj = query.FirstOrDefault(x => x.Id == dto.Id);
                if (findObj != null)
                {
                    var getValue = findObj.GetType()?.GetProperty(propertyInfo.Name)?.GetValue(findObj);
                    if (getValue != null) { value = getValue.ToString(); }
                }
            }
            if (!string.IsNullOrEmpty(value) && dto.GetType().GetProperty(propertyInfo.Name) != null)
            {
                dto.GetType()?.GetProperty(propertyInfo.Name)?.SetValue(dto, value);
            }
        }

        private void ValidateUniqueFormat(Entity inputEntity, PropertyInfo propertyInfo, string format, bool isUpdate = false)
        {
            var query = _repository.Table;
            string value = format;
            if (isUpdate)
            {
                var findObj = query.FirstOrDefault(x => x.Id == inputEntity.Id);
                if (findObj != null)
                {
                    var getValue = findObj.GetType()?.GetProperty(propertyInfo.Name)?.GetValue(inputEntity, null) ?? format;
                    if (getValue != null) { value = getValue.ToString(); }
                }
                inputEntity.GetType()?.GetProperty(propertyInfo.Name)?.SetValue(inputEntity, value);
            }
        }

        public async Task<Entity> FindIdByCode(Entity inputEntity, bool isUpdate = false)
        {
            var query = _repository.TableNoTracking;
            Type entityType = inputEntity.GetType();
            System.Reflection.PropertyInfo propertyInfo = entityType.GetProperty("Code");
            object input = inputEntity.GetType().GetProperty(propertyInfo.Name).GetValue(inputEntity);
            AdvanceSearch.Rule rule = new AdvanceSearch.Rule() { @operator = JsonExpressionParser.Equal, type = JsonExpressionParser.StringStr, field = propertyInfo.Name, value = input.ToString() };
            var _rules = new List<AdvanceSearch.Rule> { rule };
            AdvanceSearch advanceSearch = new AdvanceSearch() { condition = string.Empty, rules = _rules };
            string filter = JsonConvert.SerializeObject(advanceSearch);
            var parser = new JsonExpressionParser();
            JsonDocument jsonDocument = JsonDocument.Parse(filter);
            var predicate = parser.ParsePredicateOf<Entity>(jsonDocument);
            bool isExists = query.Where(predicate).AsQueryable().Count() != 0;

            if (isExists)
            {
                return query.Where(predicate).AsQueryable().FirstOrDefault();
            }
            return null;
        }

        public async Task<Object> ImportAsync(List<ReqDto> requests, bool overwrite)
        {
            Object result = new Object();
            List<ResDto> createdDatas = new List<ResDto>();
            List<ResDto> updatedDatas = new List<ResDto>();
            if (overwrite)
            {
                foreach (var request in requests)
                {
                    try
                    {
                        _entity = MappingExtensions.MapTo<ReqDto, Entity>(request);
                        if (Attribute.GetCustomAttribute(request.GetType(), typeof(ImportCheckDataViaCodeDto)) != null)
                        {
                            foreach (PropertyInfo property in request.GetType().GetProperties())
                            {
                                string id = string.Empty;
                                ImportReferedEntity importReferedEntity = property.GetAttribute<ImportReferedEntity>();
                                if (property.GetAttribute<ImportReferedEntity>() != null)
                                {
                                    string codeColumn = "CODE";
                                    foreach (PropertyInfo propertyChild in importReferedEntity.Entity.GetProperties())
                                    {
                                        if (propertyChild.GetAttribute<ImportCode>() != null)
                                        {
                                            codeColumn = propertyChild.GetAttribute<ColumnAttribute>()?.Name;
                                            break;
                                        }
                                    }
                                    var idCodePairs = _repository.GetImportCodesViaTableName(importReferedEntity.Entity.GetAttribute<TableAttribute>(), codeColumn);
                                    id = string.IsNullOrEmpty(Helpper.CommonHelper.GetPropertyValue(request, property.Name).ToString()) ? null : idCodePairs.First(p => p.Item2.Equals(Helpper.CommonHelper.GetPropertyValue(request, property.Name))).Item1;
                                    //codeColumn = propertyChild.GetAttribute<ColumnAttribute>().Name;
                                    Helpper.CommonHelper.UpdatePropertyValue(_entity, property.Name.Replace("Code", "Id"), id == null ? id : (object)int.Parse(id));
                                }
                            }
                            await _repository.AddAsync(_entity);
                            await _repository.SaveChangeAsync();
                            createdDatas.Add(MappingExtensions.MapTo<Entity, ResDto>(_entity));
                        }
                        else
                        {
                            var checkCreate = BeforCreate(request, _entity);
                            if (checkCreate.Status == StatusCode.Fail || checkCreate.Status == StatusCode.Waring)
                            {
                                var id = await FindIdByCode(_entity);
                                request.Id = id.Id;
                                await UpdateAsync(request);
                                updatedDatas.Add(MappingExtensions.MapTo<Entity, ResDto>(_entity));
                            }
                            else
                            {
                                await _repository.AddAsync(_entity);
                                await _repository.SaveChangeAsync();
                                createdDatas.Add(MappingExtensions.MapTo<Entity, ResDto>(_entity));
                            }
                        }
                    }
                    catch (Exception ex) { }

                }
            }
            else
            {
                foreach (var request in requests)
                {
                    try
                    {
                        _entity = MappingExtensions.MapTo<ReqDto, Entity>(request);
                        var checkCreate = BeforCreate(request, _entity);
                        if (!(checkCreate.Status == StatusCode.Fail || checkCreate.Status == StatusCode.Waring))
                        {
                            await _repository.AddAsync(_entity);
                            await _repository.SaveChangeAsync();
                            AfterCreate(request, _entity);
                            createdDatas.Add(MappingExtensions.MapTo<Entity, ResDto>(_entity));
                        }
                    }
                    catch (Exception ex) { }
                }
            }
            result = new { createdDatas = createdDatas, updatedDatas = updatedDatas };
            return await Task.FromResult(result);


        }

        public string CheckCodes(List<ReqDto> requests)
        {
            foreach (var request in requests)
            {
                try
                {
                    _entity = MappingExtensions.MapTo<ReqDto, Entity>(request);
                }
                catch (Exception ex) { }

            }
            return string.Empty;
        }

        private class AdvanceSearch
        {
            public string condition;
            public List<Rule> rules;
            public class Rule
            {
                public string @operator;
                public string type;
                public string field;
                public object value;
            }
        }
    }
    public interface IStatusSupportService
    {
        bool Activate(int id, bool isActivate);
    }
}
