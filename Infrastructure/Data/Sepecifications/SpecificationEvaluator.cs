using Core.Entities;
using Core.Specifications;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data
{
    public class SpecificationEvaluator<TEntity> where TEntity : BaseEntity
    {
        public static IQueryable<TEntity> GetQuery(IQueryable<TEntity> input,
        ISpecification<TEntity> spec)
        {
            var qurey = input;

            if (spec.Criteria != null)
            {
                qurey = qurey.Where(spec.Criteria);
            }

            if (spec.OrderBy != null)
            {
                qurey = qurey.OrderBy(spec.OrderBy);
            }

            if (spec.OrderByDescending != null)
            {
                qurey = qurey.OrderByDescending(spec.OrderByDescending);
            }

            if (spec.IsPagingEnabled)
            {
                qurey = qurey.Skip(spec.Skip).Take(spec.Take);
            }

            qurey = spec.Includes.Aggregate(qurey, (currentQuery, include) => currentQuery.Include(include));

            return qurey;
        }
    }
}