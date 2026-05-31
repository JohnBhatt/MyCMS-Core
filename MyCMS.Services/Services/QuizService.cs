using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using MyCMS.Core.Entities;
using MyCMS.Core.Interfaces;
using MyCMS.Data;

namespace MyCMS.Services
{
    public class QuizService : IQuizService
    {
        private readonly AppDbContext _context;
        private readonly IAuditService _auditService;

        public QuizService(AppDbContext context, IAuditService auditService)
        {
            _context = context;
            _auditService = auditService;
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
            
            await _auditService.LogAsync("Quizzes", quiz.Id.ToString(), "Created", null,
                JsonSerializer.Serialize(new { quiz.Title, quiz.Description, quiz.StartTime, quiz.EndTime }), "Quiz created");
            return quiz;
        }

        public async Task<Quiz> UpdateQuizAsync(Quiz quiz)
        {
            var existing = await _context.Quizzes.FindAsync(quiz.Id);
            if (existing == null) throw new InvalidOperationException("Quiz not found");
            
            var oldValues = JsonSerializer.Serialize(new { existing.Title, existing.Description, existing.StartTime, existing.EndTime, existing.PassingScore });
            
            quiz.ModifiedOn = DateTime.UtcNow;
            _context.Quizzes.Update(quiz);
            await _context.SaveChangesAsync();
            
            var newValues = JsonSerializer.Serialize(new { quiz.Title, quiz.Description, quiz.StartTime, quiz.EndTime, quiz.PassingScore });
            await _auditService.LogAsync("Quizzes", quiz.Id.ToString(), "Updated", oldValues, newValues, "Quiz updated");
            return quiz;
        }

        public async Task DeleteQuizAsync(Guid id)
        {
            var quiz = await _context.Quizzes.FindAsync(id);
            if (quiz != null)
            {
                var oldValues = JsonSerializer.Serialize(new { quiz.Title, quiz.Description });
                quiz.IsDeleted = true;
                quiz.ModifiedOn = DateTime.UtcNow;
                await _context.SaveChangesAsync();
                
                await _auditService.LogAsync("Quizzes", id.ToString(), "Deleted", oldValues, null, "Quiz deleted");
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
            
            await _auditService.LogAsync("QuizQuestions", question.Id.ToString(), "Created", null,
                JsonSerializer.Serialize(new { question.QuestionText, question.QuestionType, question.QuizId }), "Question added");
            return question;
        }

        public async Task<QuizQuestion> UpdateQuizQuestionAsync(QuizQuestion question)
        {
            var existing = await _context.QuizQuestions.FindAsync(question.Id);
            if (existing == null) throw new InvalidOperationException("Question not found");
            
            var oldValues = JsonSerializer.Serialize(new { existing.QuestionText, existing.QuestionType, existing.QuestionOrder });
            
            question.ModifiedOn = DateTime.UtcNow;
            _context.QuizQuestions.Update(question);
            await _context.SaveChangesAsync();
            
            var newValues = JsonSerializer.Serialize(new { question.QuestionText, question.QuestionType, question.QuestionOrder });
            await _auditService.LogAsync("QuizQuestions", question.Id.ToString(), "Updated", oldValues, newValues, "Question updated");
            return question;
        }

        public async Task DeleteQuizQuestionAsync(Guid id)
        {
            var question = await _context.QuizQuestions.FindAsync(id);
            if (question != null)
            {
                var oldValues = JsonSerializer.Serialize(new { question.QuestionText, question.QuizId });
                question.IsDeleted = true;
                question.ModifiedOn = DateTime.UtcNow;
                await _context.SaveChangesAsync();
                
                await _auditService.LogAsync("QuizQuestions", id.ToString(), "Deleted", oldValues, null, "Question deleted");
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
            
            await _auditService.LogAsync("QuizOptions", option.Id.ToString(), "Created", null,
                JsonSerializer.Serialize(new { option.OptionText, option.IsCorrect, option.QuestionId }), "Option added");
            return option;
        }

        public async Task<QuizOption> UpdateQuizOptionAsync(QuizOption option)
        {
            var existing = await _context.QuizOptions.FindAsync(option.Id);
            if (existing == null) throw new InvalidOperationException("Option not found");
            
            var oldValues = JsonSerializer.Serialize(new { existing.OptionText, existing.IsCorrect, existing.OptionOrder });
            
            option.ModifiedOn = DateTime.UtcNow;
            _context.QuizOptions.Update(option);
            await _context.SaveChangesAsync();
            
            var newValues = JsonSerializer.Serialize(new { option.OptionText, option.IsCorrect, option.OptionOrder });
            await _auditService.LogAsync("QuizOptions", option.Id.ToString(), "Updated", oldValues, newValues, "Option updated");
            return option;
        }

        public async Task DeleteQuizOptionAsync(Guid id)
        {
            var option = await _context.QuizOptions.FindAsync(id);
            if (option != null)
            {
                var oldValues = JsonSerializer.Serialize(new { option.OptionText, option.QuestionId });
                option.IsDeleted = true;
                option.ModifiedOn = DateTime.UtcNow;
                await _context.SaveChangesAsync();
                
                await _auditService.LogAsync("QuizOptions", id.ToString(), "Deleted", oldValues, null, "Option deleted");
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
