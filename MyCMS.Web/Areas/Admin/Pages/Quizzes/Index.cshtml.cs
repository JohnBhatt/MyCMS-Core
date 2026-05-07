using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MyCMS.Core.Entities;
using MyCMS.Core.Interfaces;

namespace MyCMS.Web.Areas.Admin.Pages.Quizzes
{
    [Authorize]
    public class IndexModel : PageModel
    {
        private readonly IQuizService _quizService;

        public IndexModel(IQuizService quizService)
        {
            _quizService = quizService;
        }

        public List<Quiz> Quizzes { get; set; } = new List<Quiz>();

        public async Task OnGetAsync()
        {
            Quizzes = await _quizService.GetAllQuizzesAsync();
        }
    }
}
