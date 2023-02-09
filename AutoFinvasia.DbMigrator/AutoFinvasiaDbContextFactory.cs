using Microsoft.EntityFrameworkCore.Design;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;

namespace AutoFinvasia.DbMigrator
{
    public class AutoFinvasiaDbContextFactory : IDesignTimeDbContextFactory<AutoFinvasiaDbContext>
    {
        public AutoFinvasiaDbContext CreateDbContext(string[] args)
        {
            var builder = new DbContextOptionsBuilder<AutoFinvasiaDbContext>()
                .UseSqlServer(args[0],
                mssql =>
                {
                    mssql.MigrationsAssembly(typeof(AutoFinvasiaDbContextFactory).GetTypeInfo().Assembly.GetName().Name);
                    mssql.MigrationsHistoryTable("__AutoFinvasia_Migrations");
                });

            return new AutoFinvasiaDbContext(builder.Options);
        }
    }
}
