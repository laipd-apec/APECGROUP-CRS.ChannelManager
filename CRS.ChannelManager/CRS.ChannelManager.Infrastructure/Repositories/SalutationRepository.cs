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
    public class SalutationRepository : RepositoryBase<SalutationEntity>, ISalutationRepository
    {
        public SalutationRepository(IDbContext dbContext, IHttpContextAccessor httpContext) : base(dbContext, httpContext)
        {

        }

        public async Task<SalutationEntity?> FindByCode(string code)
        {
            try
            {
                return TableNoTracking.FirstOrDefault(x => x.Code.Equals(code, StringComparison.OrdinalIgnoreCase));
            }
            catch (Exception)
            {
                return null;
            }
        }

        public async Task<SalutationEntity?> FindBySyncKey(long synckey)
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

    public interface ISalutationRepository : IAsyncRepository<SalutationEntity>
    {
        public Task<SalutationEntity?> FindByCode(string code);

        public Task<SalutationEntity?> FindBySyncKey(long synckey);
    }

}
