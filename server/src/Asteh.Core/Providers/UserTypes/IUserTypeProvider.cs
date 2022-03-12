using Asteh.Core.Models;

namespace Asteh.Core.Providers.UserTypes
{
	public interface IUserTypeProvider<T>
	{
		Task<IEnumerable<UserTypeModel>> GetUserTypesAsync(
			CancellationToken cancellationToken = default);
	}
}
