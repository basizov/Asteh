using Asteh.Core.Models;
using Asteh.Core.Models.RequestModels;

namespace Asteh.Core.Services.Authorize
{
	public interface IAuthorizeService<T>
	{
		Task<FullInfoModel?> AuthorizeUserAsync(
			AuthorizeModel authorizeModel,
			CancellationToken cancellationToken = default);
		Task<FullInfoModel> GetFullInfoModelToAuthorizeUser(
			CancellationToken cancellationToken = default);
	}
}
