using MyCMS.Core.Entities;

namespace MyCMS.Core.Interfaces
{
    public interface IConfigurationService
    {
        Task<Configuration?> GetConfigurationAsync(string sectionName = "Default");
        Task<Configuration> UpdateConfigurationAsync(Configuration configuration);
    }
}
