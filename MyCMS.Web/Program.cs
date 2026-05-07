using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using MyCMS.Core.Entities;
using MyCMS.Core.Interfaces;
using MyCMS.Data;
using MyCMS.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();

// Configure dual database support
var dbProvider = builder.Configuration.GetValue<string>("DatabaseProvider", "PostgreSQL");

if (dbProvider == "PostgreSQL")
{
    builder.Services.AddDbContext<PostgreSqlDbContext>(options =>
        options.UseNpgsql(builder.Configuration.GetConnectionString("PostgreSQL")));

    builder.Services.AddIdentity<ApplicationUser, IdentityRole<Guid>>(options =>
    {
        options.Password.RequireDigit = true;
        options.Password.RequireLowercase = true;
        options.Password.RequireUppercase = true;
        options.Password.RequireNonAlphanumeric = true;
        options.Password.RequiredLength = 8;
    })
    .AddEntityFrameworkStores<PostgreSqlDbContext>()
    .AddDefaultTokenProviders();
}
else
{
    builder.Services.AddDbContext<SqlServerDbContext>(options =>
        options.UseSqlServer(builder.Configuration.GetConnectionString("SqlServer")));

    builder.Services.AddIdentity<ApplicationUser, IdentityRole<Guid>>(options =>
    {
        options.Password.RequireDigit = true;
        options.Password.RequireLowercase = true;
        options.Password.RequireUppercase = true;
        options.Password.RequireNonAlphanumeric = true;
        options.Password.RequiredLength = 8;
    })
    .AddEntityFrameworkStores<SqlServerDbContext>()
    .AddDefaultTokenProviders();
}

// Register services
builder.Services.AddScoped<IPageService, PageService>();
builder.Services.AddScoped<IArticleService, ArticleService>();
builder.Services.AddScoped<IMenuService, MenuService>();
builder.Services.AddScoped<IFileService, FileService>();
builder.Services.AddScoped<IQuizService, QuizService>();
builder.Services.AddScoped<IConfigurationService, ConfigurationService>();
builder.Services.AddScoped<IOpenGraphService, OpenGraphService>();
builder.Services.AddScoped<ISitemapService, SitemapService>();
builder.Services.AddScoped<IRssFeedService, RssFeedService>();
builder.Services.AddScoped<IAmpService, AmpService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();
app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapRazorPages();

app.Run();
