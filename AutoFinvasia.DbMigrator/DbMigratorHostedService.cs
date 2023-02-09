using AutoFinvasia.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace AutoFinvasia.DbMigrator
{
    public class DbMigratorHostedService : BackgroundService
    {
        private readonly IHostApplicationLifetime _hostApplicationLifetime;
        private readonly IConfiguration _configuration;
        private readonly IServiceProvider _serviceProvider;

        public DbMigratorHostedService(IHostApplicationLifetime hostApplicationLifetime, IConfiguration configuration, IServiceProvider serviceProvider)
        {
            _hostApplicationLifetime = hostApplicationLifetime;
            _configuration = configuration;
            _serviceProvider = serviceProvider;
        }

        protected override async Task ExecuteAsync(CancellationToken cancellationToken)
        {
            using (var migrationScope = _serviceProvider.GetRequiredService<IServiceScopeFactory>().CreateScope())
            {
                using (var context = migrationScope.ServiceProvider.GetRequiredService<AutoFinvasiaDbContext>())
                {
                    await context.Database.MigrateAsync();
                }
            }

            using (var seedScope = _serviceProvider.GetRequiredService<IServiceScopeFactory>().CreateScope())
            {
                var context = seedScope.ServiceProvider.GetRequiredService<AutoFinvasiaDbContext>();

                var testCredentials = new FinvasiaCredentials();
                var userId = "TEST13";
                testCredentials.UserID = userId;
                testCredentials.PasswordHash = GetHashSHA256("Imitation@4");
                testCredentials.IMEI = "ABC123";
                testCredentials.VendorCode = "API_USER";
                testCredentials.APIKeyHash = GetHashSHA256($"{userId}|ApiUAT02012022YHDUNFJKIL987JM");
                testCredentials.PAN = "ABCDE1234F";
                testCredentials.TOTPKey = "HHQNU56C6T3OE6P6OFALZZ244S62EGLU";

                context.Add(testCredentials);

                var testCredentials2 = new FinvasiaCredentials();
                var userId2 = "FA72952";
                testCredentials2.UserID = userId2;
                testCredentials2.PasswordHash = GetHashSHA256("Imitation@5");
                testCredentials2.IMEI = "abc1234";
                testCredentials2.VendorCode = "FA72952_U";
                testCredentials2.APIKeyHash = GetHashSHA256($"{userId2}|1e96d3b0f989614ea7d5892847350678");
                testCredentials2.TOTPKey = "B6Q46G55N732253HN2P55WP2HWHO76VS";
                testCredentials2.PAN = "AOJPV8213P";

                context.Add(testCredentials2);

                await context.SaveChangesAsync();
            }

            _hostApplicationLifetime.StopApplication();
        }

        private static string GetHashSHA256(string text)
        {
            using (SHA256 shA256 = SHA256.Create())
            {
                byte[] hash = shA256.ComputeHash(Encoding.UTF8.GetBytes(text));
                StringBuilder stringBuilder = new StringBuilder();
                for (int index = 0; index < hash.Length; ++index)
                    stringBuilder.Append(hash[index].ToString("x2"));
                return stringBuilder.ToString();
            }
        }
    }
}
