using MyCMS.Core.Entities;

namespace MyCMS.Core.Interfaces
{
    public interface IQuizService
    {
        Task<List<Quiz>> GetAllQuizzesAsync();
        Task<Quiz?> GetQuizByIdAsync(Guid id);
        Task<Quiz> CreateQuizAsync(Quiz quiz);
        Task<Quiz> UpdateQuizAsync(Quiz quiz);
        Task DeleteQuizAsync(Guid id);
        Task<List<QuizQuestion>> GetQuizQuestionsAsync(Guid quizId);
        Task<QuizQuestion> CreateQuizQuestionAsync(QuizQuestion question);
        Task<QuizQuestion> UpdateQuizQuestionAsync(QuizQuestion question);
        Task DeleteQuizQuestionAsync(Guid id);
        Task<List<QuizOption>> GetQuizOptionsAsync(Guid questionId);
        Task<QuizOption> CreateQuizOptionAsync(QuizOption option);
        Task<QuizOption> UpdateQuizOptionAsync(QuizOption option);
        Task DeleteQuizOptionAsync(Guid id);
        Task<QuizAttempt> StartQuizAttemptAsync(Guid quizId, Guid userId);
        Task<QuizAttempt?> SubmitQuizAttemptAsync(Guid attemptId, List<QuizAnswer> answers);
        Task<List<QuizAttempt>> GetUserQuizAttemptsAsync(Guid userId, Guid quizId);
    }
}
