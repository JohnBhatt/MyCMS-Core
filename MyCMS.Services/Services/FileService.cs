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

        public FileService(AppDbContext context, IWebHostEnvironment environment)
        {
            _context = context;
            _environment = environment;
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
            return upload;
        }

        public async Task DeleteUploadAsync(Guid id)
        {
            var upload = await _context.Uploads.FindAsync(id);
            if (upload != null)
            {
                upload.IsDeleted = true;
                upload.ModifiedOn = DateTime.UtcNow;
                await _context.SaveChangesAsync();
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
