using System.Reflection;
using Anonymous_Survey_Ardalis.Core.CommentAggregate;
using Anonymous_Survey_Ardalis.Core.ContributorAggregate;
using Ardalis.SharedKernel;
using Microsoft.EntityFrameworkCore;

namespace Anonymous_Survey_Ardalis.Infrastructure.Data;

public class AppDbContext(
  DbContextOptions<AppDbContext> options,
  IDomainEventDispatcher? dispatcher) : DbContext(options)
{
  private readonly IDomainEventDispatcher? _dispatcher = dispatcher;

  public DbSet<Contributor> Contributors => Set<Contributor>();
  public DbSet<Comment> Comments => Set<Comment>();

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
