using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MyCMS.Core.Entities;
using MyCMS.Core.Interfaces;

namespace MyCMS.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize]
    public class QuizzesController : Controller
    {
        private readonly IQuizService _quizService;

        public QuizzesController(IQuizService quizService)
        {
            _quizService = quizService;
        }

        public async Task<IActionResult> Index()
        {
            var quizzes = await _quizService.GetAllQuizzesAsync();
            return View(quizzes);
        }

        public IActionResult Create()
        {
            return View(new Quiz());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Quiz quiz)
        {
            if (!ModelState.IsValid)
            {
                return View(quiz);
            }

            await _quizService.CreateQuizAsync(quiz);
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Edit(Guid id)
        {
            var quiz = await _quizService.GetQuizByIdAsync(id);
            if (quiz == null)
            {
                return NotFound();
            }

            return View(quiz);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, Quiz quiz)
        {
            if (!ModelState.IsValid)
            {
                return View(quiz);
            }

            await _quizService.UpdateQuizAsync(quiz);
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Delete(Guid id)
        {
            var quiz = await _quizService.GetQuizByIdAsync(id);
            if (quiz == null)
            {
                return NotFound();
            }

            return View(quiz);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            await _quizService.DeleteQuizAsync(id);
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Questions(Guid id)
        {
            var quiz = await _quizService.GetQuizByIdAsync(id);
            if (quiz == null)
            {
                return NotFound();
            }

            ViewBag.QuizId = id;
            return View(quiz);
        }

        public IActionResult CreateQuestion(Guid quizId)
        {
            ViewBag.QuizId = quizId;
            return View(new QuizQuestion());
        }

        public IActionResult EditQuestion(Guid id)
        {
            return View();
        }
    }
}
