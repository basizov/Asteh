using Asteh.Core.Models;
using Asteh.Domain.Repositories.Base;
using AutoMapper;

namespace Asteh.Core.Providers.UserTypes
{
	public class UserTypeProvider : IUserTypeProvider<UserTypeProvider>
	{
		private readonly IMapper _mapper;
		private readonly IUnitOfWork _unitOfWork;

		public UserTypeProvider(IMapper mapper, IUnitOfWork unitOfWork)
		{
			_mapper = mapper;
			_unitOfWork = unitOfWork;
		}

		public async Task<IEnumerable<UserTypeModel>> GetUserTypesAsync(
			CancellationToken cancellationToken = default)
		{
			var userTypes = await _unitOfWork.UserTypeRepository
				.GetAllAsync(cancellationToken);
			return _mapper.Map<IEnumerable<UserTypeModel>>(userTypes);
		}
	}
}
