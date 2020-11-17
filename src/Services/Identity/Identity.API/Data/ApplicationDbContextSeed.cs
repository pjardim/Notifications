using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Rainmaker.Services.Identity.API.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Rainmaker.Services.Identity.API.Data
{
    public class ApplicationDbContextSeed
    {
        private readonly IPasswordHasher<ApplicationUser> _passwordHasher = new PasswordHasher<ApplicationUser>();

        public async Task SeedAsync(ApplicationDbContext context, IWebHostEnvironment env,
            ILogger<ApplicationDbContextSeed> logger, IOptions<AppSettings> settings, int? retry = 0)
        {
            int retryForAvaiability = retry.Value;

            try
            {
                var useCustomizationData = settings.Value.UseCustomizationData;
                var contentRootPath = env.ContentRootPath;
                var webroot = env.WebRootPath;

                if (!context.Users.Any())
                {
                    context.Users.AddRange(GetDefaultUser());
                    await context.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {
                if (retryForAvaiability < 10)
                {
                    retryForAvaiability++;

                    logger.LogError(ex, "EXCEPTION ERROR while migrating {DbContextName}", nameof(ApplicationDbContext));

                    await SeedAsync(context, env, logger, settings, retryForAvaiability);
                }
            }
        }

        private ApplicationUser CreateApplicationUser(string[] column, string[] headers)
        {
            if (column.Count() != headers.Count())
            {
                throw new Exception($"column count '{column.Count()}' not the same as headers count'{headers.Count()}'");
            }

            var user = new ApplicationUser
            {
                Id = Guid.NewGuid().ToString(),
                Name = column[Array.IndexOf(headers, "name")].Trim('"').Trim(),
                UserName = column[Array.IndexOf(headers, "username")].Trim('"').Trim(),
                NormalizedEmail = column[Array.IndexOf(headers, "normalizedemail")].Trim('"').Trim(),
                NormalizedUserName = column[Array.IndexOf(headers, "normalizedusername")].Trim('"').Trim(),
                SecurityStamp = Guid.NewGuid().ToString("D"),
                PasswordHash = column[Array.IndexOf(headers, "password")].Trim('"').Trim(), // Note: This is the password
            };

            user.PasswordHash = _passwordHasher.HashPassword(user, user.PasswordHash);

            return user;
        }

        private IEnumerable<ApplicationUser> GetDefaultUser()
        {
            var user =
           new ApplicationUser()
           {
               Email = "crew@rainmaker.ie",
               Id = Guid.NewGuid().ToString(),
               Name = "Crew",
               PartyId = "123",
               NormalizedEmail = "crew@rainmaker.ie",
               NormalizedUserName = "crew@rainmaker.ie",
               UserName = "crew@rainmaker.ie",
               SecurityStamp = Guid.NewGuid().ToString("D"),
           };

            var backOffice =
           new ApplicationUser()
           {
               Email = "backOffice@rainmaker.ie",
               Id = Guid.NewGuid().ToString(),
               Name = "Back Offcie",
               PartyId = "456",
               NormalizedEmail = "backOffice@rainmaker.ie",
               NormalizedUserName = "backOffice@rainmaker.ie",
               UserName = "backOffice@rainmaker.ie",
               SecurityStamp = Guid.NewGuid().ToString("D"),
           };

            var jetblueCrewTest =
            new ApplicationUser()
            {
                Email = "jetblueCrew@rainmaker.ie",
                Id = Guid.NewGuid().ToString(),
                Name = "Joshua",
                PartyId = "737",
                NormalizedEmail = "jetblueCrew@rainmaker.ie",
                NormalizedUserName = "jetblueCrew@rainmaker.ie",
                UserName = "jetblueCrew@rainmaker.ie",
                SecurityStamp = Guid.NewGuid().ToString("D"),
            };

            var jetblueAdmintest =
           new ApplicationUser()
           {
               Email = "jetblueAdmin@rainmaker.ie",
               Id = Guid.NewGuid().ToString(),
               Name = "Back Offcie",
               PartyId = "19893",
               NormalizedEmail = "jetblueAdmin@rainmaker.ie",
               NormalizedUserName = "jetblueAdmin@rainmaker.ie",
               UserName = "jetblueAdmin@rainmaker.ie",
               SecurityStamp = Guid.NewGuid().ToString("D"),
           };

            user.PasswordHash = _passwordHasher.HashPassword(user, "123");
            backOffice.PasswordHash = _passwordHasher.HashPassword(user, "123");
            jetblueCrewTest.PasswordHash = _passwordHasher.HashPassword(user, "123");
            jetblueAdmintest.PasswordHash = _passwordHasher.HashPassword(user, "123");
            return new List<ApplicationUser>()
            {
                user,
                backOffice,
                jetblueCrewTest,
                jetblueAdmintest,
            };
        }
    }
}