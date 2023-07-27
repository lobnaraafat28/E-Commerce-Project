using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Entities;
using Talabat.Core.Repositories;
using Talabat.Core.Specifications;
using Talabat.Repository.Data;

namespace Talabat.Repository
{
	public class GenericRepository<T> : IGenericRepository<T> where T : BaseEntity
	{
		private readonly StoreContext _dbcontext;

		public GenericRepository(StoreContext dbcontext)
		{
			_dbcontext = dbcontext;
		}
		public async Task<IReadOnlyList<T>> GetAllAsync()
		{
			 return await _dbcontext.Set<T>().ToListAsync();
		}
		public async Task<T> GetByIdAsync(int id)
		{
			return await _dbcontext.Set<T>().FindAsync(id);
		}

		public async Task<IReadOnlyList<T>> GetAllSpecAsync(ISpecification<T> spec)
		{
			return await ApplySpecification(spec).ToListAsync();

		}


		public async Task<T> GetByIdSpecAsync(ISpecification<T> spec)
		{
			return await ApplySpecification(spec).FirstOrDefaultAsync();
		}


		private IQueryable<T> ApplySpecification(ISpecification<T> spec)
		{
			return SpecificationEvaluator<T>.GetQuery(_dbcontext.Set<T>(), spec);
		}

		public async Task Add(T entity)
		{
			 await _dbcontext.Set<T>().AddAsync(entity);
		}

		public void Delete(T entity)
		{
			 _dbcontext.Set<T>().Update(entity);
		}

		public void Update(T entity)
		{
		     _dbcontext.Set<T>().Remove(entity);
		}

		public async Task<int> GetCountWithSpec(ISpecification<T> spec)
		{
			return await ApplySpecification(spec).CountAsync();
		}
	}
}
