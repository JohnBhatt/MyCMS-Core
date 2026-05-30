using MyCMS.Core.Entities;

namespace MyCMS.Core.Interfaces
{
    public interface ISearchService
    {
        Task<SearchResult> SearchAsync(string query, int page = 1, int pageSize = 10);
    }

    public class SearchResult
    {
        public List<ArticleSearchResult> Articles { get; set; } = new();
        public List<PageSearchResult> Pages { get; set; } = new();
        public int TotalResults => Articles.Count + Pages.Count;
        public int CurrentPage { get; set; }
        public int PageSize { get; set; }
        public int TotalPages => (int)Math.Ceiling((double)TotalResults / PageSize);
    }

    public class ArticleSearchResult
    {
        public Guid Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Slug { get; set; } = string.Empty;
        public string? MetaDescription { get; set; }
        public string? FeaturedImage { get; set; }
        public string? CategoryName { get; set; }
        public DateTime? PublishedDate { get; set; }
        public string Type => "Article";
    }

    public class PageSearchResult
    {
        public Guid Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Slug { get; set; } = string.Empty;
        public string? MetaDescription { get; set; }
        public DateTime? CreatedOn { get; set; }
        public string Type => "Page";
    }
}
