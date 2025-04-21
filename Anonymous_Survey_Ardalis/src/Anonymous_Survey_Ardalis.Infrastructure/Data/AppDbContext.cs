using System.Reflection;
using Anonymous_Survey_Ardalis.Core.AdminAggregate;
using Anonymous_Survey_Ardalis.Core.CommentAggregate;
using Anonymous_Survey_Ardalis.Core.ContributorAggregate;
using Anonymous_Survey_Ardalis.Core.DepartmentAggregate;
using Anonymous_Survey_Ardalis.Core.SubjectAggregate;
using Ardalis.SharedKernel;
using Microsoft.EntityFrameworkCore;
using File = Anonymous_Survey_Ardalis.Core.CommentAggregate.File;

namespace Anonymous_Survey_Ardalis.Infrastructure.Data;

public class AppDbContext(
  DbContextOptions<AppDbContext> options,
  IDomainEventDispatcher? dispatcher) : DbContext(options)
{
  private readonly IDomainEventDispatcher? _dispatcher = dispatcher;

  public DbSet<Contributor> Contributors => Set<Contributor>();
  public DbSet<Comment> Comments => Set<Comment>();
  public DbSet<File> Files => Set<File>();
  public DbSet<Department> Departments => Set<Department>();
  public DbSet<Subject> Subjects => Set<Subject>();
  public DbSet<Admin> Admins => Set<Admin>();

  protected override void OnModelCreating(ModelBuilder modelBuilder)
  {
    base.OnModelCreating(modelBuilder);
    modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
  }

  public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = new())
  {
    var result = await base.SaveChangesAsync(cancellationToken).ConfigureAwait(false);

    // ignore events if no dispatcher provided
    if (_dispatcher == null)
    {
      return result;
    }

    // dispatch events only if save was successful
    var entitiesWithEvents = ChangeTracker.Entries<EntityBase>()
      .Select(e => e.Entity)
      .Where(e => e.DomainEvents.Any())
      .ToArray();

    await _dispatcher.DispatchAndClearEvents(entitiesWithEvents);

    return result;
  }

  public override int SaveChanges()
  {
    return SaveChangesAsync().GetAwaiter().GetResult();
  }
}
