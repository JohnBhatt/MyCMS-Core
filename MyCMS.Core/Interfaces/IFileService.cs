using Microsoft.AspNetCore.Http;
using MyCMS.Core.Entities;

namespace MyCMS.Core.Interfaces
{
    public interface IFileService
    {
        Task<List<Upload>> GetAllUploadsAsync();
        Task<Upload?> GetUploadByIdAsync(Guid id);
        Task<Upload> UploadFileAsync(Upload upload);
        Task DeleteUploadAsync(Guid id);
        Task<string> SaveFileAsync(IFormFile file, string folder);
        Task<byte[]?> GetFileBytesAsync(string filePath);
    }
}
