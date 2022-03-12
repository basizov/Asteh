using Asteh.Core.Models;
using Asteh.Core.Models.RequestModels;
using Asteh.Domain.Repositories.Base;
using AutoMapper;

namespace Asteh.Core.Services.Authorize
{
	public class AuthorizeService : IAuthorizeService<AuthorizeService>
	{
		private readonly IMapper _mapper;
		private readonly IUnitOfWork _unitOfWork;

		public AuthorizeService(IMapper mapper, IUnitOfWork unitOfWork)
		{
			_mapper = mapper;
			_unitOfWork = unitOfWork;
		}

		public async Task<FullInfoModel?> AuthorizeUserAsync(
			AuthorizeModel authorizeModel,
			CancellationToken cancellationToken = default)
		{
			var user = await _unitOfWork.UserRepository
				.SingleOrDefaultAsync(d =>
					d.Login.Equals(authorizeModel.Login) &&
					d.Password.Equals(authorizeModel.Password),
					cancellationToken);
			if (user is null)
			{
				return null;
			}

			var (users, userTypes) = await GetUserAndUserTypesAsync(cancellationToken);
			return new()
			{
				UserId = user.Id,
				IsAccessEnabled = user.Type?.AllowEdit ?? false,
				Users = users,
				UserTypes = userTypes
			};
		}

		public async Task<FullInfoModel> GetFullInfoModelToAuthorizeUser(
			CancellationToken cancellationToken = default)
		{
			var (users, userTypes) = await GetUserAndUserTypesAsync(cancellationToken);
			return new()
			{
				UserId = 0,
				IsAccessEnabled = false,
				Users = users,
				UserTypes = userTypes
			};
		}

		private async Task<(IEnumerable<UserModel> models,
			IEnumerable<UserTypeModel> userTypes)> GetUserAndUserTypesAsync(
			CancellationToken cancellationToken)
		{
			var	userEntities = await  _unitOfWork.UserRepository
				.GetAllAsync(cancellationToken);
			var userTypeEntities = await _unitOfWork.UserTypeRepository
				.GetAllAsync(cancellationToken);

			var userModels = _mapper.Map<IEnumerable<UserModel>>(userEntities);
			var userTypeModels = _mapper.Map<IEnumerable<UserTypeModel>>(userTypeEntities);
			return (userModels, userTypeModels);
		}
	}
}
