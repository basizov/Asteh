using Asteh.Core.Helpers;
using Asteh.Core.Models;
using Asteh.Core.Models.RequestModels;
using Asteh.Core.Services.Authorize;
using Asteh.Domain.Entities;
using Asteh.Domain.Repositories.Base;
using AutoFixture;
using AutoMapper;
using FluentAssertions;
using NSubstitute;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Xunit;

namespace Asteh.Core.UnitTests
{
	public class AuthorizeServiceTests
	{
		private readonly IMapper _mapper;
		private readonly IAuthorizeService<AuthorizeService> _sut;
		private readonly IUnitOfWork _unitOfWork = Substitute.For<IUnitOfWork>();

		public AuthorizeServiceTests()
		{
			if (_mapper == null)
			{
				var mappingConfig = new MapperConfiguration(
					mc => mc.AddProfile(new MappingProfile()));
				var mapper = mappingConfig.CreateMapper();
				_mapper = mapper;
			}
			_sut = new AuthorizeService(_mapper, _unitOfWork);
		}

		#region AuthorizeUserAsync Tests

		[Fact]
		public async Task GivenValidData_WhenAuthorizeUserAsyncIsCalled_ThenFullSuccessInfo()
		{
			// Arrange
			var authorizeUser = new Fixture()
				.Create<AuthorizeModel>();
			var userResult = new Fixture()
				.Build<UserEntity>()
				.With(d => d.Password, authorizeUser.Password)
				.Create();
			var usersResult = new Fixture()
				.CreateMany<UserEntity>()
				.ToList();
			var userTypesResult = new Fixture()
				.CreateMany<UserTypeEntity>()
				.ToList();

			_unitOfWork.UserRepository
				.SingleOrDefaultAsync(Arg.Any<Expression<Func<UserEntity, bool>>>())
				.Returns(userResult);
			_unitOfWork.UserRepository
				.GetAllAsync()
				.Returns(usersResult);
			_unitOfWork.UserTypeRepository
				.GetAllAsync()
				.Returns(userTypesResult);

			var userModelsResult = _mapper
				.Map<IEnumerable<UserModel>>(usersResult);
			var userTypeModelsResult = _mapper
				.Map<IEnumerable<UserTypeModel>>(userTypesResult);
			// Act
			var fullInfo = await _sut.AuthorizeUserAsync(authorizeUser!);

			// Assert
			fullInfo.Should().NotBeNull();
			fullInfo!.UserId.Should().Be(userResult.Id);
			fullInfo!.Users.Should().BeEquivalentTo(userModelsResult);
			fullInfo!.UserTypes.Should().BeEquivalentTo(userTypeModelsResult);
		}

		[Fact]
		public async Task GivenInValidPassword_WhenAuthorizeUserAsyncIsCalled_ThenShouldReturnNull()
		{
			// Arrange
			var authorizeUser = new Fixture()
				.Create<AuthorizeModel>();
			var userResult = new Fixture()
				.Create<UserEntity>();

			_unitOfWork.UserRepository
				.SingleOrDefaultAsync(Arg.Any<Expression<Func<UserEntity, bool>>>())
				.Returns(userResult);
			// Act
			var fullInfo = await _sut.AuthorizeUserAsync(authorizeUser!);

			// Assert
			fullInfo.Should().BeNull();
		}

		[Fact]
		public async Task GivenInValidLogin_WhenAuthorizeUserAsyncIsCalled_ThenShouldReturnNull()
		{
			// Arrange
			var authorizeUser = new Fixture()
				.Create<AuthorizeModel>();
			var userResult = new Fixture()
				.Create<UserEntity>();

			_unitOfWork.UserRepository
				.SingleOrDefaultAsync(Arg.Any<Expression<Func<UserEntity, bool>>>())
				.Returns(null as UserEntity);
			// Act
			var fullInfo = await _sut.AuthorizeUserAsync(authorizeUser!);

			// Assert
			fullInfo.Should().BeNull();
		}

		#endregion

		#region GetFullInfoModelToAuthorizeUser Tests

		[Fact]
		public async Task GivenValidData_GetFullInfoModelToAuthorizeUserIsCalled_ThenFullSuccessInfo()
		{
			// Arrange
			var usersResult = new Fixture()
				.CreateMany<UserEntity>()
				.ToList();
			var userTypesResult = new Fixture()
				.CreateMany<UserTypeEntity>()
				.ToList();

			_unitOfWork.UserRepository
				.GetAllAsync()
				.Returns(usersResult);
			_unitOfWork.UserTypeRepository
				.GetAllAsync()
				.Returns(userTypesResult);

			var userModelsResult = _mapper
				.Map<IEnumerable<UserModel>>(usersResult);
			var userTypeModelsResult = _mapper
				.Map<IEnumerable<UserTypeModel>>(userTypesResult);
			// Act
			var fullInfo = await _sut.GetFullInfoModelToAuthorizeUser();

			// Assert
			fullInfo.Users.Should().BeEquivalentTo(userModelsResult);
			fullInfo.UserTypes.Should().BeEquivalentTo(userTypeModelsResult);
		}

		#endregion
	}
}
