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
    public class ChannelRoomTypeRepository : RepositoryBase<ChannelRoomTypeEntity>, IChannelRoomTypeRepository
    {
        public ChannelRoomTypeRepository(IDbContext dbContext, IHttpContextAccessor httpContext) : base(dbContext, httpContext)
        {

        }

        public override ChannelRoomTypeEntity FindById(long? id)
        {
            return TableNoTracking.Include(x => x.Hotel).FirstOrDefault(x => x.Id == id);
        }
    }

    public interface IChannelRoomTypeRepository : IAsyncRepository<ChannelRoomTypeEntity>
    {

    }
}
