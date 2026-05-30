using Microsoft.EntityFrameworkCore;
using MyCMS.Core.Entities;
using MyCMS.Core.Interfaces;
using MyCMS.Data;

namespace MyCMS.Services
{
    public class ThemeService : IThemeService
    {
        private readonly AppDbContext _context;

        public ThemeService(AppDbContext context)
        {
            _context = context;
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
            _context.Themes.Add(theme);
            await _context.SaveChangesAsync();
            return theme;
        }

        public async Task<Theme> UpdateThemeAsync(Theme theme)
        {
            _context.Themes.Update(theme);
            await _context.SaveChangesAsync();
            return theme;
        }

        public async Task DeleteThemeAsync(Guid id)
        {
            var theme = await _context.Themes.FindAsync(id);
            if (theme != null)
            {
                theme.IsDeleted = true;
                await _context.SaveChangesAsync();
            }
        }

        public async Task SetActiveThemeAsync(Guid themeId)
        {
            // Deactivate all themes
            var allThemes = await _context.Themes.ToListAsync();
            foreach (var theme in allThemes)
            {
                theme.IsActive = false;
            }

            // Activate the selected theme
            var activeTheme = await _context.Themes.FindAsync(themeId);
            if (activeTheme != null)
            {
                activeTheme.IsActive = true;
            }

            await _context.SaveChangesAsync();
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
                config.Value = value;
            }
            else
            {
                config = new ThemeConfiguration
                {
                    ThemeId = themeId,
                    Key = key,
                    Value = value
                };
                _context.ThemeConfigurations.Add(config);
            }

            await _context.SaveChangesAsync();
        }

        public async Task<string?> GetThemeLayoutPathAsync(Guid themeId)
        {
            var theme = await _context.Themes.FindAsync(themeId);
            return theme?.FolderName;
        }
    }
}
