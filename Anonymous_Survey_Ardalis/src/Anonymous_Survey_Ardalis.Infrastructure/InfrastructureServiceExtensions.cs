using Anonymous_Survey_Ardalis.Core.Interfaces;
using Anonymous_Survey_Ardalis.Core.Services;
using Anonymous_Survey_Ardalis.Infrastructure.Data;
using Anonymous_Survey_Ardalis.Infrastructure.Data.Queries;
using Anonymous_Survey_Ardalis.Infrastructure.Email;
using Anonymous_Survey_Ardalis.UseCases.Admins.Queries.List;
using Anonymous_Survey_Ardalis.UseCases.Comments.Queries.List;
using Anonymous_Survey_Ardalis.UseCases.Contributors.List;
using Anonymous_Survey_Ardalis.UseCases.Departments.Queries.List;
using Anonymous_Survey_Ardalis.UseCases.Files.Commands.Download;
using Anonymous_Survey_Ardalis.UseCases.Subjects.Queries.List;
using Ardalis.GuardClauses;
using Ardalis.Result;
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
    var connectionString = config.GetConnectionString("DefaultConnection");
    Guard.Against.Null(connectionString);
    services.AddDbContext<AppDbContext>(options =>
      options.UseSqlServer(connectionString));


    services.AddScoped(typeof(IRepository<>), typeof(EfRepository<>))
      .AddScoped(typeof(IReadRepository<>), typeof(EfRepository<>))
      .AddScoped<IListContributorsQueryService, ListContributorsQueryService>()
      .AddScoped<IDeleteContributorService, DeleteContributorService>()
      .AddScoped<IListCommentQueryService, ListCommentQueryService>()
      .AddScoped<IListSubjectQueryService, ListSubjectsQueryService>()
      .AddScoped<IListDepartmentQueryService, ListDepartmentsQueryService>()
      .AddScoped<IListAdminsQueryService, ListAdminsQueryService>();



    
    services.AddEmailServices(config);

    logger.LogInformation("{Project} services registered", "Infrastructure");

    return services;
  }
  private static IServiceCollection AddEmailServices(this IServiceCollection services, IConfiguration configuration)
  {
    var mailConfig = new MailserverConfiguration();
    configuration.GetSection("EmailSettings").Bind(mailConfig);
    services.AddSingleton(mailConfig);

    services.AddScoped<IEmailSender, EmailSender>();

    return services;
  }
}
