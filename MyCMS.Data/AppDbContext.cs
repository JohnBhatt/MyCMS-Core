using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using MyCMS.Core.Entities;

namespace MyCMS.Data
{
    public abstract class AppDbContext : IdentityDbContext<ApplicationUser, IdentityRole<Guid>, Guid>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<Page> Pages { get; set; }
        public DbSet<Menu> Menus { get; set; }
        public DbSet<MenuItem> MenuItems { get; set; }
        public DbSet<ArticleCategory> ArticleCategories { get; set; }
        public DbSet<Article> Articles { get; set; }
        public DbSet<ArticleTag> ArticleTags { get; set; }
        public DbSet<ArticleTagMapping> ArticleTagMappings { get; set; }
        public DbSet<Quiz> Quizzes { get; set; }
        public DbSet<QuizQuestion> QuizQuestions { get; set; }
        public DbSet<QuizOption> QuizOptions { get; set; }
        public DbSet<QuizAttempt> QuizAttempts { get; set; }
        public DbSet<QuizAnswer> QuizAnswers { get; set; }
        public DbSet<Upload> Uploads { get; set; }
        public DbSet<Configuration> Configurations { get; set; }
        public DbSet<OpenGraphTag> OpenGraphTags { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configure Page
            modelBuilder.Entity<Page>()
                .Property(p => p.PageURL).IsRequired();

            // Configure Menu - MenuItem relationship
            modelBuilder.Entity<MenuItem>()
                .HasOne<Menu>()
                .WithMany()
                .HasForeignKey(m => m.MenuId)
                .OnDelete(DeleteBehavior.Restrict);

            // Configure MenuItem self-referencing for parent-child
            modelBuilder.Entity<MenuItem>()
                .HasOne<MenuItem>()
                .WithMany()
                .HasForeignKey(m => m.ParentMenuItem)
                .OnDelete(DeleteBehavior.Restrict);

            // Configure Article - ArticleCategory relationship
            modelBuilder.Entity<Article>()
                .HasOne<ArticleCategory>()
                .WithMany()
                .HasForeignKey(a => a.CategoryId)
                .OnDelete(DeleteBehavior.Restrict);

            // Configure ArticleCategory self-referencing
            modelBuilder.Entity<ArticleCategory>()
                .HasOne<ArticleCategory>()
                .WithMany()
                .HasForeignKey(c => c.ParentCategory)
                .OnDelete(DeleteBehavior.Restrict);

            // Configure ArticleTagMapping many-to-many
            modelBuilder.Entity<ArticleTagMapping>()
                .HasOne<Article>()
                .WithMany()
                .HasForeignKey(atm => atm.ArticleId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<ArticleTagMapping>()
                .HasOne<ArticleTag>()
                .WithMany()
                .HasForeignKey(atm => atm.TagId)
                .OnDelete(DeleteBehavior.Cascade);

            // Configure Quiz - QuizQuestion relationship
            modelBuilder.Entity<QuizQuestion>()
                .HasOne<Quiz>()
                .WithMany()
                .HasForeignKey(qq => qq.QuizId)
                .OnDelete(DeleteBehavior.Cascade);

            // Configure QuizQuestion - QuizOption relationship
            modelBuilder.Entity<QuizOption>()
                .HasOne<QuizQuestion>()
                .WithMany()
                .HasForeignKey(qo => qo.QuestionId)
                .OnDelete(DeleteBehavior.Cascade);

            // Configure Quiz - QuizAttempt relationship
            modelBuilder.Entity<QuizAttempt>()
                .HasOne<Quiz>()
                .WithMany()
                .HasForeignKey(qa => qa.QuizId)
                .OnDelete(DeleteBehavior.Cascade);

            // Configure QuizAttempt - QuizAnswer relationship
            modelBuilder.Entity<QuizAnswer>()
                .HasOne<QuizAttempt>()
                .WithMany()
                .HasForeignKey(qa => qa.AttemptId)
                .OnDelete(DeleteBehavior.Cascade);

            // Configure QuizQuestion - QuizAnswer relationship
            modelBuilder.Entity<QuizAnswer>()
                .HasOne<QuizQuestion>()
                .WithMany()
                .HasForeignKey(qa => qa.QuestionId)
                .OnDelete(DeleteBehavior.Restrict);

            // Configure QuizOption - QuizAnswer relationship
            modelBuilder.Entity<QuizAnswer>()
                .HasOne<QuizOption>()
                .WithMany()
                .HasForeignKey(qa => qa.SelectedOptionId)
                .OnDelete(DeleteBehavior.Restrict);

            // Global query filter for soft delete
            modelBuilder.Entity<Page>()
                .HasQueryFilter(p => !p.IsDeleted);

            modelBuilder.Entity<Menu>()
                .HasQueryFilter(m => !m.IsDeleted);

            modelBuilder.Entity<MenuItem>()
                .HasQueryFilter(m => !m.IsDeleted);

            modelBuilder.Entity<ArticleCategory>()
                .HasQueryFilter(c => !c.IsDeleted);

            modelBuilder.Entity<Article>()
                .HasQueryFilter(a => !a.IsDeleted);

            modelBuilder.Entity<ArticleTag>()
                .HasQueryFilter(t => !t.IsDeleted);

            modelBuilder.Entity<ArticleTagMapping>()
                .HasQueryFilter(atm => !atm.IsDeleted);

            modelBuilder.Entity<Quiz>()
                .HasQueryFilter(q => !q.IsDeleted);

            modelBuilder.Entity<QuizQuestion>()
                .HasQueryFilter(qq => !qq.IsDeleted);

            modelBuilder.Entity<QuizOption>()
                .HasQueryFilter(qo => !qo.IsDeleted);

            modelBuilder.Entity<QuizAttempt>()
                .HasQueryFilter(qa => !qa.IsDeleted);

            modelBuilder.Entity<QuizAnswer>()
                .HasQueryFilter(qa => !qa.IsDeleted);

            modelBuilder.Entity<Upload>()
                .HasQueryFilter(u => !u.IsDeleted);

            modelBuilder.Entity<Configuration>()
                .HasQueryFilter(c => !c.IsDeleted);

            modelBuilder.Entity<OpenGraphTag>()
                .HasQueryFilter(o => !o.IsDeleted);
        }
    }
}
