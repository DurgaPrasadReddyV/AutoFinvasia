using AutoFinvasia;
using AutoFinvasia.DbMigrator;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;
using System.Linq;
using System.Reflection;

Log.Logger = new LoggerConfiguration().WriteTo.Console().CreateBootstrapLogger();
Log.Information("Migrator Running...");

try
{
    await CreateHostBuilder(args).RunConsoleAsync();
}
catch (Exception ex) when (!ex.GetType().Name.Equals("StopTheHostException", StringComparison.Ordinal))
{
    Log.Fatal(ex, "Unhandled exception");
}
finally
{
    Log.Information("Migrator Terminating...");
    Log.CloseAndFlush();
}

static IHostBuilder CreateHostBuilder(string[] args) =>
    Host.CreateDefaultBuilder(args)
    .ConfigureAppConfiguration((_, config) =>
    {
        config.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
        config.AddEnvironmentVariables();
    })
    .UseSerilog((hostContext, config) =>
    {
        config.ReadFrom.Configuration(hostContext.Configuration);
    })
    .ConfigureServices((hostContext, services) =>
    {
        services.AddDbContext<AutoFinvasiaDbContext>(options => options.UseSqlServer(args[0],
            mssql =>
            {
                mssql.MigrationsAssembly(typeof(AutoFinvasiaDbContextFactory).GetTypeInfo().Assembly.GetName().Name);
                mssql.MigrationsHistoryTable("__AutoFinvasia_Migrations");
            }));
        services.AddHostedService<DbMigratorHostedService>();
    });
