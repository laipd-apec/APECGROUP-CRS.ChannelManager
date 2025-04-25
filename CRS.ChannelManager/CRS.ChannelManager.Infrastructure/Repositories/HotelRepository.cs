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
    public class HotelRepository : RepositoryBase<HotelEntity>, IHotelRepository
    {
        public HotelRepository(IDbContext dbContext, IHttpContextAccessor httpContext) : base(dbContext, httpContext)
        {

        }

        public async Task<HotelEntity?> FindByCode(string code)
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

        public async Task<HotelEntity?> FindBySyncKey(long synckey)
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

        public async Task<HotelEntity?> FindByHotelId(string hotelId)
        {
            try
            {
                return TableNoTracking.FirstOrDefault(x => x.HotelId == hotelId);
            }
            catch (Exception)
            {
                return null;
            }
        }

    }

    public interface IHotelRepository : IAsyncRepository<HotelEntity>
    {
        public Task<HotelEntity?> FindByCode(string code);

        public Task<HotelEntity?> FindBySyncKey(long synckey);

        public Task<HotelEntity?> FindByHotelId(string hotelId);

    }

}
