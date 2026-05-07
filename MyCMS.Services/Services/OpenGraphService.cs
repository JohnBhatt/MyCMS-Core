using Microsoft.EntityFrameworkCore;
using MyCMS.Core.Entities;
using MyCMS.Core.Interfaces;
using MyCMS.Data;

namespace MyCMS.Services
{
    public class OpenGraphService : IOpenGraphService
    {
        private readonly AppDbContext _context;

        public OpenGraphService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<OpenGraphTag>> GetAllTagsAsync()
        {
            return await _context.OpenGraphTags.ToListAsync();
        }

        public async Task<OpenGraphTag?> GetTagByIdAsync(Guid id)
        {
            return await _context.OpenGraphTags.FindAsync(id);
        }

        public async Task<OpenGraphTag> CreateTagAsync(OpenGraphTag tag)
        {
            tag.Id = Guid.NewGuid();
            tag.CreatedOn = DateTime.UtcNow;
            _context.OpenGraphTags.Add(tag);
            await _context.SaveChangesAsync();
            return tag;
        }

        public async Task<OpenGraphTag> UpdateTagAsync(OpenGraphTag tag)
        {
            tag.ModifiedOn = DateTime.UtcNow;
            _context.OpenGraphTags.Update(tag);
            await _context.SaveChangesAsync();
            return tag;
        }

        public async Task DeleteTagAsync(Guid id)
        {
            var tag = await _context.OpenGraphTags.FindAsync(id);
            if (tag != null)
            {
                tag.IsDeleted = true;
                tag.ModifiedOn = DateTime.UtcNow;
                await _context.SaveChangesAsync();
            }
        }
    }
}
