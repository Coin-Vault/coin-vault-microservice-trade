using TradingService.Models;

namespace TradingService.Data
{
    public static class PrepDb
    {
        public static void PrepPopulation(IApplicationBuilder app)
        {
            using(var serviceScope = app.ApplicationServices.CreateScope()) 
            {
                SeedData(serviceScope.ServiceProvider.GetService<AppDbContext>());
            }
        }

        private static void SeedData(AppDbContext context)
        {
            if(!context.Trades.Any()) 
            {
                Console.WriteLine("Seeding Data..."); 

                context.Trades.AddRange(
                    new Trade() 
                    {
                        Name = "TEST", 
                        Amount = "TEST", 
                        Price = "TEST"
                    },
                    new Trade()
                    {
                        Name = "TEST", 
                        Amount = "TEST", 
                        Price = "TEST"
                    },
                    new Trade() 
                    {
                        Name = "TEST", 
                        Amount = "TEST", 
                        Price = "TEST"
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