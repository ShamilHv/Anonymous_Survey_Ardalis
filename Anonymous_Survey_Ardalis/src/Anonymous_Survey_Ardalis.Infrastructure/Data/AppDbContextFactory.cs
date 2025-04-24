// using System;
// using Microsoft.EntityFrameworkCore;
// using Microsoft.EntityFrameworkCore.Design;
// using Microsoft.Extensions.Configuration;
//
// namespace Anonymous_Survey_Ardalis.Infrastructure.Data;
//
// public class AppDbContextFactory : IDesignTimeDbContextFactory<AppDbContext>
// {
//   public AppDbContext CreateDbContext(string[] args)
//   {
//     var configBuilder = new ConfigurationBuilder()
//       .AddJsonFile("appsettings.json", optional: false)
//       .AddJsonFile("appsettings.Development.json", optional: true)
//       .AddEnvironmentVariables();
//
//     var config = configBuilder.Build();
//     var connectionString = config.GetConnectionString("DefaultConnection");
//
//     if (string.IsNullOrEmpty(connectionString))
//     {
//       throw new InvalidOperationException(
//         "Could not find a connection string named 'DefaultConnection'.");
//     }
//
//     var optionsBuilder = new DbContextOptionsBuilder<AppDbContext>();
//     optionsBuilder.UseSqlServer(connectionString);
//
//     return new AppDbContext(optionsBuilder.Options, null);
//   }
// }


