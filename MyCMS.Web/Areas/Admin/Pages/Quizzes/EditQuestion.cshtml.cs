using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MyCMS.Core.Entities;
using MyCMS.Core.Interfaces;

namespace MyCMS.Web.Areas.Admin.Pages.Quizzes
{
    [Authorize]
    public class EditQuestionModel : PageModel
    {
        private readonly IQuizService _quizService;

        public EditQuestionModel(IQuizService quizService)
        {
            _quizService = quizService;
        }

        [BindProperty]
        public QuizQuestion Question { get; set; } = new QuizQuestion();

        public async Task<IActionResult> OnGetAsync(Guid id)
        {
            var questions = await _quizService.GetQuizQuestionsAsync(Guid.Empty);
            var question = questions.FirstOrDefault(q => q.Id == id);
            if (question == null)
            {
                return NotFound();
            }

            Question = question;
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            await _quizService.UpdateQuizQuestionAsync(Question);
            return RedirectToPage("./Questions", new { quizId = Question.QuizId });
        }
    }
}
