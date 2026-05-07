using MyCMS.Core.Entities;

namespace MyCMS.Core.Interfaces
{
    public interface IOpenGraphService
    {
        Task<List<OpenGraphTag>> GetAllTagsAsync();
        Task<OpenGraphTag?> GetTagByIdAsync(Guid id);
        Task<OpenGraphTag> CreateTagAsync(OpenGraphTag tag);
        Task<OpenGraphTag> UpdateTagAsync(OpenGraphTag tag);
        Task DeleteTagAsync(Guid id);
    }
}
