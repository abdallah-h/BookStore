using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace BookStore.Data.Specifications
{
    public class BaseSpecification<TEntity> : ISpecification<TEntity>
    {
        public BaseSpecification() { }

        public BaseSpecification(Expression<Func<TEntity, bool>> criteria)
        {
            Criteria = criteria;
        }

        // use criteria as where clause 
        public Expression<Func<TEntity, bool>> Criteria { get; }

        public List<Expression<Func<TEntity, object>>> Includes { get; } =
        new List<Expression<Func<TEntity, object>>>();


        protected void AddInclude(Expression<Func<TEntity, object>> includeExpression)
        {
            Includes.Add(includeExpression);
        }

    }
}
