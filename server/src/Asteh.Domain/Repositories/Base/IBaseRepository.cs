using System.Linq.Expressions;

namespace Asteh.Domain.Repositories.Base
{
	public interface IBaseRepository<T>
	{
		Task<IReadOnlyCollection<T>> GetAllAsync(CancellationToken cancellationToken = default);
		Task<IReadOnlyCollection<T>> FindByAsync(
			Expression<Func<T, bool>> expression,
			CancellationToken cancellationToken = default);
		Task<bool> AnyAsync(
			Expression<Func<T, bool>> expression,
			CancellationToken cancellationToken = default);
		T Create(T entity);
		T Update(T entity);
		void Delete(T entity);
	}
}
