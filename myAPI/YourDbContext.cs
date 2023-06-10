using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using System.Collections.Generic;
//using System.Data.Entity.Migrations;

namespace myAPI
{
    public class YourDbContext : DbContext
    {
        public YourDbContext(DbContextOptions<YourDbContext> options) : base(options)
        {
        }

        // Define your entity sets and DbSet properties here

        public DbSet<Pet> Pets { get; set; }
    }

    public class YourDbContextFactory : IDesignTimeDbContextFactory<YourDbContext>
    {
        public YourDbContext CreateDbContext(string[] args)
        {
            // Build the configuration
            IConfiguration configuration = new ConfigurationBuilder()
                .SetBasePath(AppContext.BaseDirectory)
                .AddEnvironmentVariables()//.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .Build();

            // Retrieve the connection string from the configuration
            string connectionString = configuration["CONNECTION_STRING"];// configuration.GetConnectionString("YourConnectionStringKey");

            // Create the DbContextOptions
            var optionsBuilder = new DbContextOptionsBuilder<YourDbContext>();
            optionsBuilder.UseNpgsql(connectionString);

            return new YourDbContext(optionsBuilder.Options);
        }
    }


}
