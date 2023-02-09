using AutoFinvasia.Entities;
using Microsoft.EntityFrameworkCore;
using System.Security.Principal;

namespace AutoFinvasia
{
    public class AutoFinvasiaDbContext : DbContext
    {
        public AutoFinvasiaDbContext(DbContextOptions<AutoFinvasiaDbContext> options) : base(options)
        {
        }

        public DbSet<FinvasiaCredentials> FinvasiaCredentials => Set<FinvasiaCredentials>();

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.EnableSensitiveDataLogging();
        }
    }
}
