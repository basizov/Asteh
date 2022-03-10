using System.Linq.Expressions;

namespace Asteh.Domain.Repositories.Base
{
	public interface IBaseRepository<T>
	{
		Task<IReadOnlyCollection<T>> GetAllAsync(CancellationToken cancellationToken);
		Task<IReadOnlyCollection<T>> FindByAsync(
			Expression<Func<T, bool>> expression,
			CancellationToken cancellationToken);
		T Create(T entity);
		T Update(T entity);
		void Delete(T entity);
	}
}
