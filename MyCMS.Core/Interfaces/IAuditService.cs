using MyCMS.Core.Entities;

namespace MyCMS.Core.Interfaces
{
    public interface IAuditService
    {
        Task LogAsync(string tableName, string recordId, string action, string oldValues, string newValues, string changeReason = null);
        Task<IEnumerable<AuditLog>> GetByTableAsync(string tableName, int count = 50);
        Task<IEnumerable<AuditLog>> GetByRecordAsync(string tableName, string recordId, int count = 50);
        Task<IEnumerable<AuditLog>> GetByUserAsync(Guid userId, int count = 50);
        Task<IEnumerable<AuditLog>> GetRecentAsync(int count = 100);
    }
}
