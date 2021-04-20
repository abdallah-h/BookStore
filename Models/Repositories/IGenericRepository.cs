using BookStore.Data.Specifications;
using System.Collections.Generic;

namespace BookStore.Models.Repositories
{
    public interface IGenericRepository<TEntity>
    {

        IList<TEntity> List(ISpecification<TEntity> spec);
        TEntity GetEntityWithSpec(ISpecification<TEntity> spec);
        IList<TEntity> List();
        TEntity Find(int id);
        void Add(TEntity entity);
        void Update(int id, TEntity entity);
        void Delete(int id);
        IList<TEntity> Search(ISpecification<TEntity> spec);

    }
}
