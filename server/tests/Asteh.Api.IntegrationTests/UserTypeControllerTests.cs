using Asteh.Core.Helpers;
using Asteh.Core.Models;
using FluentAssertions;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;

namespace Asteh.Api.IntegrationTests
{
	public class UserTypeControllerTests : BaseControllerTests
	{
		[Fact]
		public async Task GivenNothing_WhenGetUserTypesAsyncIsCalled_ThenReturnSeedUserTypes()
		{
			// Arrange

			// Act
			var userTypesResponse = await Client.GetAsync(Routes.GetUserTypes);
			var	content = await userTypesResponse.Content.ReadAsAsync<IEnumerable<UserTypeModel>>();

			// Assert
			userTypesResponse.StatusCode.Should().Be(HttpStatusCode.OK);
			content.Should().NotBeEmpty();
		}
	}
}
