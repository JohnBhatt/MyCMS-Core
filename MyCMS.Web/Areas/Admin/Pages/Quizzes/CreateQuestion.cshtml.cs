using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MyCMS.Core.Entities;
using MyCMS.Core.Interfaces;

namespace MyCMS.Web.Areas.Admin.Pages.Quizzes
{
    [Authorize]
    public class CreateQuestionModel : PageModel
    {
        private readonly IQuizService _quizService;

        public CreateQuestionModel(IQuizService quizService)
        {
            _quizService = quizService;
        }

        [BindProperty]
        public QuizQuestion Question { get; set; } = new QuizQuestion();

        public Guid QuizId { get; set; }

        public void OnGet(Guid quizId)
        {
            QuizId = quizId;
        }

        public async Task<IActionResult> OnPostAsync(Guid quizId)
        {
            if (!ModelState.IsValid)
            {
                QuizId = quizId;
                return Page();
            }

            Question.QuizId = quizId;
            await _quizService.CreateQuizQuestionAsync(Question);
            return RedirectToPage("./Questions", new { quizId = quizId });
        }
    }
}
