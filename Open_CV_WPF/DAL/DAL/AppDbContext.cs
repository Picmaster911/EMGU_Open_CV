using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace DAL
{
    public class AppDbContext : DbContext
    {
        public DbSet <MyAppSeting> MyAppSetings { get; set; }
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {

        }
    }
}


 //   protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
  //  {
   //     optionsBuilder.UseSqlite("Data Source=helloapp.db");
  //  }
