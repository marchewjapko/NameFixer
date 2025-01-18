using Microsoft.OpenApi.Models;
using NameFixer.Infrastructure;
using NameFixer.UseCases;

namespace NameFixer.WebApi.Configurations;

public static class ServiceConfiguration
{
    public static void AddServiceConfiguration(this IServiceCollection services)
    {
        services.AddUseCasesServices();
        services.AddInfrastructureServices();

        services.AddHealthChecks();

        services.AddGrpcSwagger();
        services.AddSwaggerGen(
            c =>
            {
                c.SwaggerDoc(
                    "v1",
                    new OpenApiInfo
                    {
                        Title = "NameFixer",
                        Description = """
                                      <h3> This is a gRPC web API that will suggest first, second and last names. </h3>
                                      <br />
                                      Names are suggested using the <a href="https://en.wikipedia.org/wiki/Levenshtein_distance">Levenshtein distance</a> algorithm.
                                      <br />
                                      Dictionary of names are provided by <a href="https://dane.gov.pl/en">dane.gov.pl</a>.
                                      <br />
                                      Links to the data sets:
                                      <ul>
                                        <li><a href="https://dane.gov.pl/pl/dataset/1501,lista-imion-wystepujacych-w-rejestrze-pesel">First and second names</a></li>
                                        <li><a href="https://dane.gov.pl/pl/dataset/568,nazwiska-wystepujace-w-rejestrze-pesel">Last names</a></li>
                                      </ul>
                                      """,
                        Version = "v1"
                    });
            });
    }
}