using CRS.ChannelManager.Domain.Entities;
using CRS.ChannelManager.Library.Base;
using CRS.ChannelManager.Library.BaseInterface;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRS.ChannelManager.Infrastructure.Repositories
{
    public class ChannelMappingRoomTypeRepository : RepositoryBase<ChannelMappingRoomTypeEntity>, IChannelMappingRoomTypeRepository
    {
        public ChannelMappingRoomTypeRepository(IDbContext dbContext, IHttpContextAccessor httpContext) : base(dbContext, httpContext)
        {

        }

        public override ChannelMappingRoomTypeEntity FindById(long? id)
        {
            return TableNoTracking.Include(x => x.ChannelRoomType)
                                  .Include(x => x.Hotel)
                                  .Include(x => x.Channel)
                                  .Include(x => x.RoomType)
                                  .Include(x => x.Product)
                                  .Include(x => x.Account)
                                  .Include(x => x.PackagePlan)
                                  .FirstOrDefault(x => x.Id == id);
        }
    }

    public interface IChannelMappingRoomTypeRepository : IAsyncRepository<ChannelMappingRoomTypeEntity>
    {

    }
}
