# MyCMS

A modern, full-featured Content Management System built with ASP.NET Core 10.0 using the MVC architecture pattern. Features dual database support (PostgreSQL and SQL Server), ASP.NET Core Identity with Guid keys, Bootstrap 5.3, and Quill.js rich text editor.

## Features

### Core Features
- **MVC Architecture**: Clean separation of concerns with Controllers, Views, and Areas
- **Dual Database Support**: PostgreSQL and SQL Server with EF Core 10.0
- **ASP.NET Core Identity**: Guid-based primary keys with role-based authorization
- **Modern UI**: Bootstrap 5.3 with Quill.js rich text editor

### Admin Area Features
- **Dashboard**: Overview of system statistics
- **User Management**: Create, edit, and manage users with roles
- **Page Management**: Create and manage static pages with rich text content
- **Article Management**: Full CRUD for articles with categories, tags, and featured images
- **Menu Management**: Create dynamic menus with menu items
- **File Management**: Upload and manage media files
- **Quiz System**: Create quizzes with questions, options, and track user attempts
- **Configuration**: Site-wide settings management
- **OpenGraph Tags**: SEO meta tags for social media sharing

### Public Frontend Features
- **Homepage**: Configurable homepage with featured articles
- **Article Listing**: Browse articles with pagination
- **Article Details**: Full article view with comments (planned)
- **Custom Pages**: Static pages with SEO-friendly URLs
- **Dynamic Menus**: Rendered via View Components
- **RSS Feeds**: Auto-generated RSS feed for articles
- **Sitemap**: Auto-generated XML sitemap for SEO

## Tech Stack

- **.NET 10.0** - Latest .NET framework
- **ASP.NET Core MVC** - Web framework
- **Entity Framework Core 10.0** - ORM for data access
- **PostgreSQL 16+ / SQL Server 2019+** - Database options
- **ASP.NET Core Identity** - Authentication and authorization
- **Bootstrap 5.3** - UI framework
- **Quill.js** - Rich text editor
- **jQuery** - JavaScript library

## Prerequisites

- [.NET 10.0 SDK](https://dotnet.microsoft.com/download/dotnet/10.0)
- PostgreSQL 16+ or SQL Server 2019+
- (Optional) Docker for containerized deployment

## Getting Started

### 1. Clone the Repository

```bash
git clone https://github.com/yourusername/MyCMS-Core.git
cd MyCMS-Core
```

### 2. Restore Dependencies

```bash
dotnet restore
```

### 3. Configure Connection Strings

Edit `MyCMS.Web/appsettings.json` to configure your database connection:

```json
{
  "DatabaseProvider": "PostgreSQL",
  "ConnectionStrings": {
    "PostgreSQL": "Host=localhost;Port=5432;Database=MyCMS;Username=postgres;Password=your_password",
    "SqlServer": "Server=localhost;Database=MyCMS;User Id=sa;Password=YourStrong@Password;TrustServerCertificate=True"
  }
}
```

Set `DatabaseProvider` to either `"PostgreSQL"` or `"SqlServer"` based on your preference.

### 4. Create and Apply Migrations

For PostgreSQL:
```bash
cd MyCMS.Data
dotnet ef migrations add InitialCreate --context PostgreSqlDbContext --output-dir Migrations/PostgreSql
dotnet ef database update --context PostgreSqlDbContext
```

For SQL Server:
```bash
cd MyCMS.Data
dotnet ef migrations add InitialCreate --context SqlServerDbContext --output-dir Migrations/SqlServer
dotnet ef database update --context SqlServerDbContext
```

### 5. Run the Application

```bash
dotnet watch --project MyCMS.Web
```

The application will start at `http://localhost:5221`

## Access Points

| Area | URL | Description |
|------|-----|-------------|
| Public Homepage | `/` | Main landing page |
| Admin Dashboard | `/Admin` | Admin panel |
| Login | `/Identity/Account/Login` | User authentication |
| Register | `/Identity/Account/Register` | New user registration |
| Profile | `/Identity/Account/Manage` | User profile management |

## Project Structure

```
MyCMS-Core/
├── MyCMS.Core/              # Domain layer
│   ├── Entities/           # Domain entities (Article, Page, User, etc.)
│   └── Interfaces/         # Service interfaces
├── MyCMS.Data/             # Data access layer
│   ├── Migrations/         # EF Core migrations
│   ├── AppDbContext.cs     # Abstract base DbContext
│   ├── PostgreSqlDbContext.cs
│   └── SqlServerDbContext.cs
├── MyCMS.Services/         # Business logic layer
│   ├── PageService.cs
│   ├── ArticleService.cs
│   ├── MenuService.cs
│   └── ...
└── MyCMS.Web/              # Presentation layer (MVC)
    ├── Controllers/        # Public controllers
    │   ├── HomeController.cs
    │   ├── ArticleController.cs
    │   └── ...
    ├── Areas/              # Areas for organization
    │   └── Admin/          # Admin area
    │       ├── Controllers/
    │       │   ├── DashboardController.cs
    │       │   ├── ArticlesController.cs
    │       │   └── ...
    │       └── Views/
    │           ├── Dashboard/
    │           ├── Articles/
    │           └── Shared/
    │               └── _AdminLayout.cshtml
    ├── Areas/Identity/     # Identity pages (scaffolded)
    │   └── Pages/
    │       └── Account/
    ├── Views/              # Public views
    │   ├── Home/
    │   ├── Article/
    │   └── Shared/
    │       ├── _Layout.cshtml
    │       └── ...
    ├── ViewComponents/     # Reusable view components
    │   └── MenuViewComponent.cs
    └── wwwroot/            # Static files
        ├── css/
        ├── js/
        └── uploads/
```

## Creating the First Admin User

1. Navigate to `/Identity/Account/Register` to create a new user
2. After registration, manually update the user's role in the database or use the Admin panel's User Management to assign admin role
3. Alternatively, create a user directly via the Admin panel's User Management page

## Development

### Hot Reload

Use `dotnet watch` for hot reload during development:

```bash
dotnet watch --project MyCMS.Web
```

Note: Route configuration changes in `Program.cs` require a full restart.

### Adding New Features

1. **Add Entity**: Create entity in `MyCMS.Core/Entities/`
2. **Add DbSet**: Add to `AppDbContext.cs`
3. **Create Migration**: Run `dotnet ef migrations add`
4. **Create Service**: Implement interface in `MyCMS.Services/`
5. **Register Service**: Add to DI in `Program.cs`
6. **Create Controller**: Add controller in appropriate area
7. **Create Views**: Add views in corresponding Views folder

### Code Style

- Follow C# naming conventions (PascalCase for public members)
- Use async/await for database operations
- Use dependency injection for services
- Keep controllers thin - business logic in services

## Deployment

### Windows IIS Deployment

1. Publish the application:
```bash
dotnet publish -c Release -o ./publish
```

2. Copy the contents of the `publish` folder to your IIS server.

3. Configure IIS to point to the published folder.

4. Update `appsettings.Production.json` with production connection strings.

5. Ensure the Application Pool identity has write permissions to the `wwwroot/uploads` folder.

### Linux VPS Deployment

1. Publish the application:
```bash
dotnet publish -c Release -o ./publish
```

2. Copy the `publish` folder to your Linux server.

3. Install the ASP.NET Core Runtime:
```bash
sudo apt-get update
sudo apt-get install -y aspnetcore-runtime-10.0
```

4. Create a systemd service file `/etc/systemd/system/mycms.service`:
```ini
[Unit]
Description=MyCMS Web Application
After=network.target

[Service]
WorkingDirectory=/var/www/mycms
ExecStart=/usr/bin/dotnet /var/www/mycms/MyCMS.Web.dll
Restart=always
RestartSec=10
Environment=ASPNETCORE_ENVIRONMENT=Production

[Install]
WantedBy=multi-user.target
```

5. Enable and start the service:
```bash
sudo systemctl enable mycms
sudo systemctl start mycms
```

6. Configure Nginx as a reverse proxy (recommended):
```nginx
server {
    listen 80;
    server_name your-domain.com;
    
    location / {
        proxy_pass http://localhost:5000;
        proxy_http_version 1.1;
        proxy_set_header Upgrade $http_upgrade;
        proxy_set_header Connection keep-alive;
        proxy_set_header Host $host;
        proxy_cache_bypass $http_upgrade;
    }
}
```

### Docker Deployment

Create a `Dockerfile`:
```dockerfile
FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src
COPY . .
RUN dotnet restore
RUN dotnet publish -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=build /src/publish .
ENTRYPOINT ["dotnet", "MyCMS.Web.dll"]
```

Build and run:
```bash
docker build -t mycms .
docker run -p 80:80 -e DatabaseProvider=PostgreSQL -e ConnectionStrings__PostgreSQL="..." mycms
```

## Contributing

We welcome contributions! Please follow these guidelines:

1. **Fork the repository**
2. **Create a feature branch**: `git checkout -b feature/your-feature-name`
3. **Make your changes** following the project structure and coding conventions
4. **Test thoroughly** - ensure all tests pass and the application builds
5. **Commit your changes** with descriptive messages
6. **Push to your fork**: `git push origin feature/your-feature-name`
7. **Submit a pull request** with a clear description of your changes

### Contribution Guidelines

- Keep code clean and well-commented
- Follow existing code style and patterns
- Add tests for new features (when applicable)
- Update documentation as needed
- Ensure backward compatibility when possible

## Roadmap

- [ ] Comments system for articles
- [ ] Tag-based article filtering
- [ ] Email notifications
- [ ] Multi-language support
- [ ] API endpoints for headless CMS
- [ ] Theme system
- [ ] Plugin architecture

## License

MIT License - feel free to use this project for personal or commercial purposes.

## Support

For issues, questions, or contributions:
- Open an issue on GitHub
- Check existing documentation
- Review the code comments

## Acknowledgments

- Built with ASP.NET Core
- UI powered by Bootstrap 5
- Rich text editing by Quill.js
- Database management via Entity Framework Core
