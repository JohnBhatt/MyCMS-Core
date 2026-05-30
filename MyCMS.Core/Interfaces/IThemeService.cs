using MyCMS.Core.Entities;

namespace MyCMS.Core.Interfaces
{
    public interface IThemeService
    {
        Task<Theme?> GetActiveThemeAsync();
        Task<Theme?> GetThemeByIdAsync(Guid id);
        Task<Theme?> GetThemeByNameAsync(string name);
        Task<IEnumerable<Theme>> GetAllThemesAsync();
        Task<Theme> CreateThemeAsync(Theme theme);
        Task<Theme> UpdateThemeAsync(Theme theme);
        Task DeleteThemeAsync(Guid id);
        Task SetActiveThemeAsync(Guid themeId);
        Task<ThemeConfiguration?> GetThemeConfigurationAsync(Guid themeId, string key);
        Task SetThemeConfigurationAsync(Guid themeId, string key, string value);
        Task<string?> GetThemeLayoutPathAsync(Guid themeId);
    }
}
