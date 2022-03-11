using Asteh.Core.Helpers;
using Asteh.Core.Models;
using Asteh.Core.Models.RequestModels;
using Asteh.Domain.Providers.Users;
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
	public class UserProviderTests
	{
		private readonly IMapper _mapper;
		private readonly IUserProvider<UserProvider> _sut;
		private readonly IUnitOfWork _unitOfWork = Substitute.For<IUnitOfWork>();

		public UserProviderTests()
		{
			if (_mapper == null)
			{
				var mappingConfig = new MapperConfiguration(
					mc => mc.AddProfile(new MappingProfile()));
				var mapper = mappingConfig.CreateMapper();
				_mapper = mapper;
			}
			_sut = new UserProvider(_mapper, _unitOfWork);
		}

		#region GetUsersAsync Tests

		[Theory]
		[InlineData(10)]
		[InlineData(5)]
		[InlineData(0)]
		[InlineData(100)]
		public async Task GivenUsersCount_WhenGetUsersAsyncIsCalled_ThenReturnCountUsers(int usersCount)
		{
			// Arrange
			var usersResult = new Fixture()
				.CreateMany<UserEntity>(usersCount)
				.ToList();

			_unitOfWork.UserRepository.GetAllAsync().Returns(usersResult);
			// Act
			var users = await _sut.GetUsersAsync();

			// Assert
			var a = users.Count();
			users.Should().HaveCount(usersCount);
		}

		[Fact]
		public async Task GivenDate_WhenGetUsersAsyncIsCalled_ThenReturnDatesStringsEquivalents()
		{
			// Arrange
			const int COUNT = 10;
			var usersResult = new Fixture()
				.CreateMany<UserEntity>(COUNT)
				.ToList();
			var usersDatesResult = usersResult.Select(u => u.LastVisitDate.ToString("dd.MM.yyyy"));

			_unitOfWork.UserRepository.GetAllAsync().Returns(usersResult);
			// Act
			var users = await _sut.GetUsersAsync();
			var usersDates = users.Select(u => u.LastVisitDate);

			// Assert
			usersDates.Should().BeEquivalentTo(usersDatesResult);
		}

		[Fact]
		public async Task GivenTypes_WhenGetUsersAsyncIsCalled_ThenReturnTypesNames()
		{
			// Arrange
			const int COUNT = 10;
			var usersResult = new Fixture()
				.CreateMany<UserEntity>(COUNT)
				.ToList();
			var usersTypesResult = usersResult.Select(
				u => u.Type?.Name ?? throw new ArgumentNullException("Type couldn't be null"));

			_unitOfWork.UserRepository.GetAllAsync().Returns(usersResult);
			// Act
			var users = await _sut.GetUsersAsync();
			var usersTypes = users.Select(u => u.TypeName);

			// Assert
			usersTypes.Should().BeEquivalentTo(usersTypesResult);
		}

		#endregion

		#region GetUserByIdAsync Tests

		[Theory]
		[InlineData(43)]
		[InlineData(98)]
		[InlineData(6571)]
		[InlineData(334)]
		public async Task GivenInvalidId_WhenGetUserByIdAsyncIsCalled_ThenThrowArgumentException(int id)
		{
			// Arrange
			_unitOfWork.UserRepository
				.SingleOrDefaultAsync(Arg.Any<Expression<Func<UserEntity, bool>>>())
				.Returns(Task.FromResult<UserEntity?>(null));
			// Act
			var getByIdUser = async () => await _sut.GetUserByIdAsync(id);

			// Assert
			await getByIdUser.Should()
				.ThrowAsync<ArgumentException>()
				.WithMessage($"Invalid user identifier {id}");
		}

		[Theory]
		[InlineData(7)]
		[InlineData(265)]
		[InlineData(639)]
		[InlineData(8)]
		public async Task GivenValidId_WhenGetUserByIdAsyncIsCalled_ThenSuccessTakenUser(int id)
		{
			// Arrange
			var	userEntity = new Fixture()
				.Create<UserEntity>();
			var userModel = _mapper.Map<UserModel>(userEntity);

			_unitOfWork.UserRepository
				.SingleOrDefaultAsync(Arg.Any<Expression<Func<UserEntity, bool>>>())
				.Returns(Task.FromResult<UserEntity?>(userEntity));
			// Act
			var getByIdUser = await _sut.GetUserByIdAsync(id);

			// Assert
			getByIdUser.Should().BeEquivalentTo(userModel);
		}

		#endregion

		#region FindUsersAsync Tests

		[Fact]
		public async Task GivenFilter_WhenFindUsersAsyncIsCalled_ThenReturnFilteredUsers()
		{
			// Arrange
			const int COUNT = 10;
			var filter = new Fixture()
				.Build<FilterUserModel>()
				.With(d => d.BeginDate, DateTime.Now.AddYears(-1).ToString("dd.MM.yyyy"))
				.With(d => d.EndDate, DateTime.Now.AddYears(1).ToString("dd.MM.yyyy"))
				.Create();
			var filteredUsersResult = new Fixture()
				.CreateMany<UserEntity>(COUNT)
				.ToList();
			var filteredUsersModelsResult = _mapper
				.Map<IEnumerable<UserModel>>(filteredUsersResult);

			_unitOfWork.UserRepository
				.FindByAsync(Arg.Any<Expression<Func<UserEntity, bool>>>())
				.Returns(filteredUsersResult);
			// Act
			var filteredUsers = await _sut.FindUsersAsync(filter);

			// Assert
			filteredUsers.Should().BeEquivalentTo(filteredUsersModelsResult);
		}

		[Fact]
		public async Task GivenNullFilter_WhenFindUsersAsyncIsCalled_ThenReturnFullUsers()
		{
			// Arrange
			const int COUNT = 10;
			var filteredUsersResult = new Fixture()
				.CreateMany<UserEntity>(COUNT)
				.ToList();
			var filteredUsersModelsResult = _mapper
				.Map<IEnumerable<UserModel>>(filteredUsersResult);

			_unitOfWork.UserRepository
				.GetAllAsync()
				.Returns(filteredUsersResult);
			// Act
			var filteredUsers = await _sut.FindUsersAsync(null!);

			// Assert
			filteredUsers.Should().BeEquivalentTo(filteredUsersModelsResult);
		}

		[Fact]
		public async Task GivenEmptyFilter_WhenFindUsersAsyncIsCalled_ThenReturnFullUsers()
		{
			// Arrange
			const int COUNT = 10;
			var filter = new Fixture()
				.Build<FilterUserModel>()
				.With(u => u.Name, "")
				.With(u => u.TypeName, "")
				.With(u => u.BeginDate, "")
				.With(u => u.EndDate, "")
				.Create();
			var filteredUsersResult = new Fixture()
				.CreateMany<UserEntity>(COUNT)
				.ToList();
			var filteredUsersModelsResult = _mapper
				.Map<IEnumerable<UserModel>>(filteredUsersResult);

			_unitOfWork.UserRepository
				.GetAllAsync()
				.Returns(filteredUsersResult);
			// Act
			var filteredUsers = await _sut.FindUsersAsync(filter);

			// Assert
			filteredUsers.Should().BeEquivalentTo(filteredUsersModelsResult);
		}

		[Fact]
		public async Task GivenInValidFilter_WhenFindUsersAsyncIsCalled_ThenThrowArgumentException()
		{
			// Arrange
			var filter = new Fixture()
				.Create<FilterUserModel>();

			// Act
			var actFilteredUsers = async () => await _sut.FindUsersAsync(filter);

			// Assert
			await actFilteredUsers.Should()
				.ThrowAsync<ArgumentException>()
				.WithMessage($"Uncorrect range date: {filter.BeginDate} - {filter.EndDate}");
		}

		[Fact]
		public async Task GivenEmptyDateRange_WhenFindUsersAsyncIsCalled_ThenReturnFilteredWithAnotherFilters()
		{
			// Arrange
			const int COUNT = 10;
			var filter = new Fixture()
				.Build<FilterUserModel>()
				.With(u => u.BeginDate, "")
				.With(u => u.EndDate, "")
				.Create();
			var filteredUsersResult = new Fixture()
				.CreateMany<UserEntity>(COUNT)
				.ToList();
			var filteredUsersModelsResult = _mapper
				.Map<IEnumerable<UserModel>>(filteredUsersResult);

			_unitOfWork.UserRepository
				.FindByAsync(Arg.Any<Expression<Func<UserEntity, bool>>>())
				.Returns(filteredUsersResult);
			// Act
			var filteredUsers = await _sut.FindUsersAsync(filter);

			// Assert
			filteredUsers.Should().BeEquivalentTo(filteredUsersModelsResult);
		}

		#endregion

		#region CreateUserAsync Tests

		[Fact]
		public async Task GivenExistedLogin_WhenCreateUserAsyncIsCalled_ThenThrowArgumentException()
		{
			// Arrange
			var newUser = new Fixture()
				.Create<UserCreateModel>();

			_unitOfWork.UserRepository
				.AnyAsync(Arg.Any<Expression<Func<UserEntity, bool>>>())
				.Returns(true);
			// Act
			var filteredUsers = async () =>await _sut.CreateUserAsync(newUser);

			// Assert
			await filteredUsers.Should()
				.ThrowAsync<ArgumentException>()
				.WithMessage($"User with login: {newUser.Login} is exists");
		}

		[Fact]
		public async Task GivenInvalidTypeName_WhenCreateUserAsyncIsCalled_ThenThrowArgumentException()
		{
			// Arrange
			var newUser = new Fixture()
				.Create<UserCreateModel>();

			_unitOfWork.UserRepository
				.AnyAsync(Arg.Any<Expression<Func<UserEntity, bool>>>())
				.Returns(false);
			_unitOfWork.UserTypeRepository
				.SingleOrDefaultAsync(Arg.Any<Expression<Func<UserTypeEntity, bool>>>())
				.Returns(Task.FromResult<UserTypeEntity?>(null));
			// Act
			var filteredUsers = async () =>await _sut.CreateUserAsync(newUser);

			// Assert
			await filteredUsers.Should()
				.ThrowAsync<ArgumentException>()
				.WithMessage($"UserType with name: {newUser.TypeName} doesn't exists");
		}

		[Fact]
		public async Task GivenValidData_WhenCreateUserAsyncIsCalled_ThenWithoutReturning()
		{
			// Arrange
			var userEntity = new Fixture()
				.Create<UserEntity>();
			var newUser = new Fixture()
				.Create<UserCreateModel>();
			var userTypeEntity = new Fixture()
				.Create<UserTypeEntity>();

			_unitOfWork.UserRepository
				.AnyAsync(Arg.Any<Expression<Func<UserEntity, bool>>>())
				.Returns(false);
			_unitOfWork.UserRepository
				.Create(Arg.Any<UserEntity>())
				.Returns(userEntity);
			_unitOfWork.UserTypeRepository
				.SingleOrDefaultAsync(Arg.Any<Expression<Func<UserTypeEntity, bool>>>())
				.Returns(Task.FromResult<UserTypeEntity?>(userTypeEntity));
			// Act
			var filteredUsers = async () =>await _sut.CreateUserAsync(newUser);

			// Assert
			await filteredUsers.Should().NotThrowAsync();
		}

		#endregion

		#region UpdateUserAsync Tests

		[Theory]
		[InlineData(2)]
		[InlineData(12)]
		[InlineData(1)]
		[InlineData(34)]
		public async Task GivenInvalidId_WhenUpdateUserAsyncIsCalled_ThenThrowArgumentException(int id)
		{
			// Arrange
			var updatedUser = new Fixture()
				.Create<UserUpdateModel>();

			_unitOfWork.UserRepository
				.SingleOrDefaultAsync(Arg.Any<Expression<Func<UserEntity, bool>>>())
				.Returns(Task.FromResult<UserEntity?>(null));
			// Act
			var filteredUsers = async () => await _sut.UpdateUserAsync(id, updatedUser);

			// Assert
			await filteredUsers.Should()
				.ThrowAsync<ArgumentException>()
				.WithMessage($"User with id: {id} doesn't exists");
		}

		[Theory]
		[InlineData(6)]
		[InlineData(31)]
		[InlineData(76)]
		[InlineData(22)]
		public async Task GivenInvalidTypeName_WhenUpdateUserAsyncIsCalled_ThenThrowArgumentException(int id)
		{
			// Arrange
			var userEntity = new Fixture()
				.Create<UserEntity>();
			var updatedUser = new Fixture()
				.Create<UserUpdateModel>();

			_unitOfWork.UserRepository
				.SingleOrDefaultAsync(Arg.Any<Expression<Func<UserEntity, bool>>>())
				.Returns(Task.FromResult<UserEntity?>(userEntity));
			_unitOfWork.UserTypeRepository
				.SingleOrDefaultAsync(Arg.Any<Expression<Func<UserTypeEntity, bool>>>())
				.Returns(Task.FromResult<UserTypeEntity?>(null));
			// Act
			var filteredUsers = async () => await _sut.UpdateUserAsync(id, updatedUser);

			// Assert
			await filteredUsers.Should()
				.ThrowAsync<ArgumentException>()
				.WithMessage($"UserType with name: {updatedUser.TypeName} doesn't exists");
		}

		[Theory]
		[InlineData(4)]
		[InlineData(141)]
		[InlineData(7347)]
		[InlineData(122)]
		public async Task GivenInvalidDateTime_WhenUpdateUserAsyncIsCalled_ThenThrowArgumentException(int id)
		{
			// Arrange
			var userEntity = new Fixture()
				.Create<UserEntity>();
			var userTypeEntity = new Fixture()
				.Create<UserTypeEntity>();
			var updatedUser = new Fixture()
				.Create<UserUpdateModel>();

			_unitOfWork.UserRepository
				.SingleOrDefaultAsync(Arg.Any<Expression<Func<UserEntity, bool>>>())
				.Returns(Task.FromResult<UserEntity?>(userEntity));
			_unitOfWork.UserTypeRepository
				.SingleOrDefaultAsync(Arg.Any<Expression<Func<UserTypeEntity, bool>>>())
				.Returns(Task.FromResult<UserTypeEntity?>(userTypeEntity));
			// Act
			var filteredUsers = async () => await _sut.UpdateUserAsync(id, updatedUser);

			// Assert
			await filteredUsers.Should()
				.ThrowAsync<ArgumentException>()
				.WithMessage($"Invalid new lastVisitDate {updatedUser.LastVisitDate}");
		}

		[Theory]
		[InlineData(10)]
		[InlineData(2)]
		[InlineData(642)]
		[InlineData(12)]
		public async Task GivenValidData_WhenUpdateUserAsyncIsCalled_ThenWithoutReturning(int id)
		{
			// Arrange
			var lastVisitDate = DateTime.UtcNow.ToString("dd.MM.yyyy");
			var userEntity = new Fixture()
				.Create<UserEntity>();
			var userTypeEntity = new Fixture()
				.Create<UserTypeEntity>();
			var updatedUser = new Fixture()
				.Build<UserUpdateModel>()
				.With(d => d.LastVisitDate, lastVisitDate)
				.Create();

			_unitOfWork.UserRepository
				.SingleOrDefaultAsync(Arg.Any<Expression<Func<UserEntity, bool>>>())
				.Returns(Task.FromResult<UserEntity?>(userEntity));
			_unitOfWork.UserTypeRepository
				.SingleOrDefaultAsync(Arg.Any<Expression<Func<UserTypeEntity, bool>>>())
				.Returns(Task.FromResult<UserTypeEntity?>(userTypeEntity));
			// Act
			var filteredUsers = async () => await _sut.UpdateUserAsync(id, updatedUser);

			// Assert
			await filteredUsers.Should().NotThrowAsync();
		}

		#endregion

		#region DeleteUserAsync Tests

		[Theory]
		[InlineData(23)]
		[InlineData(942)]
		[InlineData(4)]
		[InlineData(25)]
		public async Task GivenInvalidId_WhenDeleteUserAsyncIsCalled_ThenThrowArgumentException(int id)
		{
			// Arrange
			_unitOfWork.UserRepository
				.SingleOrDefaultAsync(Arg.Any<Expression<Func<UserEntity, bool>>>())
				.Returns(Task.FromResult<UserEntity?>(null));
			// Act
			var filteredUsers = async () => await _sut.DeleteUserAsync(id);

			// Assert
			await filteredUsers.Should()
				.ThrowAsync<ArgumentException>()
				.WithMessage($"User with id: {id} doesn't exists");
		}

		[Theory]
		[InlineData(3)]
		[InlineData(422)]
		[InlineData(1)]
		[InlineData(3215)]
		public async Task GivenValidId_WhenDeleteUserAsyncIsCalled_ThenWithoutReturning(int id)
		{
			// Arrange
			var userEntity = new Fixture()
				.Create<UserEntity>();

			_unitOfWork.UserRepository
				.SingleOrDefaultAsync(Arg.Any<Expression<Func<UserEntity, bool>>>())
				.Returns(Task.FromResult<UserEntity?>(userEntity));
			// Act
			var filteredUsers = async () => await _sut.DeleteUserAsync(id);

			// Assert
			await filteredUsers.Should().NotThrowAsync();
		}

		#endregion
	}
}
