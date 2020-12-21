using System.Collections.Generic;

namespace Fifth.Interfaces.DataAccess
{
    public interface IRepository<TEntity, TKey>
    {
        void Create(TEntity entity);

        TEntity Get(TKey key);

        IEnumerable<TEntity> GetAll();

        void Update(TEntity entity);

        void Delete(TKey key);
    }
}