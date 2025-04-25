using CRS.ChannelManager.Domain.Entities;
using CRS.ChannelManager.Library.Base;
using CRS.ChannelManager.Library.BaseEntities;
using CRS.ChannelManager.Library.EnumType;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Nest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRS.ChannelManager.Domain
{
    public class CRSChannelManagerContext : DbContextBase
    {
        private IHttpContextAccessor _httpContext;
        private readonly ILogger<CRSChannelManagerContext> _logger;

        public CRSChannelManagerContext(
            IHttpContextAccessor httpContext
            , DbContextOptions<CRSChannelManagerContext> options
            , ILogger<CRSChannelManagerContext> logger
            , IElasticClient? elasticClient) : base(httpContext, logger, options, elasticClient)
        {
            AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
            _httpContext = httpContext;
            _logger = logger;
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            //optionsBuilder.ConfigureWarnings(w => w.Throw(RelationalEventId.MultipleCollectionIncludeWarning));
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            // Add your custom model configurations here
        }

        public override void OnBeforeSaveChanges(string userName)
        {
            ChangeTracker.DetectChanges();
            var auditEntries = new List<AuditEntry>();
            foreach (var entry in ChangeTracker.Entries())
            {
                if (entry.Entity is Library.BaseEntities.AuditEntity || entry.State == EntityState.Detached || entry.State == EntityState.Unchanged)
                    continue;
                //var auditEntry = new AuditEntry(entry);
                var auditEntry = new AuditEntry();
                auditEntry.TableName = entry.Entity.GetType().Name;
                auditEntry.UserName = userName;
                auditEntries.Add(auditEntry);
                foreach (var property in entry.Properties)
                {
                    string propertyName = property.Metadata.Name;
                    if (property.Metadata.IsPrimaryKey())
                    {
                        auditEntry.KeyValues[propertyName] = property.CurrentValue;
                        continue;
                    }
                    switch (entry.State)
                    {
                        case EntityState.Added:
                            auditEntry.AuditType = AuditType.Create;
                            auditEntry.NewValues[propertyName] = property.CurrentValue;
                            break;
                        case EntityState.Deleted:
                            auditEntry.AuditType = AuditType.Delete;
                            auditEntry.OldValues[propertyName] = property.OriginalValue;
                            break;
                        case EntityState.Modified:
                            if (property.IsModified)
                            {
                                auditEntry.ChangedColumns.Add(propertyName);
                                auditEntry.AuditType = AuditType.Update;
                                auditEntry.OldValues[propertyName] = property.OriginalValue;
                                auditEntry.NewValues[propertyName] = property.CurrentValue;
                            }
                            break;
                    }
                }
                if (auditEntries.Any())
                {
                    _auditEntries = auditEntries;
                }
            }
        }

        public virtual DbSet<AccountEntity> Account { get; set; }
        public virtual DbSet<ChannelEntity> Channel { get; set; }
        public virtual DbSet<CountryEntity> Country { get; set; }
        public virtual DbSet<HotelEntity> Hotel { get; set; }
        public virtual DbSet<IdentificationTypeEntity> IdentificationType { get; set; }
        public virtual DbSet<MarketSegmentEntity> MarketSegment { get; set; }
        public virtual DbSet<PackageEntity> Package { get; set; }
        public virtual DbSet<PackagePlanEntity> PackagePlan { get; set; }
        public virtual DbSet<ProductEntity> Product { get; set; }
        public virtual DbSet<RoomTypeEntity> RoomType { get; set; }
        public virtual DbSet<RatePlanEntity> RatePlan { get; set; }
        public virtual DbSet<SalutationEntity> Salutation { get; set; }
        public virtual DbSet<ServiceEntity> Service { get; set; }
        public virtual DbSet<SubSegmentEntity> SubSegment { get; set; }
        public virtual DbSet<TravelAgentEntity> TravelAgent { get; set; }
        public virtual DbSet<ChannelRoomTypeEntity> ChannelRoomType { get; set; }
        public virtual DbSet<ChannelMappingRoomTypeEntity> ChannelMappingRoomType { get; set; }
    }
}
