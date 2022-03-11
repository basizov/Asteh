using Asteh.Api.Configuration;
using Asteh.Api.SeedProviders;
using Asteh.Domain.DataProvider;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);
var config = builder.Configuration;

builder.Services.AddControllers();

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

builder.Services.AddCoreSystem();

var app = builder.Build();
await app.SeedUsersAsync(dataSettings.FileSerializerString);

app.UseSwagger();
app.UseSwaggerUI();
app.MapControllers();

await app.RunAsync();
