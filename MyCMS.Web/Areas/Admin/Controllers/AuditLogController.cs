using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MyCMS.Core.Interfaces;
using MyCMS.Data;

namespace MyCMS.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class AuditLogController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IAuditService _auditService;

        public AuditLogController(AppDbContext context, IAuditService auditService)
        {
            _context = context;
            _auditService = auditService;
        }

        public async Task<IActionResult> Index(
            string tableName = null,
            string action = null,
            Guid? userId = null,
            DateTime? fromDate = null,
            DateTime? toDate = null,
            string search = null,
            int page = 1,
            int pageSize = 50)
        {
            var query = _context.AuditLogs.AsQueryable();

            // Apply filters
            if (!string.IsNullOrEmpty(tableName))
                query = query.Where(a => a.TableName == tableName);

            if (!string.IsNullOrEmpty(action))
                query = query.Where(a => a.Action == action);

            if (userId.HasValue)
                query = query.Where(a => a.ModifiedBy == userId);

            if (fromDate.HasValue)
                query = query.Where(a => a.CreatedOn >= fromDate.Value);

            if (toDate.HasValue)
                query = query.Where(a => a.CreatedOn <= toDate.Value.AddDays(1));

            if (!string.IsNullOrEmpty(search))
                query = query.Where(a => 
                    a.TableName.Contains(search) || 
                    a.RecordId.Contains(search) || 
                    a.ModifiedByUserName.Contains(search) ||
                    a.ChangeReason.Contains(search));

            // Get total count for pagination
            var totalCount = await query.CountAsync();

            // Get paginated results
            var logs = await query
                .OrderByDescending(a => a.CreatedOn)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            // Get distinct table names for filter dropdown
            var tableNames = await _context.AuditLogs
                .Select(a => a.TableName)
                .Distinct()
                .OrderBy(t => t)
                .ToListAsync();

            // Get users who made changes for filter dropdown
            var users = await _context.AuditLogs
                .Where(a => a.ModifiedBy != null)
                .Select(a => new { a.ModifiedBy, a.ModifiedByUserName })
                .Distinct()
                .OrderBy(u => u.ModifiedByUserName)
                .ToListAsync();

            // Build ViewBag data
            ViewBag.TableNames = new SelectList(tableNames);
            ViewBag.Actions = new SelectList(new[] { "Created", "Updated", "Deleted" });
            ViewBag.Users = new SelectList(users.Select(u => new {
                Value = u.ModifiedBy.ToString(),
                Text = u.ModifiedByUserName ?? u.ModifiedBy.ToString()
            }), "Value", "Text");

            ViewBag.CurrentTableName = tableName;
            ViewBag.CurrentAction = action;
            ViewBag.CurrentUserId = userId;
            ViewBag.CurrentFromDate = fromDate?.ToString("yyyy-MM-dd");
            ViewBag.CurrentToDate = toDate?.ToString("yyyy-MM-dd");
            ViewBag.CurrentSearch = search;
            ViewBag.CurrentPage = page;
            ViewBag.PageSize = pageSize;
            ViewBag.TotalCount = totalCount;
            ViewBag.TotalPages = (int)Math.Ceiling((double)totalCount / pageSize);

            return View(logs);
        }

        public async Task<IActionResult> Details(Guid id)
        {
            var log = await _context.AuditLogs.FindAsync(id);
            if (log == null)
                return NotFound();

            return View(log);
        }

        public async Task<IActionResult> ByRecord(string tableName, string recordId)
        {
            var logs = await _auditService.GetByRecordAsync(tableName, recordId, 100);
            ViewBag.TableName = tableName;
            ViewBag.RecordId = recordId;
            return View(logs);
        }

        public async Task<IActionResult> ByUser(Guid userId)
        {
            var logs = await _auditService.GetByUserAsync(userId, 100);
            ViewBag.UserId = userId;
            
            var userName = await _context.AuditLogs
                .Where(a => a.ModifiedBy == userId && !string.IsNullOrEmpty(a.ModifiedByUserName))
                .Select(a => a.ModifiedByUserName)
                .FirstOrDefaultAsync();
            ViewBag.UserName = userName ?? userId.ToString();
            
            return View(logs);
        }

        [HttpPost]
        public async Task<IActionResult> ClearOldLogs(int daysToKeep = 90)
        {
            var cutoffDate = DateTime.UtcNow.AddDays(-daysToKeep);
            var oldLogs = await _context.AuditLogs
                .Where(a => a.CreatedOn < cutoffDate)
                .ToListAsync();

            _context.AuditLogs.RemoveRange(oldLogs);
            await _context.SaveChangesAsync();

            TempData["Success"] = $"{oldLogs.Count} audit logs older than {daysToKeep} days have been cleared.";
            return RedirectToAction(nameof(Index));
        }
    }
}
