using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;

namespace PlatformService.Data
{
  public static class PrepDb
    {
      public static void PrepPopuration(IApplicationBuilder app, bool isProd)
      {
        using(var serviceScope = app.ApplicationServices.CreateScope())
        {
                SeedData(serviceScope.ServiceProvider.GetService<AppDbContext>(), isProd);
        }
      }

      private static void SeedData(AppDbContext context, bool isProd)
      {
            if (isProd)
            {
                Console.WriteLine("--> Attempting to apply migration...");
                try
                {
                    context.Database.Migrate();
                }
                catch (Exception e)
                {
                    Console.WriteLine($"--> Could not run migration {e.Message}");
                }
            }
            if (!context.Platforms.Any())
            {
                Console.WriteLine("--> Seeding Data...");
                context.Platforms.AddRange(
                    new Models.Platform() { Name="Dot Net", Publisher="Microsoft", Cost="Free"},
                    new Models.Platform() { Name = "SQL Server Express", Publisher = "Microsoft", Cost = "Free" },
                    new Models.Platform() { Name = "Docker", Publisher = "Docker", Cost = "Free" },
                    new Models.Platform() { Name = "Kubernetes", Publisher = "Cloud Native Computing Foundation", Cost = "Free" }
                );
                context.SaveChanges();
            } 
            else
            {
                Console.WriteLine("--> We already have data");
            }
      }

  }
}