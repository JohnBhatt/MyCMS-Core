using MyCMS.Core.Entities;

namespace MyCMS.Core.Interfaces
{
    public interface ISettingService
    {
        Task<Setting> GetByKeyAsync(string key);
        Task<string> GetValueAsync(string key, string defaultValue = null);
        Task<T> GetValueAsync<T>(string key, T defaultValue = default);
        Task<IEnumerable<Setting>> GetByCategoryAsync(string category);
        Task<IEnumerable<Setting>> GetAllAsync();
        Task<Setting> CreateAsync(Setting setting, string changeReason = null);
        Task<Setting> UpdateAsync(Setting setting, string changeReason = null);
        Task DeleteAsync(string key, string changeReason = null);
        Task<IEnumerable<AuditLog>> GetAuditLogsAsync(string key, int count = 50);
        Task SeedDefaultSettingsAsync();
    }
}
