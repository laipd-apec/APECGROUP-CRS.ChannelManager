using CRS.ChannelManager.Library.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace CRS.ChannelManager.Library.BaseInterface
{
    public interface IAsyncRepository<Entity> where Entity : EntityBase
    {
        Task<Entity> AddAsync(Entity entity);

        Task<IEnumerable<Entity>> AddAsync(IEnumerable<Entity> entity);

        Task<Entity> GetAsync(Expression<Func<Entity, bool>> expression);

        Task<List<Entity>> ListAsync(Expression<Func<Entity, bool>> expression);

        long FindLastId();

        Entity FindById(long? id);

        Entity FindById(long? id, string msg = "");

        Entity FindOriginalById(long? id);

        Task<Entity?> FindObjectByPropertyValue(string propertyName, object value);

        List<Entity> GetAll();

        void Insert(Entity entity);

        Task<bool> InsertAsync(Entity entity);

        Task<bool> Update(Entity entity, bool withChildren = false);


        void Delete(Entity entity);

        void Remove(Entity entity);

        void Insert(IEnumerable<Entity> entitys);

        Task<bool> InsertAsync(IEnumerable<Entity> entitys);

        Task<bool> Update(IEnumerable<Entity> entitys, bool withChildren = false);

        void Delete(IEnumerable<Entity> entitys);

        void Remove(IEnumerable<Entity> entitys);

        /// <summary>
        /// Gets a table
        /// </summary>
        IQueryable<Entity> Table { get; }

        /// <summary>
        /// Gets a table with "no tracking" enabled (EF feature) Use it only when you load record(s) only for read-only operations
        /// </summary>
        IQueryable<Entity> TableNoTracking { get; }

        /// <summary>
        /// Commits all changes
        /// </summary>
        /// <returns>Affected row(s)</returns>
        int SaveChange();

        Task<int> SaveChangeAsync();

        void Commit();

        List<Tuple<string, string>> GetImportCodesViaTableName(TableAttribute table, string codeColumn);

    }
}