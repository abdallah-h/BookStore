using BookStore.Data;
using BookStore.Data.Specifications;
using System.Collections.Generic;
using System.Linq;

namespace BookStore.Models.Repositories
{
    public class GenericRepository<TEntity> : IGenericRepository<TEntity> where TEntity : BaseEntity
    {
        private readonly StoreContext storeContext;

        public GenericRepository(StoreContext storeContext)
        {
            this.storeContext = storeContext;
        }

        public void Add(TEntity entity)
        {
            storeContext.Set<TEntity>().Add(entity);
            CommitChanges();
        }

        public void Delete(int id)
        {
            var entity = Find(id);
            storeContext.Set<TEntity>().Remove(entity);
            CommitChanges();
        }

        public TEntity Find(int id)
        {
            var entity = storeContext.Set<TEntity>().SingleOrDefault(a => a.Id == id);

            return entity;
        }



        public IList<TEntity> List(ISpecification<TEntity> spec)
        {
            return ApplySpecification(spec).ToList();
        }


        public TEntity GetEntityWithSpec(ISpecification<TEntity> spec)
        {
            return ApplySpecification(spec).FirstOrDefault();
        }

        public IList<TEntity> List()
        {
            return storeContext.Set<TEntity>().ToList();
        }


        //public IList<Book> ListInc()
        //{
        //    return storeContext.Books.Include(a => a.Author).ToList();
        //}

        public IList<TEntity> Search(ISpecification<TEntity> spec)
        {
            return ApplySpecification(spec).ToList();
        }

        public void Update(int id, TEntity entity)
        {
            storeContext.Set<TEntity>().Update(entity);
            CommitChanges();
        }

        private void CommitChanges()
        {
            storeContext.SaveChanges();
        }



        private IQueryable<TEntity> ApplySpecification(ISpecification<TEntity> spec)
        {
            return SpecificationEvaluator<TEntity>.GetQuery(storeContext.Set<TEntity>().AsQueryable(), spec);
        }

    }
}
