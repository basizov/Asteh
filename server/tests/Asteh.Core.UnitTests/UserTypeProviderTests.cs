using Asteh.Core.Helpers;
using Asteh.Core.Providers.UserTypes;
using Asteh.Domain.Entities;
using Asteh.Domain.Repositories.Base;
using AutoFixture;
using AutoMapper;
using FluentAssertions;
using NSubstitute;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Asteh.Core.UnitTests
{
	public class UserTypeProviderTests
	{
		private readonly IMapper _mapper;
		private readonly IUserTypeProvider<UserTypeProvider> _sut;
		private readonly IUnitOfWork _unitOfWork = Substitute.For<IUnitOfWork>();

		public UserTypeProviderTests()
		{
			if (_mapper == null)
			{
				var mappingConfig = new MapperConfiguration(
					mc => mc.AddProfile(new MappingProfile()));
				var mapper = mappingConfig.CreateMapper();
				_mapper = mapper;
			}
			_sut = new UserTypeProvider(_mapper, _unitOfWork);
		}

		[Theory]
		[InlineData(55)]
		[InlineData(14)]
		[InlineData(2)]
		[InlineData(102)]
		public async Task GivenUserTypesCount_WhenGetUserTypesAsync_ThenReturnCountUserTypes(int userTypesCount)
		{
			// Arrange
			var userTypesResult = new Fixture()
				.CreateMany<UserTypeEntity>(userTypesCount)
				.ToList();

			_unitOfWork.UserTypeRepository.GetAllAsync().Returns(userTypesResult);
			// Act
			var users = await _sut.GetUserTypesAsync();

			// Assert
			users.Should().HaveCount(userTypesCount);
		}
	}
}
