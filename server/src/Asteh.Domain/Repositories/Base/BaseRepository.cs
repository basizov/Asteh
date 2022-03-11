using Asteh.Domain.DataProvider;
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

		public IQueryable<T> GetAllToIncludeAsync() => _entities.AsNoTracking();

		public async Task<IReadOnlyCollection<T>> GetAllAsync(
			CancellationToken cancellationToken = default)
		{
			return await _entities
				.AsNoTracking()
				.ToListAsync(cancellationToken);
		}

		public async Task<IReadOnlyCollection<T>> GetAllWithLazyLoadingAsync(
			CancellationToken cancellationToken = default)
		{
			return await _entities
				.ToListAsync(cancellationToken);
		}

		public async Task<IReadOnlyCollection<T>> FindByAsync(
			Expression<Func<T, bool>> expression,
			CancellationToken cancellationToken = default)
		{
			return await _entities
				.Where(expression)
				.ToListAsync(cancellationToken);
		}

		public async Task<T?> SingleOrDefaultAsync(
			Expression<Func<T, bool>> expression,
			CancellationToken cancellationToken = default)
		{
			return await _entities
				.Where(expression)
				.SingleOrDefaultAsync(cancellationToken);
		}

		public async Task<bool> AnyAsync(
			Expression<Func<T, bool>> expression,
			CancellationToken cancellationToken = default)
		{
			return await _entities
				.AnyAsync(expression, cancellationToken);
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
