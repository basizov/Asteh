using Asteh.Api.Configuration;
using Asteh.Api.Middlewares;
using Asteh.Api.SeedProviders;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
var config = builder.Configuration;

builder.Services.AddControllers(opt => opt.SuppressAsyncSuffixInActionNames = false);

var openApiDescriptionSection = config.GetSection("OpenApiDescription");
var creatorContactsSection = config.GetSection("CreatorContacts");
builder.Services.AddOpenApiWithDescription(
    new OpenApiDescription(
        openApiDescriptionSection["Title"],
        openApiDescriptionSection["Description"],
        openApiDescriptionSection["Version"]),
    new CreatorContacts(
        creatorContactsSection["Email"],
        creatorContactsSection["Name"],
        creatorContactsSection["Url"]));

var dataSettings = new DataSettings(
    config.GetConnectionString("DatabaseConnection"),
    Path.Combine(Environment.CurrentDirectory, "Serializing"));
builder.Services.AddDomainSystem(dataSettings);


builder.Services.AddCors(o => o.AddPolicy("CorsPolicy", policy => policy
    .WithOrigins("http://localhost:3000")
    .AllowAnyHeader()
    .AllowAnyMethod()
    .AllowCredentials()
));

builder.Services.AddCoreSystem();

var app = builder.Build();
await app.SeedUsersAsync(dataSettings.FileSerializerString);

app.UseMiddleware<ErrorHandlerMiddleware>();
app.UseSwagger();
app.UseSwaggerUI();
app.UseCors("CorsPolicy");
app.MapControllers();

await app.RunAsync();

public partial class Program { }
