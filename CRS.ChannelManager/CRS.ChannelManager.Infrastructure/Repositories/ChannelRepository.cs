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
    public class ChannelRepository : RepositoryBase<ChannelEntity>, IChannelRepository
    {
        public ChannelRepository(IDbContext dbContext, IHttpContextAccessor httpContext) : base(dbContext, httpContext)
        {

        }
        public async Task<ChannelEntity?> FindByCode(string code)
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

        public async Task<ChannelEntity?> FindBySyncKey(long synckey)
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

    public interface IChannelRepository : IAsyncRepository<ChannelEntity>
    {
        public Task<ChannelEntity?> FindByCode(string code);

        public Task<ChannelEntity?> FindBySyncKey(long synckey);
    }

}
