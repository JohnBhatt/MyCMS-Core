using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MyCMS.Core.Entities;
using MyCMS.Core.Interfaces;

namespace MyCMS.Web.Areas.Admin.Pages.Files
{
    [Authorize]
    public class IndexModel : PageModel
    {
        private readonly IFileService _fileService;

        public IndexModel(IFileService fileService)
        {
            _fileService = fileService;
        }

        public List<Upload> Uploads { get; set; } = new List<Upload>();

        public async Task OnGetAsync()
        {
            Uploads = await _fileService.GetAllUploadsAsync();
        }
    }
}
