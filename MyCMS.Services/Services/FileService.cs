using System.Text.Json;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using MyCMS.Core.Entities;
using MyCMS.Core.Interfaces;
using MyCMS.Data;

namespace MyCMS.Services
{
    public class FileService : IFileService
    {
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _environment;
        private readonly IAuditService _auditService;

        public FileService(AppDbContext context, IWebHostEnvironment environment, IAuditService auditService)
        {
            _context = context;
            _environment = environment;
            _auditService = auditService;
        }

        public async Task<List<Upload>> GetAllUploadsAsync()
        {
            return await _context.Uploads.ToListAsync();
        }

        public async Task<Upload?> GetUploadByIdAsync(Guid id)
        {
            return await _context.Uploads.FindAsync(id);
        }

        public async Task<Upload> UploadFileAsync(Upload upload)
        {
            upload.Id = Guid.NewGuid();
            upload.CreatedOn = DateTime.UtcNow;
            _context.Uploads.Add(upload);
            await _context.SaveChangesAsync();
            
            await _auditService.LogAsync("Uploads", upload.Id.ToString(), "Created", null,
                JsonSerializer.Serialize(new { upload.DocumentName, upload.DocumentType, upload.FilePath }), "File uploaded");
            return upload;
        }

        public async Task DeleteUploadAsync(Guid id)
        {
            var upload = await _context.Uploads.FindAsync(id);
            if (upload != null)
            {
                var oldValues = JsonSerializer.Serialize(new { upload.DocumentName, upload.FilePath });
                upload.IsDeleted = true;
                upload.ModifiedOn = DateTime.UtcNow;
                await _context.SaveChangesAsync();
                
                await _auditService.LogAsync("Uploads", id.ToString(), "Deleted", oldValues, null, "File deleted");
            }
        }

        public async Task<string> SaveFileAsync(IFormFile file, string folder)
        {
            var uploadsFolder = Path.Combine(_environment.WebRootPath, "uploads", folder);
            Directory.CreateDirectory(uploadsFolder);

            var uniqueFileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
            var filePath = Path.Combine(uploadsFolder, uniqueFileName);

            using (var fileStream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(fileStream);
            }

            return $"/uploads/{folder}/{uniqueFileName}";
        }

        public async Task<byte[]?> GetFileBytesAsync(string filePath)
        {
            var fullPath = Path.Combine(_environment.WebRootPath, filePath.TrimStart('/'));
            if (!File.Exists(fullPath))
                return null;

            return await File.ReadAllBytesAsync(fullPath);
        }
    }
}
