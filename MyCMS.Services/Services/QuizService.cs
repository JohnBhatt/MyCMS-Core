using Microsoft.EntityFrameworkCore;
using MyCMS.Core.Entities;
using MyCMS.Core.Interfaces;
using MyCMS.Data;

namespace MyCMS.Services
{
    public class QuizService : IQuizService
    {
        private readonly AppDbContext _context;

        public QuizService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<Quiz>> GetAllQuizzesAsync()
        {
            return await _context.Quizzes.ToListAsync();
        }

        public async Task<Quiz?> GetQuizByIdAsync(Guid id)
        {
            return await _context.Quizzes.FindAsync(id);
        }

        public async Task<Quiz> CreateQuizAsync(Quiz quiz)
        {
            quiz.Id = Guid.NewGuid();
            quiz.CreatedOn = DateTime.UtcNow;
            _context.Quizzes.Add(quiz);
            await _context.SaveChangesAsync();
            return quiz;
        }

        public async Task<Quiz> UpdateQuizAsync(Quiz quiz)
        {
            quiz.ModifiedOn = DateTime.UtcNow;
            _context.Quizzes.Update(quiz);
            await _context.SaveChangesAsync();
            return quiz;
        }

        public async Task DeleteQuizAsync(Guid id)
        {
            var quiz = await _context.Quizzes.FindAsync(id);
            if (quiz != null)
            {
                quiz.IsDeleted = true;
                quiz.ModifiedOn = DateTime.UtcNow;
                await _context.SaveChangesAsync();
            }
        }

        public async Task<List<QuizQuestion>> GetQuizQuestionsAsync(Guid quizId)
        {
            return await _context.QuizQuestions
                .Where(q => q.QuizId == quizId)
                .OrderBy(q => q.QuestionOrder)
                .ToListAsync();
        }

        public async Task<QuizQuestion> CreateQuizQuestionAsync(QuizQuestion question)
        {
            question.Id = Guid.NewGuid();
            question.CreatedOn = DateTime.UtcNow;
            _context.QuizQuestions.Add(question);
            await _context.SaveChangesAsync();
            return question;
        }

        public async Task<QuizQuestion> UpdateQuizQuestionAsync(QuizQuestion question)
        {
            question.ModifiedOn = DateTime.UtcNow;
            _context.QuizQuestions.Update(question);
            await _context.SaveChangesAsync();
            return question;
        }

        public async Task DeleteQuizQuestionAsync(Guid id)
        {
            var question = await _context.QuizQuestions.FindAsync(id);
            if (question != null)
            {
                question.IsDeleted = true;
                question.ModifiedOn = DateTime.UtcNow;
                await _context.SaveChangesAsync();
            }
        }

        public async Task<List<QuizOption>> GetQuizOptionsAsync(Guid questionId)
        {
            return await _context.QuizOptions
                .Where(o => o.QuestionId == questionId)
                .OrderBy(o => o.OptionOrder)
                .ToListAsync();
        }

        public async Task<QuizOption> CreateQuizOptionAsync(QuizOption option)
        {
            option.Id = Guid.NewGuid();
            option.CreatedOn = DateTime.UtcNow;
            _context.QuizOptions.Add(option);
            await _context.SaveChangesAsync();
            return option;
        }

        public async Task<QuizOption> UpdateQuizOptionAsync(QuizOption option)
        {
            option.ModifiedOn = DateTime.UtcNow;
            _context.QuizOptions.Update(option);
            await _context.SaveChangesAsync();
            return option;
        }

        public async Task DeleteQuizOptionAsync(Guid id)
        {
            var option = await _context.QuizOptions.FindAsync(id);
            if (option != null)
            {
                option.IsDeleted = true;
                option.ModifiedOn = DateTime.UtcNow;
                await _context.SaveChangesAsync();
            }
        }

        public async Task<QuizAttempt> StartQuizAttemptAsync(Guid quizId, Guid userId)
        {
            var attempt = new QuizAttempt
            {
                Id = Guid.NewGuid(),
                QuizId = quizId,
                UserId = userId,
                CreatedOn = DateTime.UtcNow
            };
            _context.QuizAttempts.Add(attempt);
            await _context.SaveChangesAsync();
            return attempt;
        }

        public async Task<QuizAttempt?> SubmitQuizAttemptAsync(Guid attemptId, List<QuizAnswer> answers)
        {
            var attempt = await _context.QuizAttempts.FindAsync(attemptId);
            if (attempt == null) return null;

            // Save answers
            foreach (var answer in answers)
            {
                answer.Id = Guid.NewGuid();
                answer.CreatedOn = DateTime.UtcNow;
                _context.QuizAnswers.Add(answer);
            }

            // Calculate score
            var correctAnswers = answers.Count(a => a.IsCorrect);
            var totalQuestions = answers.Count;
            attempt.Score = (decimal)correctAnswers / totalQuestions * 100;
            attempt.IsPassed = attempt.Score >= 60; // Default passing score
            attempt.CompletedAt = DateTime.UtcNow;
            attempt.ModifiedOn = DateTime.UtcNow;

            await _context.SaveChangesAsync();
            return attempt;
        }

        public async Task<List<QuizAttempt>> GetUserQuizAttemptsAsync(Guid userId, Guid quizId)
        {
            return await _context.QuizAttempts
                .Where(a => a.UserId == userId && a.QuizId == quizId)
                .OrderByDescending(a => a.CompletedAt)
                .ToListAsync();
        }
    }
}
