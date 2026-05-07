using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MyCMS.Core.Entities;
using MyCMS.Core.Interfaces;

namespace MyCMS.Web.Areas.Admin.Pages.Quizzes
{
    [Authorize]
    public class EditModel : PageModel
    {
        private readonly IQuizService _quizService;

        public EditModel(IQuizService quizService)
        {
            _quizService = quizService;
        }

        [BindProperty]
        public Quiz Quiz { get; set; } = new Quiz();

        public async Task<IActionResult> OnGetAsync(Guid id)
        {
            var quiz = await _quizService.GetQuizByIdAsync(id);
            if (quiz == null)
            {
                return NotFound();
            }

            Quiz = quiz;
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            await _quizService.UpdateQuizAsync(Quiz);
            return RedirectToPage("./Index");
        }
    }
}
