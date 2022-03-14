using Asteh.Core.Helpers;
using Asteh.Core.Models;
using Asteh.Domain.Entities;
using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;

namespace Asteh.Api.IntegrationTests
{
	public class UserControllerTests : BaseControllerTests
	{
		public UserControllerTests() : base("users")
		{
		}

		[Fact]
		public async Task GivenNothing_WhenGetUsersAsyncIsCalled_ThenReturnSeedUsers()
		{
			// Arrange

			// Act
			var usersResponse = await Client.GetAsync(Routes.GetUsers);
			var	content = await usersResponse.Content.ReadAsAsync<IEnumerable<UserModel>>();

			// Assert
			usersResponse.StatusCode.Should().Be(HttpStatusCode.OK);
			content.Should().NotBeEmpty();
		}

		[Fact]
		public async Task GivenUserIdentifier_WhenGetUserByIdAsyncIsCalled_ThenReturnUser()
		{
			// Arrange
			var rnd = new Random();
			var usersResponse = await Client.GetAsync(Routes.GetUsers);
			var contentUsers = await usersResponse.Content.ReadAsAsync<IEnumerable<UserModel>>();
			var user = contentUsers.ElementAt(rnd.Next(0, contentUsers.Count()));

			// Act
			var userByIdResponse = await Client
				.GetAsync(Routes.GetUserById.Replace("{userId}", user!.Id.ToString()));
			var	content = await userByIdResponse.Content.ReadAsAsync<UserModel>();

			// Assert
			userByIdResponse.StatusCode.Should().Be(HttpStatusCode.OK);
			content.Id.Should().Be(user!.Id);
			content.Should().BeEquivalentTo(user);
		}

		[Theory]
		[InlineData(1000)]
		[InlineData(3000)]
		[InlineData(4000)]
		public async Task GivenUnExistedUserIdentifier_WhenGetUserByIdAsyncIsCalled_ThenBadRequestWithMessage(int id)
		{
			// Arrange

			// Act
			var userByIdResponse = await Client.GetAsync(Routes.GetUserById.Replace("{userId}", id.ToString()));
			var userByIdErrorMessage = await userByIdResponse.Content
				.ReadAsAsync<ApplicationError>();

			// Assert
			userByIdResponse.StatusCode.Should().Be(HttpStatusCode.NotFound);
			userByIdErrorMessage.Message.Should()
				.Be($"Invalid user identifier {id}");
		}

		[Fact]
		public async Task GivenEmptyFilter_WhenFindUsersAsyncIsCalled_ThenAllUsers()
		{
			// Arrange
			var usersResponse = await Client.GetAsync(Routes.GetUsers);
			var contentUsers = await usersResponse.Content.ReadAsAsync<IEnumerable<UserModel>>();

			// Act
			var userFilterResponse = await Client.GetAsync(Routes.GetUserFilter);
			var contentFilterUsers = await userFilterResponse.Content.ReadAsAsync<IEnumerable<UserModel>>();

			// Assert
			userFilterResponse.StatusCode.Should().Be(HttpStatusCode.OK);
			contentUsers.Should().BeEquivalentTo(contentFilterUsers);
		}

		[Fact]
		public async Task GivenNotValidDateFilter_WhenFindUsersAsyncIsCalled_ThenThrowBadRequestWithMessages()
		{
			// Arrange
			var invalidBeginDate = "dasdasd";
			var invalidEndDate = "dasdasd";

			// Act
			var invalidBeginDateResponse = await Client
				.GetAsync($"{Routes.GetUserFilter}?beginDate={invalidBeginDate}");
			var invalidEndDateResponse = await Client
				.GetAsync($"{Routes.GetUserFilter}?endDate={invalidEndDate}");

			var beginDateErrorMessage = await invalidBeginDateResponse.Content
				.ReadAsAsync<ApplicationError>();
			var endDateErrorMessage = await invalidEndDateResponse.Content
				.ReadAsAsync<ApplicationError>();

			// Assert
			invalidBeginDateResponse.StatusCode.Should().Be(HttpStatusCode.BadRequest);
			invalidEndDateResponse.StatusCode.Should().Be(HttpStatusCode.BadRequest);
			beginDateErrorMessage.Message.Should()
				.Be($"Uncorrect range date: {invalidBeginDate} - ");
			endDateErrorMessage.Message.Should()
				.Be($"Uncorrect range date:  - {invalidEndDate}");
		}
	}
}
