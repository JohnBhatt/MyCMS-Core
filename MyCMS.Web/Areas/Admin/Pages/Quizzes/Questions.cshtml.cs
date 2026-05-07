using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MyCMS.Core.Entities;
using MyCMS.Core.Interfaces;

namespace MyCMS.Web.Areas.Admin.Pages.Quizzes
{
    [Authorize]
    public class QuestionsModel : PageModel
    {
        private readonly IQuizService _quizService;

        public QuestionsModel(IQuizService quizService)
        {
            _quizService = quizService;
        }

        public Guid QuizId { get; set; }
        public List<QuizQuestion> Questions { get; set; } = new List<QuizQuestion>();

        public async Task OnGetAsync(Guid quizId)
        {
            QuizId = quizId;
            Questions = await _quizService.GetQuizQuestionsAsync(quizId);
        }
    }
}
