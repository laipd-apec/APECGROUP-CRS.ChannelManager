
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper.Internal;
using CRS.ChannelManager.Library.BaseDto;
using CRS.ChannelManager.Library.BaseInterface;
using CRS.ChannelManager.Library.Helpper;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace CRS.ChannelManager.Library.Base
{
    public abstract class UnitOfWorkBase : IUnitOfWork
    {
        private readonly IDbContext _dbContext;
        private IHttpContextAccessor _httpContext;

        public UnitOfWorkBase(IDbContext dbContext, IHttpContextAccessor httpContext)
        {
            _dbContext = dbContext;
            _httpContext = httpContext;
        }

        public IAsyncRepository<T> AsyncRepository<T>() where T : EntityBase
        {
            return null;
        }

        public Task<int> SaveChangesAsync()
        {
            return _dbContext.SaveChangesAsync();
        }
        public object GetRepositoryViaType(Type type)
        {
            var properties = this.GetType().GetProperties();

            return properties.FirstOrDefault(p => p.PropertyType.GetInterfaces().Any(i => i.GenericTypeArguments.Any(gta => gta.Equals(type)))).GetValue(this);
        }
        public void CleanDirtyDatas(object entity, string? coModelProperty)
        {
            var rp = this.GetRepositoryViaType(entity.GetType());
            if (!string.IsNullOrEmpty(coModelProperty))
            {
                CommonHelper.UpdatePropertyValue(entity, coModelProperty, null);
                CommonHelper.ExecuteMethod(rp, "Update", new object[] { entity, false });
            }
            else
            {
                CommonHelper.ExecuteMethod(rp, "Remove", new object[] { entity });
            }
            CommonHelper.ExecuteMethod(rp, "SaveChange", new object[] { });
        }
        public bool? VerifyEntityCode<T>(T entity, string code) where T : EntityBase
        {
            var rp = this.GetRepositoryViaType(entity.GetType());

            var codes = (CommonHelper.GetPropertyValue(rp, "TableNoTracking") as IEnumerable<T>);

            object result = null;
            try
            {
                result = codes.FirstOrDefault(t => t is ICodeSupportEntity c ? c.Code.Equals(code) : false);
            }
            catch (Exception)
            {
                result = codes.ToList().FirstOrDefault(t => t is ICodeSupportEntity c ? c.Code.Equals(code) : false);
            }

            if (result != null && CommonHelper.GetPropertyValue(result, "Deleted").Equals("N"))
            {
                return result is IStatusSupportEntity status ? status.Status.Equals("A") : true;
            }
            return null;
        }
        private Dictionary<string, List<CodeNameResponseDto>> _tableDefinition;

        private void InitializeTableDefinition()
        {
            _tableDefinition = new Dictionary<string, List<CodeNameResponseDto>>();
            var properties = this.GetType().GetProperties();
            try
            {
                foreach (var property in properties)
                {
                    var propertyInterface = property.PropertyType.GetInterfaces().FirstOrDefault(i => i.Name.StartsWith(nameof(IAsyncRepository<EntityBase>)));
                    if (propertyInterface != null)
                    {
                        var argument = propertyInterface.GenericTypeArguments.FirstOrDefault(a => a.BaseType.Equals(typeof(EntityBase)));
                        if (argument != null)
                        {
                            var className = argument.Name;

                            List<CodeNameResponseDto> codeNames = new List<CodeNameResponseDto>();
                            var columns = argument.GetProperties().Where(p => p.GetCustomAttributes(typeof(ColumnAttribute), true) != null);
                            foreach (var column in columns)
                            {
                                var attribute = column.GetCustomAttributes(typeof(DescriptionAttribute), true);
                                var description = attribute != null && attribute.Length != 0 ? attribute.First() : null;
                                var code = column.Name;
                                codeNames.Add(new CodeNameResponseDto() { Code = code, Name = description != null && description is DescriptionAttribute des ? des.Description : column.Name });
                            }
                            _tableDefinition.Add(className.Substring(0, className.LastIndexOf("Entity")), codeNames);
                        }
                    }

                }
            }
            catch (Exception e)
            {
                _tableDefinition = null;
            }
        }
        public List<CodeNameResponseDto> GetTableDescription(string definedName) 
        {
            if (_tableDefinition == null)
            {
                InitializeTableDefinition();
            }
            return _tableDefinition.GetValueOrDefault(definedName);
        }
        public List<string> GetTableList()
        {
            if (_tableDefinition == null)
            {
                InitializeTableDefinition();
            }
            return _tableDefinition.Keys.ToList();
        }
    }
}
