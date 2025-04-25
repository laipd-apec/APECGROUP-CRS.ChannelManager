using CRS.ChannelManager.Domain.Entities;
using CRS.ChannelManager.Library.Base;
using CRS.ChannelManager.Library.BaseInterface;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRS.ChannelManager.Infrastructure.Repositories
{
    public class PackageRepository : RepositoryBase<PackageEntity>, IPackageRepository
    {
        public PackageRepository(IDbContext dbContext, IHttpContextAccessor httpContext) : base(dbContext, httpContext)
        {

        }
        public async Task<PackageEntity?> FindByCode(string code)
        {
            try
            {
                return TableNoTracking.FirstOrDefault(x => x.Code.ToLower() == code.ToLower());
            }
            catch (Exception)
            {
                return null;
            }
        }

        public async Task<PackageEntity?> FindBySyncKey(long synckey)
        {
            try
            {
                return TableNoTracking.FirstOrDefault(x => x.SyncKey == synckey);
            }
            catch (Exception)
            {
                return null;
            }
        }
    }

    public interface IPackageRepository : IAsyncRepository<PackageEntity>
    {
        public Task<PackageEntity?> FindByCode(string code);

        public Task<PackageEntity?> FindBySyncKey(long synckey);
    }

}
