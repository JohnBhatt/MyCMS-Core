using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using MyCMS.Core.Entities;
using MyCMS.Core.Interfaces;
using MyCMS.Data.Interfaces;

namespace MyCMS.Data.Services
{
    public class DbInitializer : IDbInitializer
    {
        private readonly AppDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole<Guid>> _roleManager;
        private readonly IConfiguration _configuration;
        private readonly ISettingService _settingService;
        private readonly ILogger<DbInitializer> _logger;

        public DbInitializer(
            AppDbContext context,
            UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole<Guid>> roleManager,
            IConfiguration configuration,
            ISettingService settingService,
            ILogger<DbInitializer> logger)
        {
            _context = context;
            _userManager = userManager;
            _roleManager = roleManager;
            _configuration = configuration;
            _settingService = settingService;
            _logger = logger;
        }

        public async Task SeedAsync()
        {
            try
            {
                _logger.LogInformation("Starting database seeding...");

                await SeedRolesAsync();
                await SeedAdminUserAsync();
                await SeedDefaultMenuAsync();
                await SeedThemesAsync();
                await _settingService.SeedDefaultSettingsAsync();

                _logger.LogInformation("Database seeding completed successfully.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while seeding the database.");
                throw;
            }
        }

        private async Task SeedRolesAsync()
        {
            string[] roleNames = { "Admin", "User" };

            foreach (var roleName in roleNames)
            {
                if (!await _roleManager.RoleExistsAsync(roleName))
                {
                    await _roleManager.CreateAsync(new IdentityRole<Guid>(roleName));
                    _logger.LogInformation("Created role: {RoleName}", roleName);
                }
            }
        }

        private async Task SeedAdminUserAsync()
        {
            var adminEmail = "admin@mycms.com";
            var adminUser = await _userManager.FindByEmailAsync(adminEmail);

            if (adminUser == null)
            {
                // Get password from configuration or generate random
                var password = _configuration["DefaultAdminPassword"];
                if (string.IsNullOrEmpty(password))
                {
                    password = GenerateRandomPassword();
                    _logger.LogWarning("========================================");
                    _logger.LogWarning("DEFAULT ADMIN PASSWORD: {Password}", password);
                    _logger.LogWarning("Change this password immediately after first login!");
                    _logger.LogWarning("========================================");
                }

                adminUser = new ApplicationUser
                {
                    UserName = adminEmail,
                    Email = adminEmail,
                    EmailConfirmed = true,
                    FirstName = "Admin",
                    LastName = "User",
                    IsAuthorized = true,
                    RegDate = DateTime.UtcNow
                };

                var result = await _userManager.CreateAsync(adminUser, password);

                if (result.Succeeded)
                {
                    await _userManager.AddToRoleAsync(adminUser, "Admin");
                    _logger.LogInformation("Created default admin user: {Email}", adminEmail);
                }
                else
                {
                    _logger.LogError("Failed to create admin user: {Errors}",
                        string.Join(", ", result.Errors.Select(e => e.Description)));
                }
            }
        }

        private static string GenerateRandomPassword()
        {
            const string upperCase = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            const string lowerCase = "abcdefghijklmnopqrstuvwxyz";
            const string digits = "0123456789";
            const string special = "!@#$%^&*";
            
            var random = new Random();
            var password = new List<char>();
            
            // Ensure at least one of each required character type
            password.Add(upperCase[random.Next(upperCase.Length)]);
            password.Add(lowerCase[random.Next(lowerCase.Length)]);
            password.Add(digits[random.Next(digits.Length)]);
            password.Add(special[random.Next(special.Length)]);
            
            // Fill remaining with random characters
            var allChars = upperCase + lowerCase + digits + special;
            for (int i = 4; i < 12; i++)
            {
                password.Add(allChars[random.Next(allChars.Length)]);
            }
            
            // Shuffle the password
            return new string(password.OrderBy(x => random.Next()).ToArray());
        }

        private async Task SeedDefaultMenuAsync()
        {
            if (!await _context.Menus.AnyAsync(m => m.Position == "Main" && !m.IsDeleted))
            {
                var mainMenu = new Menu
                {
                    Id = Guid.NewGuid(),
                    MenuName = "Main Menu",
                    Position = "Main",
                    MenuDesc = "Primary navigation menu",
                    CreatedOn = DateTime.UtcNow
                };

                _context.Menus.Add(mainMenu);
                await _context.SaveChangesAsync();

                _logger.LogInformation("Created default Main menu.");
            }
        }

        private async Task SeedThemesAsync()
        {
            var defaultThemes = new List<Theme>
            {
                new Theme
                {
                    Id = Guid.Parse("11111111-1111-1111-1111-111111111111"),
                    Name = "Minimal",
                    DisplayName = "Minimal",
                    FolderName = "Minimal",
                    Thumbnail = "/Themes/Minimal/assets/thumbnail.svg",
                    Description = "Clean, typography-focused design with elegant serif headings and lots of whitespace.",
                    IsActive = true,
                    IsDefault = true,
                    CreatedOn = DateTime.UtcNow
                },
                new Theme
                {
                    Id = Guid.Parse("22222222-2222-2222-2222-222222222222"),
                    Name = "Blog",
                    DisplayName = "Blog",
                    FolderName = "Blog",
                    Thumbnail = "/Themes/Blog/assets/thumbnail.svg",
                    Description = "Classic blog layout with sidebar, blue accent, and card-based article grid.",
                    IsActive = false,
                    IsDefault = false,
                    CreatedOn = DateTime.UtcNow
                },
                new Theme
                {
                    Id = Guid.Parse("33333333-3333-3333-3333-333333333333"),
                    Name = "Magazine",
                    DisplayName = "Magazine",
                    FolderName = "Magazine",
                    Thumbnail = "/Themes/Magazine/assets/thumbnail.svg",
                    Description = "Bold magazine style with dark hero section, featured posts, and trending sidebar.",
                    IsActive = false,
                    IsDefault = false,
                    CreatedOn = DateTime.UtcNow
                },
                new Theme
                {
                    Id = Guid.Parse("44444444-4444-4444-4444-444444444444"),
                    Name = "Modern",
                    DisplayName = "Modern",
                    FolderName = "Modern",
                    Thumbnail = "/Themes/Modern/assets/thumbnail.svg",
                    Description = "Vibrant gradients (purple/pink), rounded cards, and configurable category grid with icons.",
                    IsActive = false,
                    IsDefault = false,
                    CreatedOn = DateTime.UtcNow
                }
            };

            foreach (var theme in defaultThemes)
            {
                if (!await _context.Themes.AnyAsync(t => t.Id == theme.Id))
                {
                    _context.Themes.Add(theme);
                    _logger.LogInformation("Created theme: {ThemeName}", theme.Name);
                }
            }

            await _context.SaveChangesAsync();
        }
    }
}
