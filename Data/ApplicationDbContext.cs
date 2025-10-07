using Microsoft.EntityFrameworkCore;
using MyBffProject.Models;
using System.Collections.Generic;

namespace MyBffProject.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Team> Teams { get; set; }
    }
}
