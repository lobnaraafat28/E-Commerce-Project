using Microsoft.EntityFrameworkCore;
using Talabat.Core.Entities;
using Talabat.Core.Specifications;

namespace Talabat.Repository
{
	public static class SpecificationEvaluator<T> where T : BaseEntity
	{
		public static IQueryable<T> GetQuery(IQueryable<T> startQuery, ISpecification<T> spec)
		{
			var query = startQuery;
			if(spec.OrderBy != null)
	           query = query.OrderBy(spec.OrderBy);
			if (spec.OrderByDesc != null)
				query = query.OrderByDescending(spec.OrderByDesc);

			if (spec.Criteria!= null)
				query= query.Where(spec.Criteria);
			if (spec.IsPagenationEnabled)
			{
				query = query.Skip(spec.Skip).Take(spec.Take);
			}
			query = spec.Includes.Aggregate(query,(CurrentQuery,includeExpression)=> CurrentQuery.Include(includeExpression));



			return query;
		}
	}
}
