using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRS.ChannelManager.Infrastructure.Shares.ElasticSearch
{
    public interface IElasticSearchRepository<T>
    {
        Task<T?> Get(Guid id, CancellationToken cancellationToken);
        Task Add(Guid id, T entity, CancellationToken cancellationToken);
        Task Update(Guid id, T entity, CancellationToken cancellationToken);
        Task Delete(Guid id, T entity, CancellationToken cancellationToken);
        Task<IEnumerable<T>> GetAll(CancellationToken cancellationToken);
    }
}
