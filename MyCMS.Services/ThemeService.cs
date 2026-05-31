using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using MyCMS.Core.Entities;
using MyCMS.Core.Interfaces;
using MyCMS.Data;

namespace MyCMS.Services
{
    public class ThemeService : IThemeService
    {
        private readonly AppDbContext _context;
        private readonly IAuditService _auditService;

        public ThemeService(AppDbContext context, IAuditService auditService)
        {
            _context = context;
            _auditService = auditService;
        }

        public async Task<Theme?> GetActiveThemeAsync()
        {
            return await _context.Themes.FirstOrDefaultAsync(t => t.IsActive);
        }

        public async Task<Theme?> GetThemeByIdAsync(Guid id)
        {
            return await _context.Themes.FindAsync(id);
        }

        public async Task<Theme?> GetThemeByNameAsync(string name)
        {
            return await _context.Themes.FirstOrDefaultAsync(t => t.Name == name);
        }

        public async Task<IEnumerable<Theme>> GetAllThemesAsync()
        {
            return await _context.Themes.ToListAsync();
        }

        public async Task<Theme> CreateThemeAsync(Theme theme)
        {
            theme.Id = Guid.NewGuid();
            theme.CreatedOn = DateTime.UtcNow;
            _context.Themes.Add(theme);
            await _context.SaveChangesAsync();
            
            await _auditService.LogAsync("Themes", theme.Id.ToString(), "Created", null,
                JsonSerializer.Serialize(new { theme.Name, theme.DisplayName, theme.FolderName }), "Theme created");
            return theme;
        }

        public async Task<Theme> UpdateThemeAsync(Theme theme)
        {
            var existing = await _context.Themes.FindAsync(theme.Id);
            if (existing == null) throw new InvalidOperationException("Theme not found");
            
            var oldValues = JsonSerializer.Serialize(new { existing.Name, existing.DisplayName, existing.IsActive, existing.IsDefault });
            
            theme.ModifiedOn = DateTime.UtcNow;
            _context.Themes.Update(theme);
            await _context.SaveChangesAsync();
            
            var newValues = JsonSerializer.Serialize(new { theme.Name, theme.DisplayName, theme.IsActive, theme.IsDefault });
            await _auditService.LogAsync("Themes", theme.Id.ToString(), "Updated", oldValues, newValues, "Theme updated");
            return theme;
        }

        public async Task DeleteThemeAsync(Guid id)
        {
            var theme = await _context.Themes.FindAsync(id);
            if (theme != null)
            {
                var oldValues = JsonSerializer.Serialize(new { theme.Name, theme.DisplayName });
                theme.IsDeleted = true;
                theme.ModifiedOn = DateTime.UtcNow;
                await _context.SaveChangesAsync();
                
                await _auditService.LogAsync("Themes", id.ToString(), "Deleted", oldValues, null, "Theme deleted");
            }
        }

        public async Task SetActiveThemeAsync(Guid themeId)
        {
            // Deactivate all themes
            var allThemes = await _context.Themes.ToListAsync();
            foreach (var theme in allThemes)
            {
                if (theme.IsActive)
                {
                    theme.IsActive = false;
                    await _auditService.LogAsync("Themes", theme.Id.ToString(), "Updated",
                        JsonSerializer.Serialize(new { IsActive = true }),
                        JsonSerializer.Serialize(new { IsActive = false }), "Theme deactivated");
                }
            }

            // Activate the selected theme
            var activeTheme = await _context.Themes.FindAsync(themeId);
            if (activeTheme != null)
            {
                activeTheme.IsActive = true;
                await _context.SaveChangesAsync();
                
                await _auditService.LogAsync("Themes", themeId.ToString(), "Updated",
                    JsonSerializer.Serialize(new { IsActive = false }),
                    JsonSerializer.Serialize(new { IsActive = true }), "Theme activated");
            }
        }

        public async Task<ThemeConfiguration?> GetThemeConfigurationAsync(Guid themeId, string key)
        {
            return await _context.ThemeConfigurations
                .FirstOrDefaultAsync(tc => tc.ThemeId == themeId && tc.Key == key);
        }

        public async Task SetThemeConfigurationAsync(Guid themeId, string key, string value)
        {
            var config = await _context.ThemeConfigurations
                .FirstOrDefaultAsync(tc => tc.ThemeId == themeId && tc.Key == key);

            if (config != null)
            {
                var oldValue = config.Value;
                config.Value = value;
                await _context.SaveChangesAsync();
                
                await _auditService.LogAsync("ThemeConfigurations", config.Id.ToString(), "Updated",
                    JsonSerializer.Serialize(new { Value = oldValue }),
                    JsonSerializer.Serialize(new { Value = value }), $"Theme config '{key}' updated");
            }
            else
            {
                config = new ThemeConfiguration
                {
                    Id = Guid.NewGuid(),
                    ThemeId = themeId,
                    Key = key,
                    Value = value,
                    CreatedOn = DateTime.UtcNow
                };
                _context.ThemeConfigurations.Add(config);
                await _context.SaveChangesAsync();
                
                await _auditService.LogAsync("ThemeConfigurations", config.Id.ToString(), "Created", null,
                    JsonSerializer.Serialize(new { config.Key, config.Value }), $"Theme config '{key}' created");
            }
        }

        public async Task<string?> GetThemeLayoutPathAsync(Guid themeId)
        {
            var theme = await _context.Themes.FindAsync(themeId);
            return theme?.FolderName;
        }
    }
}
