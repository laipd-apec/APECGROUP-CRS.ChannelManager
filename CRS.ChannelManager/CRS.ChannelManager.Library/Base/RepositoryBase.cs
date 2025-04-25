using CRS.ChannelManager.Library.Base;
using CRS.ChannelManager.Library.EnumType;
using CRS.ChannelManager.Library.Helpper;
using CRS.ChannelManager.Library.BaseInterface;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using CRS.ChannelManager.Library.BaseEnum;
using CRS.ChannelManager.Library.Utils;
using System.Data;
using Npgsql;
using System.ComponentModel.DataAnnotations.Schema;
using Nest;
//using static Microsoft.AspNetCore.Hosting.Internal.HostingApplication;

namespace CRS.ChannelManager.Library.Base
{
    public abstract class RepositoryBase<T> : IAsyncRepository<T> where T : EntityBase
    {
        private readonly DbSet<T> _dbSet;

        private DbSet<T> _entities;

        public IDbContext _dbContext { get; }

        private IHttpContextAccessor _httpContext;

        public RepositoryBase(IDbContext dbContext, IHttpContextAccessor httpContext)
        {
            _dbSet = dbContext.Set<T>();
            _dbContext = dbContext;
            _httpContext = httpContext;
        }

        public virtual IQueryable<T> TableNoTracking
        {
            get
            {
                return Entities.Where(x => x.Deleted == YesNoEnum.No.ToEnumMemberString()).AsNoTracking();
            }
        }

        protected virtual DbSet<T> Entities
        {
            get
            {
                if (_entities == null)
                    _entities = _dbContext.Set<T>();
                return _entities;
            }
        }

        public virtual IQueryable<T> Table
        {
            get
            {
                return Entities;
            }
        }

        public virtual IQueryable<T> ExtendQuery(IQueryable<T> query) { return query; }

        public async Task<T> AddAsync(T entity)
        {
            await _dbSet.AddAsync(entity);
            return entity;
        }

        public async Task<IEnumerable<T>> AddAsync(IEnumerable<T> entity)
        {
            await _dbSet.AddRangeAsync(entity);
            return entity;
        }

        public void Delete(T entity)
        {
            try
            {
                if (entity == null)
                    throw new ArgumentNullException(nameof(entity));

                _dbContext.Entry(entity).State = EntityState.Modified;
                var validationContext = new ValidationContext(entity);
                Validator.ValidateObject(entity, validationContext);

                entity.Deleted = YesNoEnum.Yes.ToEnumMemberString();
                entity.DeletedDate = DateTime.Now;
                entity.DeletedBy = CommonUtils.GetCurrentUsername(_httpContext);
            }
            catch (ValidationException dbEx)
            {
                //ensure that the detailed error text is saved in the Log
                throw new DomainExceptionBase(dbEx.Message);
            }
        }

        public void Delete(IEnumerable<T> entitys)
        {
            try
            {
                if (entitys == null)
                    throw new ArgumentNullException(nameof(entitys));

                foreach (var entity in entitys)
                    Delete(entity);
            }
            catch (ValidationException dbEx)
            {
                //ensure that the detailed error text is saved in the Log
                throw new DomainExceptionBase(dbEx.Message);
            }
        }

        public void Delete(List<long> ids)
        {
            try
            {
                List<T> entitys = new List<T>();
                foreach (var id in ids)
                {
                    var find = FindById(id);
                    entitys.Add(find);
                }
                if (entitys == null)
                    throw new ArgumentNullException(nameof(entitys));

                foreach (var entity in entitys)
                    Delete(entity);
            }
            catch (ValidationException dbEx)
            {
                //ensure that the detailed error text is saved in the Log
                throw new DomainExceptionBase(dbEx.Message);
            }
        }

        public void Remove(T entity)
        {
            try
            {
                if (entity == null)
                    throw new ArgumentNullException(nameof(entity));

                _dbContext.Entry(entity).State = EntityState.Deleted;
                var validationContext = new ValidationContext(entity);
                Validator.ValidateObject(entity, validationContext);

                Entities.Remove(entity);
            }
            catch (ValidationException dbEx)
            {
                //ensure that the detailed error text is saved in the Log
                throw new DomainExceptionBase(dbEx.Message);
            }
        }

        public void Remove(IEnumerable<T> entitys)
        {
            try
            {
                if (entitys == null)
                    throw new ArgumentNullException(nameof(entitys));

                foreach (var entity in entitys)
                    Remove(entity);
            }
            catch (ValidationException dbEx)
            {
                //ensure that the detailed error text is saved in the Log
                throw new DomainExceptionBase(dbEx.Message);
            }
        }

        public void Remove(List<long> ids)
        {
            try
            {
                IEnumerable<T> entitys = new List<T>();
                foreach (var id in ids)
                {
                    entitys.Append(FindById(id));
                }

                if (entitys == null)
                    throw new ArgumentNullException(nameof(entitys));

                foreach (var entity in entitys)
                    Remove(entity);
            }
            catch (ValidationException dbEx)
            {
                //ensure that the detailed error text is saved in the Log
                throw new DomainExceptionBase(dbEx.Message);
            }
        }

        public virtual T FindById(long? id)
        {
            var entity = ExtendQuery(TableNoTracking).FirstOrDefault(p => p.Id == id);
            if (entity == null)
            {
                throw new DomainExceptionBase($"{typeof(T).Name} Not found object");
            }
            return entity;
        }

        public virtual T FindById(long? id, string msg = "")
        {
            var entity = ExtendQuery(TableNoTracking).FirstOrDefault(t => t.Id == id);
            if (entity == null)
            {
                if (string.IsNullOrEmpty(msg))
                {
                    throw new DomainExceptionBase($"{typeof(T).Name} Not found object");
                }
                throw new DomainExceptionBase(msg);
            }
            return entity;
        }

        public virtual T FindOriginalById(long? id)
        {
            return ExtendQuery(TableNoTracking).FirstOrDefault(p => p.Id == id);
        }

        public async Task<T?> FindObjectByPropertyValue(string propertyName, object value)
        {
            var query = TableNoTracking.AsQueryable();
            var parameter = Expression.Parameter(typeof(T), "x");
            var property = Expression.Property(parameter, propertyName);

            // Handle value based on its type
            ConstantExpression constant;
            switch (value)
            {
                case int intValue:
                    constant = Expression.Constant(intValue);
                    break;
                case long longValue:
                    constant = Expression.Constant(longValue);
                    break;
                case string stringValue:
                    constant = Expression.Constant(stringValue);
                    break;
                case DateTime dateTimeValue:
                    constant = Expression.Constant(dateTimeValue);
                    break;
                default:
                    constant = Expression.Constant(value);
                    break;
            }

            // Ensure both sides of the comparison are of the same type
            var equality = Expression.Equal(property, Expression.Convert(constant, property.Type));
            var lambda = Expression.Lambda<Func<T, bool>>(equality, parameter);

            return query.SingleOrDefault(lambda);

            //foreach (var entity in query)
            //{
            //    var propertyInfo = entity.GetType().GetProperty(propertyName);
            //    if (propertyInfo != null)
            //    {
            //        var propertyValue = propertyInfo.GetValue(entity);
            //        if (propertyValue != null && propertyValue.Equals(value))
            //        {
            //            return entity;
            //        }
            //    }
            //}
            //return null;
        }

        public virtual long FindLastId()
        {
            var lastItem = ExtendQuery(TableNoTracking).OrderByDescending(t => t.Id).FirstOrDefault();
            if (lastItem != null)
                return lastItem.Id;
            return 0;
        }

        public virtual List<T> GetAll()
        {
            return ExtendQuery(TableNoTracking).OrderByDescending(t => t.Id).ToList();
        }

        public Task<T> GetAsync(Expression<Func<T, bool>> expression)
        {
            return _dbSet.FirstOrDefaultAsync(expression);
        }

        public void Insert(T entity)
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity));
            Entities.Add(entity);
        }

        public void Insert(IEnumerable<T> entitys)
        {
            Entities.AddRange(entitys);
        }

        public Task<bool> InsertAsync(T entity)
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity));
            Entities.AddAsync(entity);
            return Task.FromResult(true);
        }

        public Task<bool> InsertAsync(IEnumerable<T> entitys)
        {
            try
            {
                foreach (var item in entitys)
                {
                    InsertAsync(item);
                }
                return Task.FromResult(true);
            }
            catch (DomainExceptionBase ex)
            {
                throw new DomainExceptionBase(ex.Message);
            }
        }

        public Task<List<T>> ListAsync(Expression<Func<T, bool>> expression)
        {
            return _dbSet.Where(expression).ToListAsync();
        }

        public int SaveChange()
        {
            return _dbContext.SaveChanges();

        }

        public async Task<int> SaveChangeAsync()
        {
            return await _dbContext.SaveChangesAsync();
        }


        public async Task<bool> Update(T entity, bool withChildren = false)
        {
            T existing = _dbContext.Set<T>().Local.FirstOrDefault(entry => entry.Id.Equals(entity.Id));
            if (existing != null)
            {
                _dbContext.Entry(existing).State = EntityState.Detached;
                entity.CreatedBy = existing.CreatedBy;
                entity.CreatedDate = existing.CreatedDate;
            }
            if (string.IsNullOrEmpty(entity.Deleted))
            {
                entity.Deleted = YesNoEnum.No.ToEnumMemberString();
            }
            if (!withChildren)
            {
                _dbContext.Entry(entity).State = EntityState.Modified;
            }
            var validationContext = new ValidationContext(entity);
            try
            {
                Validator.ValidateObject(entity, validationContext);
                Entities.Update(entity);
                return true;
            }
            catch (DomainExceptionBase ex)
            {
                object message = ex.InnerException == null ? ex.Message : ex.InnerException;
                throw new DomainExceptionBase(Convert.ToString(message));
            }
        }

        public async Task<bool> Update(IEnumerable<T> entitys, bool withChildren = false)
        {
            try
            {
                foreach (var item in entitys)
                {
                    Update(item, withChildren);
                }
                return true;
            }
            catch (DomainExceptionBase ex)
            {
                object message = ex.InnerException == null ? ex.Message : ex.InnerException;
                throw new DomainExceptionBase(Convert.ToString(message));
            }

        }

        public void Commit()
        {
            using (IDbContextTransaction transaction = _dbContext.Database.BeginTransaction())
            {
                try
                {
                    _dbContext.SaveChanges();
                    transaction.Commit();
                }
                catch (DomainExceptionBase ex)
                {
                    object message = ex.InnerException == null ? ex.Message : ex.InnerException;
                    throw new DomainExceptionBase(Convert.ToString(message));
                }
            }
        }

        public string ExecuteRawSqlFirstLineReturn(string rawSql)
        {
            try
            {
                NpgsqlConnection conn = new NpgsqlConnection(_dbContext.Database.GetConnectionString());
                conn.Open();

                NpgsqlCommand command = new NpgsqlCommand(rawSql/*"SELECT public.get_emp_code() CODE;"*/, conn);
                command.CommandType = CommandType.Text;
                NpgsqlDataReader reader = command.ExecuteReader();

                string empCode = string.Empty;
                while (reader.Read())
                {
                    empCode = reader[0].ToString();
                    break;
                }

                reader.Close();
                command.Dispose();
                conn.Close();

                return empCode;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

        }

        public List<Tuple<string, string>> GetImportCodesViaTableName(TableAttribute table, string codeColumn)
        {
            try
            {
                NpgsqlConnection conn = new NpgsqlConnection(_dbContext.Database.GetConnectionString());
                conn.Open();

                NpgsqlCommand command = new NpgsqlCommand($"SELECT public.get_codes('{table.Schema}','{table.Name}','{codeColumn}')", conn);
                command.CommandType = CommandType.Text;
                NpgsqlDataReader reader = command.ExecuteReader();

                List<Tuple<string, string>> idCodesPairs = new List<Tuple<string, string>>();
                while (reader.Read())
                {
                    idCodesPairs.Add(new Tuple<string, string>(((object[])reader[0])[0].ToString(), ((object[])reader[0])[1].ToString()));
                }

                reader.Close();
                command.Dispose();
                conn.Close();

                return idCodesPairs;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

        }


    }
}