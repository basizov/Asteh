using Asteh.Core.Helpers;
using Asteh.Core.Models;
using Asteh.Core.Services.Users;
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
	public class UserServiceTests
	{
		private readonly IMapper _mapper;
		private readonly IUserService _sut;
		private readonly IUnitOfWork _unitOfWork = Substitute.For<IUnitOfWork>();

		public UserServiceTests()
		{
			if (_mapper == null)
			{
				var mappingConfig = new MapperConfiguration(
					mc => mc.AddProfile(new MappingProfile()));
				var mapper = mappingConfig.CreateMapper();
				_mapper = mapper;
			}
			_sut = new UserService(_mapper, _unitOfWork);
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

		//[Fact]
		//public async Task GivenNewUser_WhenCreateUserAsyncIsCalled_ThenReturnFullUsers()
		//{
		//	// Arrange
		//	const int COUNT = 10;
		//	var newUser = new Fixture()
		//		.Create<UserEntity>();
		//	var filteredUsersResult = new Fixture()
		//		.CreateMany<UserEntity>(COUNT)
		//		.ToList();

		//	_unitOfWork.UserRepository
		//		.Create(newUser)
		//		.Returns(filteredUsersResult);
		//	// Act
		//	var filteredUsers = await _sut.FindUsersAsync(null!);

		//	// Assert
		//	filteredUsers.Should().BeEquivalentTo(filteredUsersResult);
		//}

		#endregion

		#region UpdateUserAsync Tests

		#endregion

		#region DeleteUserAsync Tests

		#endregion
	}
}
