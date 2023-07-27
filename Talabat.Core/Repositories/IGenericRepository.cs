using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Entities;
using Talabat.Core.Specifications;

namespace Talabat.Core.Repositories
{
	public interface IGenericRepository<T> where T: BaseEntity
	{
		Task<IReadOnlyList<T>> GetAllAsync();
		Task<T> GetByIdAsync(int id);
		Task<IReadOnlyList<T>> GetAllSpecAsync(ISpecification<T> spec);
		Task<T> GetByIdSpecAsync(ISpecification<T> spec);
		Task<int> GetCountWithSpec(ISpecification<T> spec);

		Task Add(T entity);
		void Delete(T entity);
		void Update(T entity);
	}
}
