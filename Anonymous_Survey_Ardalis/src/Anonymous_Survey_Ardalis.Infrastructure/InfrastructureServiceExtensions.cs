using Anonymous_Survey_Ardalis.Core.Interfaces;
using Anonymous_Survey_Ardalis.Core.Services;
using Anonymous_Survey_Ardalis.Infrastructure.Data;
using Anonymous_Survey_Ardalis.Infrastructure.Data.Queries;
using Anonymous_Survey_Ardalis.UseCases.Contributors.List;
using Ardalis.GuardClauses;
using Ardalis.SharedKernel;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Anonymous_Survey_Ardalis.Infrastructure;

public static class InfrastructureServiceExtensions
{
  public static IServiceCollection AddInfrastructureServices(
    this IServiceCollection services,
    ConfigurationManager config,
    ILogger logger)
  {
    var connectionString = config.GetConnectionString("SqliteConnection");
    Guard.Against.Null(connectionString);
    services.AddDbContext<AppDbContext>(options =>
      options.UseSqlite(connectionString));

    services.AddScoped(typeof(IRepository<>), typeof(EfRepository<>))
      .AddScoped(typeof(IReadRepository<>), typeof(EfRepository<>))
      .AddScoped<IListContributorsQueryService, ListContributorsQueryService>()
      .AddScoped<IDeleteContributorService, DeleteContributorService>();


    logger.LogInformation("{Project} services registered", "Infrastructure");

    return services;
  }
}
