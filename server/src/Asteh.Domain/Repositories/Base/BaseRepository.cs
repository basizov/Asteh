using Asteh.Domain.Database;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Asteh.Domain.Repositories.Base
{
	internal class BaseRepository<T> : IBaseRepository<T>
		where T : class
	{
		private readonly DbSet<T> _entities;
		private readonly ApplicationDbContext _applicationDbContext;

		public BaseRepository(ApplicationDbContext applicationDbContext)
		{
			_entities = applicationDbContext.Set<T>();
			_applicationDbContext = applicationDbContext;
		}

		public async Task<IReadOnlyCollection<T>> GetAllAsync(CancellationToken cancellationToken)
		{
			return await _entities
				.AsNoTracking()
				.ToListAsync(cancellationToken);
		}

		public async Task<IReadOnlyCollection<T>> FindByAsync
			(Expression<Func<T, bool>> expression,
			CancellationToken cancellationToken)
		{
			return await _entities
				.Where(expression)
				.AsNoTracking()
				.ToListAsync(cancellationToken);
		}

		public virtual T Create(T entity) => _entities.Add(entity).Entity;

		public virtual T Update(T entity)
		{
			var	newEntity = _entities.Attach(entity).Entity;
			_applicationDbContext.Entry(entity).State = EntityState.Modified;
			return newEntity;
		}

		public virtual void Delete(T entity)
		{
			if (_applicationDbContext.Entry(entity).State == EntityState.Detached)
			{
				_entities.Attach(entity);
			}
			_entities.Remove(entity);
		}
	}
}
