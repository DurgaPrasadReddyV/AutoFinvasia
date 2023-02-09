using AutoFinvasia;
using AutoFinvasia.Clients;
using AutoFinvasia.Clients.Models.Converters;
using AutoFinvasia.Options;
using Microsoft.EntityFrameworkCore;
using Serilog;
using System;
using System.Text.Json.Serialization;

Log.Logger = new LoggerConfiguration().WriteTo.Console().CreateLogger();
Log.Information("Server Booting Up...");
try
{
    var builder = WebApplication.CreateBuilder(args);

    builder.Host.ConfigureAppConfiguration((context, config) =>
    {
        var env = context.HostingEnvironment;
        config.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
            .AddEnvironmentVariables();
    });

    builder.Host.UseSerilog((_, config) =>
    {
        config.ReadFrom.Configuration(builder.Configuration);
    });

    // Add services to the container.
    builder.Services.AddDbContext<AutoFinvasiaDbContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("AutoFinvasiaDbConnection")));
    builder.Services.Configure<FinvasiaApiOptions>(builder.Configuration.GetSection(FinvasiaApiOptions.FinvasiaApi));
    builder.Services.Configure<FinvasiaSocketOptions>(builder.Configuration.GetSection(FinvasiaSocketOptions.FinvasiaSocket));
    builder.Services.AddHttpClient<FinvasiaApiClient>();
    builder.Services.AddSingleton<FinvasiaSocketClient>();
    builder.Services.AddControllers().AddJsonOptions(opts =>
    {
        opts.JsonSerializerOptions.Converters.Add(new ExarrConverter());
        opts.JsonSerializerOptions.Converters.Add(new InstnameConverter());
        opts.JsonSerializerOptions.Converters.Add(new DateOnlyConverter());
        opts.JsonSerializerOptions.Converters.Add(new IsoDateTimeOffsetConverter());
        opts.JsonSerializerOptions.Converters.Add(new ParseStringConverter());
        opts.JsonSerializerOptions.Converters.Add(new TimeOnlyConverter());
    });
    // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen();

    var app = builder.Build();

    // Configure the HTTP request pipeline.
    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI();
    }

    app.UseHttpsRedirection();

    app.UseAuthorization();

    app.MapControllers();

    app.Run();
}
catch (Exception ex) when (!ex.GetType().Name.Equals("StopTheHostException", StringComparison.Ordinal))
{
    Log.Fatal(ex, "Unhandled exception");
}
finally
{
    Log.Information("Server Shutting down...");
    Log.CloseAndFlush();
}

