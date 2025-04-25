using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using CRS.ChannelManager.Library.Utils;
using Microsoft.EntityFrameworkCore.Infrastructure;
using CRS.ChannelManager.Library.BaseInterface;
using CRS.ChannelManager.Library.Constants;
using static CRS.ChannelManager.Library.Constants.CommonConstants;
using System.Linq;
using System;
using System.Threading.Tasks;
using MediatR;
using Nest;
using CRS.ChannelManager.Library.BaseEntities;
using static CRS.ChannelManager.Library.BaseDto.UtilsDto;
using System.Reflection.Metadata;
using Newtonsoft.Json;
using Microsoft.EntityFrameworkCore.Query.SqlExpressions;
using Core.Helpper;

namespace CRS.ChannelManager.Library.Base
{
    public class DbContextBase : DbContext, IDbContext
    {
        private readonly ILogger _logger;

        private IHttpContextAccessor _httpContext;

        private readonly IElasticClient? _elasticClient;

        protected List<AuditEntry>? _auditEntries = null;

        public DbContextBase(
            IHttpContextAccessor httpContext
            , ILogger logger, DbContextOptions options
            , IElasticClient? elasticClient = null) : base(options)
        {
            _httpContext = httpContext;
            _logger = logger;
            _elasticClient = elasticClient;
            _auditEntries = new List<AuditEntry>();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //base.ChangeTracker.LazyLoadingEnabled = false;
            //modelBuilder.HasDbFunction(typeof(SqlFunctionHelper)
            //.GetMethod(nameof(SqlFunctionHelper.Unaccent), new[] { typeof(DbFunctions), typeof(string) }))
            //.HasTranslation(args => new SqlFunctionExpression(
            //    functionName: "unaccent",
            //    nullable: true,
            //    type: typeof(string),
            //    typeMapping: new StringTypeMapping("text", System.Data.DbType.String)
            //));

            base.OnModelCreating(modelBuilder);
        }

        public override int SaveChanges()
        {
            string method = string.Empty;
            string path = string.Empty;
            string username = string.Empty;
            if (_httpContext != null && _httpContext.HttpContext != null)
            {
                method = _httpContext.HttpContext.Request.Method.ToUpper();
                path = _httpContext.HttpContext.Request.Path;
                username = CommonUtils.GetCurrentUsername(_httpContext);
            }
            using (IDbContextTransaction transaction = Database.BeginTransaction())
            {
                try
                {
                    var entries = ChangeTracker.Entries()
                    .Where(x => (x.Entity is EntityBase) && (x.State == EntityState.Added || x.State == EntityState.Modified || x.State == EntityState.Deleted));
                    var data = entries.Select(t => t.Entity).ToList();
                    username = string.IsNullOrEmpty(username) ? "root" : username;
                    var dateTimeNow = TimeZoneInfo.ConvertTime(DateTime.Now, TimeZoneInfo.FindSystemTimeZoneById(TimeZoneValue.VnPlut7));
                    if (entries != null && entries.Any())
                    {
                        foreach (var entry in entries)
                        {
                            if (entry.Entity is EntityBase entity)
                            {
                                if (entry.State == EntityState.Added)
                                {
                                    if (string.IsNullOrEmpty(entity.CreatedBy))
                                    {
                                        entity.CreatedBy = username;
                                    }
                                    entity.CreatedDate = dateTimeNow;
                                    entity.Deleted = CommonConstants.YesNo.NO;
                                }
                                else if (entry.State == EntityState.Modified)
                                {
                                    var res = entity.GetType().GetProperty("ModifiedBy") != null;
                                    if (res)
                                    {
                                        entity.GetType().GetProperty("ModifiedBy").SetValue(entity, username);
                                    }
                                    res = entity.GetType().GetProperty("ModifiedDate") != null;
                                    if (res)
                                    {
                                        entity.GetType().GetProperty("ModifiedDate").SetValue(entity, dateTimeNow);
                                    }
                                    //entity.ModifiedBy = username;
                                    //entity.ModifiedDate = DateTime.Now;
                                }
                                if (string.IsNullOrEmpty(entity.CreatedBy))
                                {
                                    entity.CreatedBy = username;
                                }
                            }
                        }
                    }
                    OnBeforeSaveChanges(username);
                    int save = base.SaveChanges();
                    transaction.Commit();
                    if (_elasticClient != null && _auditEntries != null && _auditEntries.Any())
                    {
                        //foreach (var itemEntry in _auditEntries)
                        //{
                        //    var checkLog = _elasticClient.IndexDocument(itemEntry);
                        //}
                    }
                    return save;
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    object message = ex.InnerException == null ? ex.Message : ex.InnerException;
                    _logger.LogError(string.Format("{0} {1} {2}", method, path, message));
                    //if (_elasticClient != null)
                    //{
                    //    var checkLog = _elasticClient.IndexDocument((string.Format("{0} {1} {2}", method, path, message)));
                    //}
                    throw new DomainExceptionBase(Constants.ErrorCode.SQL_BASE_ERROR, message.ToString() ?? ex.Message);
                }
            }

        }

        public async Task<int> SaveChangesAsync()
        {
            string method = _httpContext.HttpContext.Request.Method.ToUpper();
            string path = _httpContext.HttpContext.Request.Path;
            await using (var transaction = await Database.BeginTransactionAsync())
            {
                try
                {
                    var entries = ChangeTracker.Entries()
                    .Where(x => (x.Entity is EntityBase) && (x.State == EntityState.Added || x.State == EntityState.Modified || x.State == EntityState.Deleted));
                    var data = entries.Select(t => t.Entity).ToList();
                    string username = CommonUtils.GetCurrentUsername(_httpContext);
                    username = string.IsNullOrEmpty(username) ? "root" : username;
                    if (entries != null && entries.Any())
                    {
                        foreach (var entry in entries)
                        {
                            if (entry.Entity is EntityBase entity)
                            {
                                if (entry.State == EntityState.Added)
                                {
                                    if (string.IsNullOrEmpty(entity.CreatedBy))
                                    {
                                        entity.CreatedBy = username;
                                    }
                                    entity.CreatedDate = DateTime.Now;
                                    entity.Deleted = CommonConstants.YesNo.NO;
                                }
                                else if (entry.State == EntityState.Modified)
                                {
                                    var res = entity.GetType().GetProperty("ModifiedBy") != null;
                                    if (res)
                                    {
                                        entity.GetType().GetProperty("ModifiedBy").SetValue(entity, username);
                                    }
                                    res = entity.GetType().GetProperty("ModifiedDate") != null;
                                    if (res)
                                    {
                                        entity.GetType().GetProperty("ModifiedDate").SetValue(entity, DateTime.Now);
                                    }
                                }
                            }
                        }
                    }
                    OnBeforeSaveChanges(username);
                    var save = await base.SaveChangesAsync();
                    await transaction.CommitAsync();
                    if (_elasticClient != null && _auditEntries != null && _auditEntries.Any())
                    {
                        var checkLog = _elasticClient.IndexDocument(_auditEntries);
                    }
                    return save;
                }
                catch (Exception ex)
                {
                    await transaction.RollbackAsync();
                    object message = ex.InnerException == null ? ex.Message : ex.InnerException;
                    _logger.LogError(string.Format("{0} {1} {2}", method, path, ex.ToString()));
                    if (_elasticClient != null)
                    {
                        var checkLog = _elasticClient.IndexDocument((string.Format("{0} {1} {2}", method, path, message)));
                    }
                    throw new DomainExceptionBase(Constants.ErrorCode.SQL_BASE_ERROR, ex.Message);
                }
            }
        }

        /// <summary>
        /// Get DbSet
        /// </summary>
        /// <typeparam name="TEntity">Entity type</typeparam>
        /// <returns>DbSet</returns>
        public new DbSet<TEntity> Set<TEntity>() where TEntity : class
        {
            return base.Set<TEntity>();
        }

        public virtual void OnBeforeSaveChanges(string userName)
        {

        }

        /// <summary>
        /// Provides access to features of the context that deal with change tracking of entities.
        /// </summary>
        public new ChangeTracker ChangeTracker
        {
            get
            {
                base.ChangeTracker.LazyLoadingEnabled = false;
                return base.ChangeTracker;
            }
        }

        public new DatabaseFacade Database
        {
            get
            {
                return base.Database;
            }
        }

    }
}