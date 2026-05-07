namespace MyCMS.Core.Entities
{
    public class ArticleTagMapping : BaseEntity
    {
        public Guid ArticleId { get; set; }
        public Guid TagId { get; set; }
    }
}
