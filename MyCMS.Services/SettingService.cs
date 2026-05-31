using System.Text.Json;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using MyCMS.Core.Entities;
using MyCMS.Core.Interfaces;
using MyCMS.Data;

namespace MyCMS.Services
{
    public class SettingService : ISettingService
    {
        private readonly AppDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IAuditService _auditService;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public SettingService(
            AppDbContext context,
            UserManager<ApplicationUser> userManager,
            IAuditService auditService,
            IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _userManager = userManager;
            _auditService = auditService;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<Setting> GetByKeyAsync(string key)
        {
            return await _context.Settings
                .FirstOrDefaultAsync(s => s.Key == key && !s.IsDeleted);
        }

        public async Task<string> GetValueAsync(string key, string defaultValue = null)
        {
            var setting = await GetByKeyAsync(key);
            return setting?.Value ?? defaultValue;
        }

        public async Task<T> GetValueAsync<T>(string key, T defaultValue = default)
        {
            var value = await GetValueAsync(key);
            if (string.IsNullOrEmpty(value))
                return defaultValue;

            try
            {
                return JsonSerializer.Deserialize<T>(value);
            }
            catch
            {
                return defaultValue;
            }
        }

        public async Task<IEnumerable<Setting>> GetByCategoryAsync(string category)
        {
            return await _context.Settings
                .Where(s => s.Category == category && !s.IsDeleted)
                .OrderBy(s => s.Key)
                .ToListAsync();
        }

        public async Task<IEnumerable<Setting>> GetAllAsync()
        {
            return await _context.Settings
                .Where(s => !s.IsDeleted)
                .OrderBy(s => s.Category)
                .ThenBy(s => s.Key)
                .ToListAsync();
        }

        public async Task<Setting> CreateAsync(Setting setting, string changeReason = null)
        {
            setting.CreatedOn = DateTime.UtcNow;
            _context.Settings.Add(setting);
            await _context.SaveChangesAsync();

            await _auditService.LogAsync("Settings", setting.Key, "Created", null, 
                JsonSerializer.Serialize(new { setting.Value, setting.Category, setting.Description }), changeReason);
            return setting;
        }

        public async Task<Setting> UpdateAsync(Setting setting, string changeReason = null)
        {
            var existing = await GetByKeyAsync(setting.Key);
            if (existing == null)
                throw new InvalidOperationException($"Setting '{setting.Key}' not found");

            if (!existing.IsEditable)
                throw new InvalidOperationException($"Setting '{setting.Key}' is not editable");

            var oldValue = existing.Value;

            existing.Value = setting.Value;
            existing.Description = setting.Description ?? existing.Description;
            existing.Category = setting.Category ?? existing.Category;
            existing.ModifiedOn = DateTime.UtcNow;

            await _context.SaveChangesAsync();
            await _auditService.LogAsync("Settings", setting.Key, "Updated", 
                JsonSerializer.Serialize(new { Value = oldValue }), 
                JsonSerializer.Serialize(new { setting.Value, setting.Category, setting.Description }), changeReason);

            return existing;
        }

        public async Task DeleteAsync(string key, string changeReason = null)
        {
            var setting = await GetByKeyAsync(key);
            if (setting == null)
                throw new InvalidOperationException($"Setting '{key}' not found");

            if (!setting.IsEditable)
                throw new InvalidOperationException($"Setting '{key}' cannot be deleted");

            var oldValue = setting.Value;
            setting.IsDeleted = true;
            setting.ModifiedOn = DateTime.UtcNow;

            await _context.SaveChangesAsync();
            await _auditService.LogAsync("Settings", key, "Deleted", 
                JsonSerializer.Serialize(new { Value = oldValue }), null, changeReason);
        }

        public async Task<IEnumerable<AuditLog>> GetAuditLogsAsync(string key, int count = 50)
        {
            return await _auditService.GetByRecordAsync("Settings", key, count);
        }

        public async Task SeedDefaultSettingsAsync()
        {
            var defaultSettings = new List<Setting>
            {
                // Social Media Links
                new Setting { Key = "Social.Facebook", Value = "", Category = "Social", Description = "Facebook page URL", IsEditable = true },
                new Setting { Key = "Social.Twitter", Value = "", Category = "Social", Description = "Twitter/X profile URL", IsEditable = true },
                new Setting { Key = "Social.LinkedIn", Value = "", Category = "Social", Description = "LinkedIn profile URL", IsEditable = true },
                new Setting { Key = "Social.Instagram", Value = "", Category = "Social", Description = "Instagram profile URL", IsEditable = true },
                new Setting { Key = "Social.YouTube", Value = "", Category = "Social", Description = "YouTube channel URL", IsEditable = true },
                new Setting { Key = "Social.GitHub", Value = "", Category = "Social", Description = "GitHub profile URL", IsEditable = true },

                // SEO Settings
                new Setting { Key = "SEO.SiteName", Value = "MyCMS", Category = "SEO", Description = "Site name for SEO", IsEditable = true },
                new Setting { Key = "SEO.Description", Value = "", Category = "SEO", Description = "Default meta description", IsEditable = true },
                new Setting { Key = "SEO.Keywords", Value = "", Category = "SEO", Description = "Default meta keywords", IsEditable = true },

                // General Settings
                new Setting { Key = "General.SiteName", Value = "MyCMS", Category = "General", Description = "Website name displayed in header", IsEditable = true },
                new Setting { Key = "General.FooterText", Value = "&copy; 2026 MyCMS. All rights reserved.", Category = "General", Description = "Footer copyright text", IsEditable = true },
                new Setting { Key = "General.ContactEmail", Value = "", Category = "General", Description = "Contact email address", IsEditable = true },

                // Analytics (Encrypted)
                new Setting { Key = "Analytics.GoogleAnalyticsId", Value = "", Category = "Analytics", Description = "Google Analytics Tracking ID", IsEditable = true, IsEncrypted = false },
                new Setting { Key = "Analytics.FacebookPixelId", Value = "", Category = "Analytics", Description = "Facebook Pixel ID", IsEditable = true, IsEncrypted = false },
            };

            foreach (var setting in defaultSettings)
            {
                if (!await _context.Settings.AnyAsync(s => s.Key == setting.Key))
                {
                    setting.CreatedOn = DateTime.UtcNow;
                    _context.Settings.Add(setting);
                    await _auditService.LogAsync("Settings", setting.Key, "Created", null,
                        JsonSerializer.Serialize(new { setting.Value, setting.Category, setting.Description }), "Default seed");
                }
            }

            await _context.SaveChangesAsync();
        }
    }
}
