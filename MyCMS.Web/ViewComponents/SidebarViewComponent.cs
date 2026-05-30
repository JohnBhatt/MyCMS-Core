using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyCMS.Core.Entities;
using MyCMS.Core.Interfaces;
using MyCMS.Data;

namespace MyCMS.Web.ViewComponents
{
    public class SidebarViewComponent : ViewComponent
    {
        private readonly AppDbContext _context;
        private readonly IArticleService _articleService;
        private readonly ICategoryService _categoryService;

        public SidebarViewComponent(AppDbContext context, IArticleService articleService, ICategoryService categoryService)
        {
            _context = context;
            _articleService = articleService;
            _categoryService = categoryService;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var model = new SidebarViewModel
            {
                RecentPosts = await _context.Articles
                    .Where(a => a.PublishedDate != null && a.PublishedDate <= DateTime.UtcNow)
                    .OrderByDescending(a => a.PublishedDate)
                    .Take(5)
                    .Select(a => new SidebarPostItem
                    {
                        Id = a.Id,
                        Title = a.Title,
                        Slug = a.Slug,
                        FeaturedImage = a.FeaturedImage,
                        PublishedDate = a.PublishedDate
                    })
                    .ToListAsync(),

                Categories = await BuildCategoryTreeAsync(),

                Tags = await _context.ArticleTags
                    .Where(t => !t.IsDeleted)
                    .Take(20)
                    .Select(t => new SidebarTagItem
                    {
                        Id = t.Id,
                        Name = t.TagName,
                        Slug = t.Slug
                    })
                    .ToListAsync()
            };

            return View(model);
        }

        private async Task<List<SidebarCategoryItem>> BuildCategoryTreeAsync()
        {
            var allCategories = await _context.ArticleCategories
                .Where(c => !c.IsDeleted)
                .Select(c => new SidebarCategoryItem
                {
                    Id = c.Id,
                    Name = c.CategoryName,
                    Slug = c.Slug,
                    ParentId = c.ParentCategory,
                    PostCount = _context.Articles.Count(a => a.CategoryId == c.Id && !a.IsDeleted)
                })
                .ToListAsync();

            var rootCategories = allCategories.Where(c => c.ParentId == null).ToList();
            
            foreach (var root in rootCategories)
            {
                BuildCategoryHierarchy(root, allCategories, 0);
            }

            return rootCategories;
        }

        private void BuildCategoryHierarchy(SidebarCategoryItem parent, List<SidebarCategoryItem> allCategories, int level)
        {
            parent.Level = level;
            var children = allCategories.Where(c => c.ParentId == parent.Id).ToList();
            
            foreach (var child in children)
            {
                BuildCategoryHierarchy(child, allCategories, level + 1);
                parent.Children.Add(child);
            }
        }
    }

    public class SidebarViewModel
    {
        public List<SidebarPostItem> RecentPosts { get; set; } = new();
        public List<SidebarCategoryItem> Categories { get; set; } = new();
        public List<SidebarTagItem> Tags { get; set; } = new();
    }

    public class SidebarPostItem
    {
        public Guid Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Slug { get; set; } = string.Empty;
        public string? FeaturedImage { get; set; }
        public DateTime? PublishedDate { get; set; }
    }

    public class SidebarCategoryItem
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Slug { get; set; } = string.Empty;
        public int PostCount { get; set; }
        public Guid? ParentId { get; set; }
        public int Level { get; set; }
        public List<SidebarCategoryItem> Children { get; set; } = new();
    }

    public class SidebarTagItem
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Slug { get; set; } = string.Empty;
    }
}
