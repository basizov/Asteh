using Asteh.Domain.DataProvider;
using Asteh.Domain.Exceptions;
using Asteh.Domain.Repositories.Users;
using Asteh.Domain.Repositories.UserTypes;
using Microsoft.EntityFrameworkCore;

namespace Asteh.Domain.Repositories.Base
{
	internal class UnitOfWork : IUnitOfWork
	{
		private readonly ApplicationDbContext _applicationDbContext;
		private IUserRepository? _userRepository;
		private IUserTypeRepository? _userTypeRepository;

		public UnitOfWork(ApplicationDbContext applicationDbContext)
		{
			_applicationDbContext = applicationDbContext;
		}

		public IUserRepository UserRepository =>
			_userRepository ??= new UserRepository(_applicationDbContext);

		public IUserTypeRepository UserTypeRepository =>
			_userTypeRepository ??= new UserTypeRepository(_applicationDbContext);

		public async Task SaveChangesAsync(CancellationToken cancellationToken)
		{
			try
			{
				var	saveResult = await _applicationDbContext.SaveChangesAsync(cancellationToken);
				if (saveResult <= 0)
				{
					throw new DbWorkException("Couldn't save data to the database");
				}
			}
			catch (DbUpdateException ex)
			{
				throw new DbWorkException("Exception is occurred during updating the database", ex);
			}
		}
	}
}
