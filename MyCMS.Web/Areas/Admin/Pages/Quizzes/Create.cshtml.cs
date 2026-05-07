using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MyCMS.Core.Entities;
using MyCMS.Core.Interfaces;

namespace MyCMS.Web.Areas.Admin.Pages.Quizzes
{
    [Authorize]
    public class CreateModel : PageModel
    {
        private readonly IQuizService _quizService;

        public CreateModel(IQuizService quizService)
        {
            _quizService = quizService;
        }

        [BindProperty]
        public Quiz Quiz { get; set; } = new Quiz();

        public void OnGet()
        {
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            await _quizService.CreateQuizAsync(Quiz);
            return RedirectToPage("./Index");
        }
    }
}
