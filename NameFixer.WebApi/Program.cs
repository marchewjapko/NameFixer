using NameFixer.Infrastructure;
using NameFixer.WebApi.Configurations;
using NameFixer.WebApi.Services;

var builder = WebApplication.CreateBuilder(args);

builder
    .Services.AddGrpc()
    .AddJsonTranscoding();

builder.Services.AddOptionsConfigurations(builder.Configuration);
builder.Services.AddServiceConfiguration();

var app = builder.Build();

await app.Services.InitializeRepositories();

app.MapGrpcService<FirstNameService>();
app.MapGrpcService<SecondNameService>();
app.MapGrpcService<LastNameService>();

app.UseSwagger();

app.UseSwaggerUI(c => { c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1"); });

app.Run();