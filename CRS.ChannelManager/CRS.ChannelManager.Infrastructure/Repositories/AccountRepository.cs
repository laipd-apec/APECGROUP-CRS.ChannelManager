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
    public class AccountRepository : RepositoryBase<AccountEntity>, IAccountRepository
    {
        public AccountRepository(IDbContext dbContext, IHttpContextAccessor httpContext) : base(dbContext, httpContext)
        {

        }

        public async Task<AccountEntity?> FindByCode(string code)
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

        public async Task<AccountEntity?> FindBySyncKey(long synckey)
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

    public interface IAccountRepository : IAsyncRepository<AccountEntity>
    {
        public Task<AccountEntity?> FindByCode(string code);

        public Task<AccountEntity?> FindBySyncKey(long synckey);
    }

}
