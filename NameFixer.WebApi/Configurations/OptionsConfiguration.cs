using System.ComponentModel.DataAnnotations;
using NameFixer.UseCases.Options;

namespace NameFixer.WebApi.Configurations;

public static class OptionsConfiguration
{
    public static void AddOptionsConfigurations(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<RepositoryOptions>(
            RepositoryOptions.FirstName,
            configuration
                .GetSection($"DatasetsOptions:{RepositoryOptions.FirstName}")
                .ValidateOptions<RepositoryOptions>());

        services.Configure<RepositoryOptions>(
            RepositoryOptions.SecondName,
            configuration
                .GetSection($"DatasetsOptions:{RepositoryOptions.SecondName}")
                .ValidateOptions<RepositoryOptions>());

        services.Configure<RepositoryOptions>(
            RepositoryOptions.LastName,
            configuration
                .GetSection($"DatasetsOptions:{RepositoryOptions.LastName}")
                .ValidateOptions<RepositoryOptions>());
    }

    private static IConfigurationSection ValidateOptions<T>(this IConfigurationSection configuration)
    {
        var options = configuration.Get<T>();

        if (options is null)
            throw new ValidationException(
                $"Unable to map configuration section {configuration.Path} to {typeof(T).FullName}");

        var validationResults = new List<ValidationResult>();
        var validationContext = new ValidationContext(options);

        if (Validator.TryValidateObject(
                options,
                validationContext,
                validationResults,
                true)) return configuration;

        var errorMessages = string.Join("; ", validationResults.Select(v => v.ErrorMessage));

        throw new ValidationException($"Options validation for section '{configuration.Path}' failed: {errorMessages}");
    }
}