using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MyCMS.Core.Entities;
using MyCMS.Core.Interfaces;

namespace MyCMS.Web.Areas.Admin.Pages.Configuration
{
    [Authorize]
    public class IndexModel : PageModel
    {
        private readonly IConfigurationService _configService;

        public IndexModel(IConfigurationService configService)
        {
            _configService = configService;
        }

        [BindProperty]
        public Configuration Config { get; set; } = new Configuration();

        public async Task OnGetAsync()
        {
            var config = await _configService.GetConfigurationAsync("Default");
            if (config != null)
            {
                Config = config;
            }
            else
            {
                Config.SectionName = "Default";
            }
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            Config.SectionName = "Default";
            await _configService.UpdateConfigurationAsync(Config);
            return RedirectToPage("./Index");
        }
    }
}
