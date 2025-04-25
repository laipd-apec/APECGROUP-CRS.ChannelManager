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
    public class RoomTypeRepository : RepositoryBase<RoomTypeEntity>, IRoomTypeRepository
    {
        public RoomTypeRepository(IDbContext dbContext, IHttpContextAccessor httpContext) : base(dbContext, httpContext)
        {

        }
        public async Task<RoomTypeEntity?> FindByCode(string code)
        {
            try
            {
                return TableNoTracking.FirstOrDefault(x => x.Code.ToLower().Equals(code.ToLower()));
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public async Task<RoomTypeEntity?> FindBySyncKey(long synckey)
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

        public async Task<RoomTypeEntity?> FindByRoomtypeId(long roomTypeId)
        {
            try
            {
                return TableNoTracking.FirstOrDefault(x => x.RoomTypeId.ToLower() == roomTypeId.ToString());
            }
            catch (Exception)
            {
                return null;
            }
        }
    }

    public interface IRoomTypeRepository : IAsyncRepository<RoomTypeEntity>
    {
        public Task<RoomTypeEntity?> FindByCode(string code);

        public Task<RoomTypeEntity?> FindBySyncKey(long synckey);

        public Task<RoomTypeEntity?> FindByRoomtypeId(long synckey);
    }

}
