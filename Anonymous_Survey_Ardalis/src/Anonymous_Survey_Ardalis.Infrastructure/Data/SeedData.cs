using Anonymous_Survey_Ardalis.Core.AdminAggregate;
using Anonymous_Survey_Ardalis.Core.ContributorAggregate;
using Anonymous_Survey_Ardalis.Core.DepartmentAggregate;
using Anonymous_Survey_Ardalis.Core.SubjectAggregate;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Anonymous_Survey_Ardalis.Infrastructure.Data;

public  class SeedData
{
public static async Task Initialize(IServiceProvider serviceProvider, bool isDevelopment)
{
    using var scope = serviceProvider.CreateScope();
    var services = scope.ServiceProvider;
    var dbContext = services.GetRequiredService<AppDbContext>();
    var logger = services.GetRequiredService<ILogger<SeedData>>();
    var passwordHasher = services.GetRequiredService<IPasswordHasher<Admin>>();

    try
    {
        logger.LogInformation("Checking if admins exist in the database");
        
        if (!await dbContext.Admins.AnyAsync())
        {
            logger.LogInformation("No admins found. Creating a default SuperAdmin.");
            
            var department = await EnsureDepartmentExists(dbContext);
            var subject = await EnsureSubjectExists(dbContext, department.Id);
            
            var superAdmin = new Admin("SuperAdmin", "admin@example.com", subject.Id, AdminRole.SuperAdmin)
            {
                DepartmentId = department.Id,
                CreatedAt = DateTime.UtcNow
            };
            
            superAdmin.PasswordHash = passwordHasher.HashPassword(superAdmin, "Admin123!");
            
            dbContext.Admins.Add(superAdmin);
            await dbContext.SaveChangesAsync();
            
            logger.LogInformation("SuperAdmin created successfully");
        }
        else
        {
            logger.LogInformation("Admins already exist in the database");
        }
    }
    catch (Exception ex)
    {
        logger.LogError(ex, "An error occurred while seeding the database for admins");
    }
}

// Helper methods to ensure department and subject exist
private static async Task<Department> EnsureDepartmentExists(AppDbContext dbContext)
{
    var department = await dbContext.Departments.FirstOrDefaultAsync();
    if (department == null)
    {
        department = new Department ("Default Department");
        dbContext.Departments.Add(department);
        await dbContext.SaveChangesAsync();
    }
    return department;
}

private static async Task<Subject> EnsureSubjectExists(AppDbContext dbContext, int departmentId)
{
    var subject = await dbContext.Subjects.FirstOrDefaultAsync();
    if (subject == null)
    {
        subject = new Subject("Default Subject", departmentId);
        dbContext.Subjects.Add(subject);
        await dbContext.SaveChangesAsync();
    }
    return subject;
}
  public static readonly Contributor Contributor1 = new("Ardalis");
  public static readonly Contributor Contributor2 = new("Snowfrog");

  public static async Task InitializeAsync(AppDbContext dbContext)
  {
    if (await dbContext.Contributors.AnyAsync())
    {
      return; // DB has been seeded
    }

    await PopulateTestDataAsync(dbContext);
  }

  public static async Task PopulateTestDataAsync(AppDbContext dbContext)
  {
    dbContext.Contributors.AddRange(Contributor1, Contributor2);
    await dbContext.SaveChangesAsync();
  }
}
