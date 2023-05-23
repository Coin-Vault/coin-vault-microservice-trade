using Microsoft.EntityFrameworkCore;
using TradingService.Models;

namespace TradingService.Data
{
    public static class PrepDb
    {
        public static void PrepPopulation(IApplicationBuilder app, bool isProd)
        {
            using(var serviceScope = app.ApplicationServices.CreateScope()) 
            {
                SeedData(serviceScope.ServiceProvider.GetService<AppDbContext>(), isProd);
            }
        }

        private static void SeedData(AppDbContext context, bool isProd)
        {
            if(isProd)
            {
                try
                {
                    Console.WriteLine("Attampting to apply migrations");
                    context.Database.Migrate();
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Could not run migrations: {ex.Message}");
                }
            }

            if(!context.Trades.Any()) 
            {
                Console.WriteLine("Seeding Data..."); 

                context.Trades.AddRange(
                    new Trade() 
                    {
                        Name = "BITCOIN", 
                        UserId = "google-oauth2|107328215575499709402",
                        Amount = (decimal)5.0 
                    },
                    new Trade()
                    {
                        Name = "BITCOIN",
                        UserId = "google-oauth2|107328215575499709402", 
                        Amount = (decimal)10.0 
                    },
                    new Trade() 
                    {
                        Name = "BITCOIN", 
                        UserId = "google-oauth2|107328215575499709402",
                        Amount = (decimal)15.5 
                    }
                );

                context.SaveChanges();
            }
            else 
            {
                Console.WriteLine("Already Data (Trades) In the Database..."); 
            }
        }
    }
}