using Asteh.Api.Configuration;
using Asteh.Api.Examples;
using Asteh.Api.Examples.Users;
using Swashbuckle.AspNetCore.Filters;

namespace Microsoft.Extensions.DependencyInjection
{
	public static class OpenApiExtension
	{
		public static IServiceCollection AddOpenApiWithDescription(
            this IServiceCollection services,
            OpenApiDescription openApiDescription,
            CreatorContacts createContacts)
		{
            return services
                .AddSwaggerGen(opt =>
                {
                    opt.SwaggerDoc("v1", new()
                    {
                        Title = openApiDescription.Title,
                        Version = openApiDescription.Version,
                        Description = openApiDescription.Description,
                        Contact = new()
                        {
                            Email = createContacts.Email,
                            Name = createContacts.Name,
                            Url = new(createContacts.Url)
                        }
                    });

                    var files = Directory.GetFiles(AppContext.BaseDirectory, "*.xml");
                    foreach (var file in files)
                    {
                        opt.IncludeXmlComments(file);
                    }

                    opt.UseInlineDefinitionsForEnums();

                    opt.ExampleFilters();
                })
                .AddSwaggerExamplesFromAssemblyOf<ErrorExample>()
                .AddSwaggerExamplesFromAssemblyOf<GetUserResponseExample>()
                .AddSwaggerExamplesFromAssemblyOf<GetUsersResponseExample>();
		}
	}
}
