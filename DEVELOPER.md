# MyCMS Developer Documentation

**Version:** 1.0  
**Target Audience:** Developers customizing or extending MyCMS  
**Last Updated:** May 2026

---

## Table of Contents

1. [Architecture Overview](#architecture-overview)
2. [Project Structure](#project-structure)
3. [Layer-by-Layer Documentation](#layer-by-layer-documentation)
4. [Entity Framework & Data Access](#entity-framework--data-access)
5. [Authentication & Authorization](#authentication--authorization)
6. [Frontend Components](#frontend-components)
7. [Extending the System](#extending-the-system)

---

## Architecture Overview

MyCMS follows a **Layered Architecture** with clean separation of concerns:

```
┌─────────────────────────────────────────────────────────────┐
│                    Presentation Layer                        │
│  ┌─────────────┐  ┌──────────────┐  ┌──────────────────┐  │
│  │   Controllers│  │     Views     │  │  ViewComponents  │  │
│  └─────────────┘  └──────────────┘  └──────────────────┘  │
└─────────────────────────────────────────────────────────────┘
                              │
                              ▼
┌─────────────────────────────────────────────────────────────┐
│                     Business Layer                         │
│  ┌──────────────────────────────────────────────────────┐  │
│  │                    Services                           │  │
│  │  (PageService, ArticleService, MenuService, etc.)   │  │
│  └──────────────────────────────────────────────────────┘  │
└─────────────────────────────────────────────────────────────┘
                              │
                              ▼
┌─────────────────────────────────────────────────────────────┐
│                      Data Layer                             │
│  ┌─────────────┐  ┌──────────────┐  ┌──────────────────┐  │
│  │   DbContext  │  │   Migrations  │  │  Audit Logging   │  │
│  └─────────────┘  └──────────────┘  └──────────────────┘  │
└─────────────────────────────────────────────────────────────┘
```

### Key Principles
- **Dependency Injection**: All services injected via constructors
- **Async/Await**: All database operations are asynchronous
- **Repository Pattern**: EF Core DbContext acts as unit of work
- **Audit Trail**: Global audit logging for all data changes

---

## Project Structure

### Solution Organization

```
MyCMS-Core/
├── MyCMS.Core/           # Domain layer (Entities, Interfaces)
├── MyCMS.Data/          # Data access (DbContext, Migrations, Seeders)
├── MyCMS.Services/      # Business logic (Service implementations)
└── MyCMS.Web/           # Presentation (MVC Controllers, Views, Static files)
```

---

## Layer-by-Layer Documentation

### 1. MyCMS.Core (Domain Layer)

Location: `/MyCMS.Core/`

#### 1.1 Entities

Location: `/MyCMS.Core/Entities/`

**BaseEntity**
```csharp
public abstract class BaseEntity
{
    public Guid Id { get; set; }
    public DateTime CreatedOn { get; set; }
    public Guid? CreatedBy { get; set; }
    public DateTime? ModifiedOn { get; set; }
    public Guid? ModifiedBy { get; set; }
    public bool IsDeleted { get; set; }
}
```
**Usage:** All entities inherit from BaseEntity for consistent audit fields.

---

**ApplicationUser**
- **File:** `ApplicationUser.cs`
- **Purpose:** Extends IdentityUser<Guid> with CMS-specific fields
- **Key Properties:**
  - `FirstName`, `LastName` - User profile
  - `RegDate` - Registration date
  - `IsAuthorized` - Account status
  - `Avatar`, `Bio`, `Location` - Profile info

**Used By:**
- `AccountController` (Identity)
- `UsersController` (Admin)
- `DbInitializer` (seed admin)

---

**Article**
- **File:** `Article.cs`
- **Purpose:** Blog posts and content articles
- **Key Properties:**
  - `Title`, `Slug`, `Summary`, `Content`
  - `CategoryId` - FK to ArticleCategory
  - `AuthorId` - FK to ApplicationUser
  - `FeaturedImage` - URL to uploaded image
  - `PublishedDate`, `IsPublished`
  - `ViewCount`, `LikeCount`, `CommentCount`
  - `IsFeatured`, `AllowComments`

**Relationships:**
```
Article --> ArticleCategory (many-to-one)
Article --> ApplicationUser (many-to-one, Author)
Article <-> ArticleTag (many-to-many via ArticleTagMapping)
```

**Used By:**
- `ArticlesController` (Admin CRUD)
- `ArticleController` (Public views)
- `ArticleService` (business logic)

---

**ArticleCategory**
- **File:** `ArticleCategory.cs`
- **Purpose:** Hierarchical category system
- **Key Properties:**
  - `CategoryName`, `Slug`, `Description`
  - `ParentId` - Self-referencing for hierarchy
  - `DisplayOrder`, `IsVisible`

**Relationships:**
```
ArticleCategory --> ArticleCategory (Parent, self-ref)
ArticleCategory --> Articles (one-to-many)
```

**Used By:**
- `CategoriesController`
- `SidebarViewComponent` (category tree)

---

**ArticleTag & ArticleTagMapping**
- **Files:** `ArticleTag.cs`, `ArticleTagMapping.cs`
- **Purpose:** Tag system for articles

**Used By:**
- `SidebarViewComponent` (tag cloud)
- Article detail pages

---

**Menu & MenuItem**
- **Files:** `Menu.cs`, `MenuItem.cs`
- **Purpose:** Dynamic navigation system

**Menu Properties:**
- `MenuName`, `Position` (e.g., "Main", "Footer")
- `MenuDesc`, `IsActive`

**MenuItem Properties:**
- `MenuId`, `ParentMenuItemId` (hierarchical)
- `ItemLabel`, `ItemUrl`, `ItemTarget`
- `DisplayOrder`, `IsVisible`

**Relationships:**
```
Menu <-> MenuItems (one-to-many)
MenuItem --> MenuItem (Parent, self-ref for nesting)
```

**Used By:**
- `MenusController` (Admin)
- `MenuViewComponent` (renders in themes)
- `DbInitializer` (creates default Main menu)

---

**Page**
- **File:** `Page.cs`
- **Purpose:** Static pages (About, Contact, etc.)
- **Key Properties:**
  - `PageTitle`, `Slug`, `PageURL`, `PageContent`
  - `IsPublished`, `IsHomePage`
  - `MetaTitle`, `MetaDescription`, `MetaKeywords`

**Used By:**
- `PagesController` (Admin)
- `HomeController` (renders homepage)
- `PageService`

---

**Quiz System**
- **Files:** `Quiz.cs`, `QuizQuestion.cs`, `QuizOption.cs`, `QuizAttempt.cs`, `QuizAnswer.cs`
- **Purpose:** Interactive quiz feature

**Used By:**
- `QuizzesController` (Admin)
- `QuizController` (Public)
- `QuizService`

---

**Setting**
- **File:** `Setting.cs`
- **Purpose:** Key-value configuration storage
- **Key Properties:**
  - `Key` (e.g., "Social.Facebook")
  - `Value`, `Category` (Social, SEO, General)
  - `Description`, `IsEditable`, `IsEncrypted`

**Used By:**
- `SettingsController` (Admin)
- `SettingService`
- Themes (social media links)

---

**Theme**
- **File:** `Theme.cs`
- **Purpose:** Theme management system
- **Key Properties:**
  - `Name`, `DisplayName`, `FolderName`
  - `Thumbnail`, `Description`
  - `IsActive`, `IsDefault`
  - `LayoutOptions` (JSON config)

**Used By:**
- `ThemesController`
- `ThemeService`
- `DbInitializer` (seeds default themes)

---

**Upload**
- **File:** `Upload.cs`
- **Purpose:** File upload tracking
- **Key Properties:**
  - `FileName`, `OriginalName`, `FilePath`
  - `FileSize`, `MimeType`
  - `UploadedBy`, `AltText`, `Title`

**Used By:**
- `FilesController`
- `FileService`
- Article featured images

---

**AuditLog**
- **File:** `AuditLog.cs`
- **Purpose:** Global audit trail
- **Key Properties:**
  - `TableName`, `RecordId`
  - `Action` (Created/Updated/Deleted)
  - `OldValues`, `NewValues` (JSON)
  - `ChangeReason`, `ModifiedBy`, `IpAddress`

**Used By:**
- `AuditService`
- Any service making changes

---

#### 1.2 Interfaces (Service Contracts)

Location: `/MyCMS.Core/Interfaces/`

**ISettingService**
```csharp
public interface ISettingService
{
    Task<Setting> GetByKeyAsync(string key);
    Task<string> GetValueAsync(string key, string defaultValue = null);
    Task<T> GetValueAsync<T>(string key, T defaultValue = default);
    Task<IEnumerable<Setting>> GetByCategoryAsync(string category);
    Task<IEnumerable<Setting>> GetAllAsync();
    Task<Setting> CreateAsync(Setting setting, string changeReason = null);
    Task<Setting> UpdateAsync(Setting setting, string changeReason = null);
    Task DeleteAsync(string key, string changeReason = null);
    Task<IEnumerable<AuditLog>> GetAuditLogsAsync(string key, int count = 50);
    Task SeedDefaultSettingsAsync();
}
```

**Implementation:** `SettingService.cs` in MyCMS.Services

---

**IAuditService**
```csharp
public interface IAuditService
{
    Task LogAsync(string tableName, string recordId, string action, 
                  string oldValues, string newValues, string changeReason = null);
    Task<IEnumerable<AuditLog>> GetByTableAsync(string tableName, int count = 50);
    Task<IEnumerable<AuditLog>> GetByRecordAsync(string tableName, string recordId, int count = 50);
    Task<IEnumerable<AuditLog>> GetByUserAsync(Guid userId, int count = 50);
    Task<IEnumerable<AuditLog>> GetRecentAsync(int count = 100);
}
```

**Implementation:** `AuditService.cs` in MyCMS.Services

---

**Other Service Interfaces:**
- `IPageService` - Static page management
- `IArticleService` - Article CRUD and queries
- `ICategoryService` - Category hierarchy management
- `IMenuService` - Menu and menu item CRUD
- `IFileService` - File upload handling
- `IQuizService` - Quiz management
- `IThemeService` - Theme activation/switching
- `ISearchService` - Site search functionality

---

### 2. MyCMS.Data (Data Layer)

Location: `/MyCMS.Data/`

#### 2.1 DbContext

**AppDbContext** (Abstract Base)
- **File:** `AppDbContext.cs`
- **Purpose:** Base DbContext with all DbSets
- **Key DbSets:**
  - `Pages`, `Menus`, `MenuItems`
  - `Articles`, `ArticleCategories`, `ArticleTags`
  - `Quizzes`, `QuizQuestions`, `QuizOptions`, `QuizAttempts`
  - `Uploads`, `Themes`, `ThemeConfigurations`
  - `Settings`, `AuditLogs`
  - `Configurations`, `OpenGraphTags`

**Configuration:**
- Uses `IdentityDbContext<ApplicationUser, IdentityRole<Guid>, Guid>`
- Configures entity relationships in `OnModelCreating`

---

**PostgreSqlDbContext**
- **File:** `PostgreSqlDbContext.cs`
- **Purpose:** PostgreSQL-specific configuration
- **Inherits:** AppDbContext

**Usage:**
```csharp
// In Program.cs
builder.Services.AddDbContext<PostgreSqlDbContext>(options =>
    options.UseNpgsql(connectionString));
```

---

**SqlServerDbContext**
- **File:** `SqlServerDbContext.cs`
- **Purpose:** SQL Server-specific configuration
- **Inherits:** AppDbContext

---

#### 2.2 Services

**DbInitializer**
- **File:** `Services/DbInitializer.cs`
- **Purpose:** Seeds default data on application startup
- **Interface:** `IDbInitializer`

**Methods:**
```csharp
Task SeedAsync()
├── SeedRolesAsync()          // Creates Admin, User roles
├── SeedAdminUserAsync()      // Creates admin@mycms.com
├── SeedDefaultMenuAsync()    // Creates "Main Menu"
├── SeedThemesAsync()         // Creates default themes
└── _settingService.SeedDefaultSettingsAsync() // Social, SEO, General settings
```

**Configuration:** Reads `DefaultAdminPassword` from appsettings or generates random.

---

#### 2.3 Seed Data

**RoleSeedData**
- Creates "Admin" and "User" roles with fixed GUIDs

**ThemeSeedData**
- Seeds Minimal, Blog, Magazine, Modern themes

---

### 3. MyCMS.Services (Business Layer)

Location: `/MyCMS.Services/`

All services implement interfaces from MyCMS.Core.Interfaces.

#### 3.1 SettingService

**File:** `SettingService.cs`

**Constructor Dependencies:**
- `AppDbContext _context`
- `UserManager<ApplicationUser> _userManager`
- `IAuditService _auditService`
- `IHttpContextAccessor _httpContextAccessor`

**Key Methods:**

```csharp
// Get value with fallback default
Task<string> GetValueAsync(string key, string defaultValue = null)

// Get and deserialize JSON value
Task<T> GetValueAsync<T>(string key, T defaultValue = default)

// Get by category filter
Task<IEnumerable<Setting>> GetByCategoryAsync(string category)

// Create with audit
Task<Setting> CreateAsync(Setting setting, string changeReason = null)

// Update with audit
Task<Setting> UpdateAsync(Setting setting, string changeReason = null)

// Delete with audit
Task DeleteAsync(string key, string changeReason = null)

// Get audit history
Task<IEnumerable<AuditLog>> GetAuditLogsAsync(string key, int count = 50)
```

**Usage Example:**
```csharp
// In a controller or view
var facebookUrl = await _settingService.GetValueAsync("Social.Facebook", "");

// In a theme
var siteName = await _settingService.GetValueAsync("General.SiteName", "MyCMS");
```

---

#### 3.2 AuditService

**File:** `AuditService.cs`

**Purpose:** Generic audit logging for any entity.

**Constructor Dependencies:**
- `AppDbContext _context`
- `IHttpContextAccessor _httpContextAccessor`

**Key Methods:**

```csharp
// Log any change
Task LogAsync(string tableName, string recordId, string action, 
              string oldValues, string newValues, string changeReason = null)

// Query methods
Task<IEnumerable<AuditLog>> GetByTableAsync(string tableName, int count = 50)
Task<IEnumerable<AuditLog>> GetByRecordAsync(string tableName, string recordId, int count = 50)
Task<IEnumerable<AuditLog>> GetByUserAsync(Guid userId, int count = 50)
Task<IEnumerable<AuditLog>> GetRecentAsync(int count = 100)
```

**Usage in Other Services:**
```csharp
// In SettingService.CreateAsync:
await _auditService.LogAsync(
    "Settings",           // table name
    setting.Key,          // record id
    "Created",            // action
    null,                 // old values (none for create)
    JsonSerializer.Serialize(new { setting.Value, setting.Category }), // new values
    changeReason          // reason from user
);
```

---

#### 3.3 Other Services

All services follow similar patterns:

| Service | Key Methods | Used By |
|---------|-------------|---------|
| **PageService** | `GetBySlugAsync`, `CreateAsync`, `UpdateAsync`, `DeleteAsync` | PagesController, HomeController |
| **ArticleService** | `GetPublishedAsync`, `GetByCategoryAsync`, `CreateAsync`, `IncrementViewCountAsync` | ArticlesController, ArticleController, HomeController |
| **CategoryService** | `GetTreeAsync`, `GetBySlugAsync`, `CreateAsync` | CategoriesController, SidebarViewComponent |
| **MenuService** | `GetByPositionAsync`, `CreateMenuItemAsync`, `ReorderItemsAsync` | MenusController, MenuViewComponent |
| **FileService** | `UploadAsync`, `DeleteAsync`, `GetByIdAsync` | FilesController, ArticlesController |
| **QuizService** | `GetPublishedAsync`, `StartAttemptAsync`, `SubmitAnswerAsync`, `GetResultsAsync` | QuizzesController, QuizController |
| **ThemeService** | `GetActiveThemeAsync`, `GetAllAsync`, `ActivateThemeAsync` | ThemesController, BaseController |
| **SearchService** | `SearchAsync`, `GetSuggestionsAsync` | SearchController |

---

### 4. MyCMS.Web (Presentation Layer)

Location: `/MyCMS.Web/`

#### 4.1 Public Controllers

Location: `/Controllers/`

**HomeController**
- **Routes:** `/`, `/Home/Index`
- **Purpose:** Homepage display
- **Key Methods:**
  - `Index()` - Shows homepage with featured articles or configured landing page
  - `Error()` - Error page handler

**Dependencies:**
- `IPageService`, `IArticleService`, `IThemeService`

---

**ArticleController**
- **Routes:** 
  - `/{category}/{slug}` - Article detail (via route config)
  - `/Article/Index` - Article listing
- **Purpose:** Public article browsing

**Key Methods:**
```csharp
Task<IActionResult> Index(int? page, string category)
Task<IActionResult> Details(string category, string slug)
Task<IActionResult> ByTag(string tag)
```

---

**PageController**
- **Routes:** `/Page/{slug}`
- **Purpose:** Renders static pages

**Key Methods:**
```csharp
Task<IActionResult> Show(string slug)
```

---

**SearchController**
- **Routes:** `/Search`
- **Purpose:** Site search

---

#### 4.2 Admin Controllers (Area)

Location: `/Areas/Admin/Controllers/`

All Admin controllers inherit from `Controller` and use:
```csharp
[Area("Admin")]
[Authorize(Roles = "Admin")]
```

**DashboardController**
- **Route:** `/Admin`
- **Purpose:** Admin dashboard with statistics
- **View:** Statistics cards (Pages, Articles, Menus, Users, etc.)

**Key Methods:**
```csharp
Task<IActionResult> Index()
// Queries all entities for counts
```

---

**SettingsController**
- **Routes:** 
  - `/Admin/Settings` - List all settings
  - `/Admin/Settings/Create` - Add new setting
  - `/Admin/Settings/Edit?key={key}` - Edit setting
  - `/Admin/Settings/AuditLog?key={key}` - View audit history

**Dependencies:** `ISettingService`

**Key Methods:**
```csharp
Task<IActionResult> Index(string category)  // Filter by category
Task<IActionResult> Create()
Task<IActionResult> Create(Setting setting, string changeReason)
Task<IActionResult> Edit(string key)
Task<IActionResult> Edit(Setting setting, string changeReason)
Task<IActionResult> AuditLog(string key, int count)
Task<IActionResult> Delete(string key, string changeReason)
```

---

**ArticlesController**
- **Routes:** `/Admin/Articles/*`
- **Purpose:** Article CRUD management

**Key Methods:**
```csharp
Task<IActionResult> Index(string search, int? page)
Task<IActionResult> Create()
Task<IActionResult> Create(Article model, IFormFile featuredImage)
Task<IActionResult> Edit(Guid id)
Task<IActionResult> Edit(Article model, IFormFile featuredImage)
Task<IActionResult> Delete(Guid id)
```

---

**Other Admin Controllers:**

| Controller | Route | Purpose |
|------------|-------|---------|
| **PagesController** | `/Admin/Pages/*` | Static page CRUD |
| **MenusController** | `/Admin/Menus/*` | Menu and menu item CRUD |
| **CategoriesController** | `/Admin/Categories/*` | Category hierarchy |
| **FilesController** | `/Admin/Files/*` | File upload manager |
| **QuizzesController** | `/Admin/Quizzes/*` | Quiz CRUD |
| **UsersController** | `/Admin/Users/*` | User management |
| **ThemesController** | `/Admin/Themes/*` | Theme activation |
| **ConfigurationController** | `/Admin/Configuration/*` | Legacy site config |
| **OpenGraphTagsController** | `/Admin/OpenGraphTags/*` | SEO meta tags |

---

#### 4.3 Identity Controllers

Location: `/Areas/Identity/Pages/Account/`

Scaffolded ASP.NET Core Identity pages:
- `Login.cshtml` - User authentication
- `Register.cshtml` - New user registration
- `Logout.cshtml` - Sign out
- `Manage/` - Profile management

**Modified:**
- `Register.cshtml.cs` - Automatically assigns "User" role on registration

---

#### 4.4 ViewComponents

Location: `/ViewComponents/`

**MenuViewComponent**
- **File:** `MenuViewComponent.cs`
- **Purpose:** Renders dynamic menus in themes

**Usage in Views:**
```razor
@await Component.InvokeAsync("Menu", new { position = "Main" })
```

**Key Methods:**
```csharp
Task<IViewComponentResult> InvokeAsync(string position)
// Fetches menu items by position and renders Default.cshtml
```

**View Location:** `/Views/Shared/Components/Menu/Default.cshtml`

---

**SidebarViewComponent**
- **File:** `SidebarViewComponent.cs`
- **Purpose:** Renders sidebar with categories, recent posts, tags

**Usage:**
```razor
@await Component.InvokeAsync("Sidebar")
```

**Key Methods:**
```csharp
Task<IViewComponentResult> InvokeAsync()
├── BuildCategoryTreeAsync()    // Hierarchical categories
├── GetRecentPostsAsync()       // Latest 5 articles
└── GetTagsAsync()              // Tag cloud
```

---

**Other ViewComponents:**
- `ThemeSelectorViewComponent` - Theme switching dropdown

---

#### 4.5 Themes System

Location: `/Views/Themes/`

**Structure:**
```
Views/Themes/
├── Minimal/
│   ├── Shared/
│   │   ├── _Layout.cshtml      # Theme layout
│   │   └── _Header.cshtml      # Theme header
│   └── Home/
│       └── Index.cshtml        # Theme-specific homepage
├── Blog/
│   └── ...
├── Magazine/
│   └── ...
└── Modern/
    └── ...
```

**Theme Resolution:**
- `ThemeService.GetActiveThemeAsync()` determines current theme
- Controllers use theme-specific views if available
- Falls back to default views if theme view not found

---

## Entity Framework & Data Access

### Database Providers

**PostgreSQL (Default)**
```csharp
// Program.cs
builder.Services.AddDbContext<PostgreSqlDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("PostgreSQL")));
```

**SQL Server**
```csharp
builder.Services.AddDbContext<SqlServerDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("SqlServer")));
```

### Migrations

**Create Migration:**
```bash
dotnet ef migrations add MigrationName \
  --project MyCMS.Data \
  --startup-project MyCMS.Web \
  --context PostgreSqlDbContext
```

**Update Database:**
```bash
dotnet ef database update \
  --project MyCMS.Data \
  --startup-project MyCMS.Web \
  --context PostgreSqlDbContext
```

### Seeding

Automatic seeding happens via `DbInitializer.SeedAsync()` on startup:
1. Roles (Admin, User)
2. Default admin user
3. Default menu
4. Default themes
5. Default settings

---

## Authentication & Authorization

### Identity Configuration

**Program.cs:**
```csharp
builder.Services.AddIdentity<ApplicationUser, IdentityRole<Guid>>(options =>
{
    options.Password.RequireDigit = true;
    options.Password.RequireLowercase = true;
    options.Password.RequireUppercase = true;
    options.Password.RequireNonAlphanumeric = true;
    options.Password.RequiredLength = 8;
})
.AddEntityFrameworkStores<PostgreSqlDbContext>() // or SqlServerDbContext
.AddDefaultTokenProviders();
```

### Roles

**Default Roles:**
- **Admin** - Full access to `/Admin` area
- **User** - Standard user for public features

**Assigning Roles:**
```csharp
// Registration automatically assigns "User"
await _userManager.AddToRoleAsync(user, "User");

// Admin assignment (via admin panel or seed)
await _userManager.AddToRoleAsync(user, "Admin");
```

### Protecting Controllers

```csharp
[Area("Admin")]
[Authorize(Roles = "Admin")]  // Requires Admin role
public class SettingsController : Controller
```

---

## Frontend Components

### Admin Layout

Location: `/Areas/Admin/Views/Shared/_AdminLayout.cshtml`

**Features:**
- Sidebar navigation with icons
- Collapsible menu sections
- Active state highlighting
- Bootstrap 5 styling

**Navigation Sections:**
1. Content (Articles, Pages, Categories)
2. Media (Files)
3. Site Structure (Menus)
4. Interactive (Quizzes)
5. System (Users, Configuration, Settings, Themes, OpenGraph)

### Theme Integration

**Using Settings in Themes:**
```razor
@inject ISettingService SettingService

@{
    var facebookUrl = await SettingService.GetValueAsync("Social.Facebook", "");
    var siteName = await SettingService.GetValueAsync("General.SiteName", "MyCMS");
}

<a href="@facebookUrl">Facebook</a>
<title>@siteName</title>
```

---

## Extending the System

### Adding a New Entity

1. **Create Entity** in `MyCMS.Core/Entities/`
```csharp
public class CustomEntity : BaseEntity
{
    public string Name { get; set; }
    // ... properties
}
```

2. **Add DbSet** in `AppDbContext.cs`
```csharp
public DbSet<CustomEntity> CustomEntities { get; set; }
```

3. **Create Interface** in `MyCMS.Core/Interfaces/`
```csharp
public interface ICustomService
{
    Task<CustomEntity> GetByIdAsync(Guid id);
    Task CreateAsync(CustomEntity entity);
    // ... methods
}
```

4. **Implement Service** in `MyCMS.Services/`
```csharp
public class CustomService : ICustomService
{
    private readonly AppDbContext _context;
    private readonly IAuditService _auditService;
    // ... constructor and methods with audit logging
}
```

5. **Register Service** in `Program.cs`
```csharp
builder.Services.AddScoped<ICustomService, CustomService>();
```

6. **Create Controller** in `MyCMS.Web/Areas/Admin/Controllers/`
```csharp
[Area("Admin")]
[Authorize(Roles = "Admin")]
public class CustomEntitiesController : Controller
{
    private readonly ICustomService _service;
    // ... CRUD actions
}
```

7. **Create Views** in `MyCMS.Web/Areas/Admin/Views/CustomEntities/`

8. **Add Migration**
```bash
dotnet ef migrations add AddCustomEntity --project MyCMS.Data --startup-project MyCMS.Web --context PostgreSqlDbContext
```

---

### Adding Audit to Existing Services

Inject `IAuditService` and log changes:

```csharp
public async Task UpdateAsync(Entity entity, string changeReason)
{
    var existing = await GetByIdAsync(entity.Id);
    var oldValues = JsonSerializer.Serialize(existing);
    
    // ... update logic
    
    await _auditService.LogAsync(
        "Entities",           // table name
        entity.Id.ToString(), // record id
        "Updated",            // action
        oldValues,            // old values (JSON)
        JsonSerializer.Serialize(entity), // new values
        changeReason          // user-provided reason
    );
}
```

---

## Common Tasks Reference

### Get Current User

```csharp
var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
var userName = User.Identity?.Name;
```

### Access HttpContext in Services

```csharp
public class MyService
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    
    public MyService(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }
    
    public void DoSomething()
    {
        var ipAddress = _httpContextAccessor.HttpContext?.Connection?.RemoteIpAddress?.ToString();
        var userAgent = _httpContextAccessor.HttpContext?.Request?.Headers["User-Agent"].ToString();
    }
}
```

### TempData for Redirect Messages

```csharp
TempData["Success"] = "Setting updated successfully.";
TempData["Error"] = "Failed to update setting.";
```

### ModelState Validation

```csharp
if (!ModelState.IsValid)
    return View(model);
```

---

## Troubleshooting

### Common Issues

**1. DbContext Not Registered**
- Ensure `AddDbContext<>` is called in Program.cs
- Check connection string in appsettings.json

**2. Service Not Found**
- Verify `AddScoped<Interface, Implementation>()` in Program.cs
- Check constructor injection in controller

**3. Migration Errors**
- Delete `/Migrations` folder
- Run `dotnet ef migrations add InitialCreate --context YourDbContext`

**4. Audit Not Logging**
- Ensure `AddHttpContextAccessor()` is in Program.cs
- Check `IAuditService` is injected and `LogAsync` is called

---

## Additional Resources

- **ASP.NET Core Docs:** https://docs.microsoft.com/aspnet/core/
- **EF Core Docs:** https://docs.microsoft.com/ef/core/
- **Bootstrap 5:** https://getbootstrap.com/docs/5.3/

---

*End of Developer Documentation*
