using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core;
using Talabat.Core.Entities;
using Talabat.Core.Entities.Orders;
using Talabat.Core.Repositories;
using Talabat.Repository.Data;

namespace Talabat.Repository
{
	public class UnitOfWork : IUnitOfWork
	{
		private readonly StoreContext _context;
		private Hashtable _repositories;
		public UnitOfWork(StoreContext context)
		{
			_context = context;
		}
		
		public async Task<int> Complete()
		{
			return await _context.SaveChangesAsync();
		}

		public async ValueTask DisposeAsync()
		 =>  await _context.DisposeAsync();

		public IGenericRepository<T>? Repository<T>() where T : BaseEntity
		{
			if(_repositories == null)
				_repositories = new Hashtable();
			var type = typeof(T).Name;
			if (!_repositories.ContainsKey(type))
			{
				var repository = new GenericRepository<T>(_context);
				_repositories.Add(type, repository);
			}
			return _repositories[type] as IGenericRepository<T>;
		}
	}
}
