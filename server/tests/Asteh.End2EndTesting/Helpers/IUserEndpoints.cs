using Asteh.Core.Models;
using Asteh.Core.Models.RequestModels;
using Refit;

namespace Asteh.End2EndTesting.Helpers
{
	public interface IUserEndpoints
	{
		[Get("/User/{id}")]
		Task<UserModel> GetUserByIdAsync(int id);
		[Post("/User")]
		Task<UserModel> CreateUserAsync(UserCreateModel model);
	}
}
