using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace myAPI
{
    public class myContext : DbContext
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            //optionsBuilder.UseNpgsql("your-postgresql-connection-string");
            optionsBuilder.UseNpgsql("Server=localhost;Port=5432;Database=mydatabase;User Id=postgres;Password=password");
        }
        // Define DbSet properties for your entities
        public DbSet<Pet> Pets { get; set; }
    }
}
