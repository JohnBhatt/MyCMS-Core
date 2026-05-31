using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using MyCMS.Core.Entities;
using MyCMS.Core.Interfaces;
using MyCMS.Data;

namespace MyCMS.Services
{
    public class AuditService : IAuditService
    {
        private readonly AppDbContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public AuditService(AppDbContext context, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task LogAsync(string tableName, string recordId, string action, string oldValues, string newValues, string changeReason = null)
        {
            var user = _httpContextAccessor.HttpContext?.User;
            var userId = user?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var userName = user?.Identity?.Name;

            var audit = new AuditLog
            {
                TableName = tableName,
                RecordId = recordId,
                Action = action,
                OldValues = oldValues,
                NewValues = newValues,
                ChangeReason = changeReason,
                ModifiedBy = userId != null ? Guid.Parse(userId) : null,
                ModifiedByUserName = userName,
                IpAddress = _httpContextAccessor.HttpContext?.Connection?.RemoteIpAddress?.ToString(),
                UserAgent = _httpContextAccessor.HttpContext?.Request?.Headers["User-Agent"].ToString(),
                CreatedOn = DateTime.UtcNow
            };

            _context.AuditLogs.Add(audit);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<AuditLog>> GetByTableAsync(string tableName, int count = 50)
        {
            return await _context.AuditLogs
                .Where(a => a.TableName == tableName)
                .OrderByDescending(a => a.CreatedOn)
                .Take(count)
                .ToListAsync();
        }

        public async Task<IEnumerable<AuditLog>> GetByRecordAsync(string tableName, string recordId, int count = 50)
        {
            return await _context.AuditLogs
                .Where(a => a.TableName == tableName && a.RecordId == recordId)
                .OrderByDescending(a => a.CreatedOn)
                .Take(count)
                .ToListAsync();
        }

        public async Task<IEnumerable<AuditLog>> GetByUserAsync(Guid userId, int count = 50)
        {
            return await _context.AuditLogs
                .Where(a => a.ModifiedBy == userId)
                .OrderByDescending(a => a.CreatedOn)
                .Take(count)
                .ToListAsync();
        }

        public async Task<IEnumerable<AuditLog>> GetRecentAsync(int count = 100)
        {
            return await _context.AuditLogs
                .OrderByDescending(a => a.CreatedOn)
                .Take(count)
                .ToListAsync();
        }
    }
}
