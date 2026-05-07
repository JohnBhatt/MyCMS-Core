# MyCMS

A modern Content Management System built with ASP.NET Core 10.0, featuring dual database support (PostgreSQL and SQL Server), ASP.NET Core Identity with Guid keys, Bootstrap 5.3, and Quill.js rich text editor.

## Features

- **Dual Database Support**: PostgreSQL and SQL Server with EF Core 10.0
- **ASP.NET Core Identity**: Guid-based primary keys with role-based authorization
- **Modern UI**: Bootstrap 5.3 with Quill.js rich text editor
- **Admin Area**: Complete CRUD operations for Pages, Articles, Menus, Files, Quizzes, Configuration, OpenGraph Tags, and Users
- **Public Frontend**: Homepage, Article listing, Article details, and custom pages
- **SEO Features**: Sitemap.xml, RSS feeds, and AMP pages
- **Quiz System**: Create quizzes with questions, options, and track user attempts

## Prerequisites

- .NET 10.0 SDK
- PostgreSQL 16 or SQL Server 2019+
- Node.js (for frontend dependencies, if any)

## Getting Started

### 1. Clone the Repository

```bash
git clone <repository-url>
cd MyCMS-New
```

### 2. Restore Dependencies

```bash
dotnet restore
```

### 3. Configure Connection Strings

Edit `appsettings.json` to configure your database connection:

```json
{
  "DatabaseProvider": "PostgreSQL",
  "ConnectionStrings": {
    "PostgreSQL": "Host=localhost;Port=5432;Database=mycms;Username=postgres;Password=your_password",
    "SqlServer": "Server=localhost;Database=MyCMS;User Id=sa;Password=YourStrong@Password;TrustServerCertificate=True"
  }
}
```

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
cd MyCMS.Web
dotnet run
```

Access the application at `https://localhost:5001`

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

### Linux VPS Deployment with Docker

1. Build and run with Docker Compose:
```bash
docker-compose up -d
```

2. The application will be available at `http://localhost`

3. Update the PostgreSQL password in `docker-compose.yml` before deployment.

### Manual Linux Deployment

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

6. Configure Nginx as a reverse proxy (optional).

## Project Structure

- `MyCMS.Core`: Domain entities and interfaces
- `MyCMS.Data`: Entity Framework Core context and migrations
- `MyCMS.Services`: Business logic implementations
- `MyCMS.Web`: ASP.NET Core Razor Pages application
  - `Areas/Admin`: Admin area with CRUD operations
  - `Pages`: Public frontend pages

## Admin Access

After running the application, navigate to `/Admin` to access the admin panel. You'll need to create the first admin user through the User management page.

## License

MIT License
