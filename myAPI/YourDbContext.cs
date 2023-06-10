using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using System.Collections.Generic;
//using System.Data.Entity.Migrations;

namespace myAPI
{
    public class YourDbContext : DbContext
    {
        public YourDbContext(DbContextOptions<YourDbContext> options) : base(options){}

        // Define your entity sets and DbSet properties here
        public DbSet<Pet> Pets { get; set; }
    }
}
