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
    public class IdentificationTypeRepository : RepositoryBase<IdentificationTypeEntity>, IIdentificationTypeRepository
    {
        public IdentificationTypeRepository(IDbContext dbContext, IHttpContextAccessor httpContext) : base(dbContext, httpContext)
        {

        }

    }

    public interface IIdentificationTypeRepository : IAsyncRepository<IdentificationTypeEntity>
    {

    }

}
