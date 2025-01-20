using Google.Protobuf.Reflection;
using Google.Rpc;
using Microsoft.AspNetCore.Grpc.JsonTranscoding;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using NameFixer.Infrastructure.ExceptionHandling;
using NameFixer.WebApi.Configurations;
using NameFixer.WebApi.Services;

var builder = WebApplication.CreateBuilder(args);

builder
    .Services.AddGrpc(
        options =>
        {
            options.EnableDetailedErrors = true;
            options.Interceptors.Add<ExceptionHandlingInterceptor>();
        })
    .AddJsonTranscoding(
        options =>
        {
            options.JsonSettings = new GrpcJsonSettings
            {
                WriteIndented = true
            };

            options.TypeRegistry = TypeRegistry.FromMessages(ErrorInfo.Descriptor);
        });

builder.Services.AddGrpcReflection();

builder
    .Services.AddGrpcHealthChecks()
    .AddCheck("health", () => HealthCheckResult.Healthy());

builder.Services.AddOptionsConfigurations(builder.Configuration);
builder.Services.AddServiceConfiguration();

var app = builder.Build();

await app.Services.InitializeStores();

app.MapGrpcService<SuggestionService>();

app.UseSwagger();
app.UseSwaggerUI(c => { c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1"); });

app.UseHsts();

app.MapGrpcReflectionService();

app.MapHealthChecks("/_health");

app.MapGrpcHealthChecksService();

await app.RunAsync();