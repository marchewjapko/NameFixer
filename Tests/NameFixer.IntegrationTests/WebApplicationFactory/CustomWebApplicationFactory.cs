using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Configuration;

namespace NameFixer.IntegrationTests.WebApplicationFactory;

internal class CustomWebApplicationFactory : WebApplicationFactory<Program>
{
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        var projectDir = Directory.GetCurrentDirectory();
        var configPath = Path.Combine(projectDir, "WebApplicationFactory", "appsettings.IntegrationTests.json");

        builder.ConfigureAppConfiguration(configurationBuilder => configurationBuilder.AddJsonFile(configPath));

        base.ConfigureWebHost(builder);
    }
}