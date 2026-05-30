using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using MyCMS.Core.Entities;
using MyCMS.Data;

namespace MyCMS.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class DashboardController : Controller
    {
        private readonly AppDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public DashboardController(AppDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index()
        {
            var stats = new DashboardStatistics
            {
                TotalPages = _context.Pages.Count(p => !p.IsDeleted),
                TotalArticles = _context.Articles.Count(a => !a.IsDeleted),
                TotalMenus = _context.Menus.Count(m => !m.IsDeleted),
                TotalQuizzes = _context.Quizzes.Count(q => !q.IsDeleted),
                TotalCategories = _context.ArticleCategories.Count(c => !c.IsDeleted),
                TotalFiles = _context.Uploads.Count(f => !f.IsDeleted),
                TotalUsers = _userManager.Users.Count()
            };

            return View(stats);
        }
    }

    public class DashboardStatistics
    {
        public int TotalPages { get; set; }
        public int TotalArticles { get; set; }
        public int TotalMenus { get; set; }
        public int TotalQuizzes { get; set; }
        public int TotalCategories { get; set; }
        public int TotalFiles { get; set; }
        public int TotalUsers { get; set; }
    }
}
