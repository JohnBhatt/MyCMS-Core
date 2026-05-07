using System.ComponentModel.DataAnnotations;

namespace MyCMS.Core.Entities
{
    public class QuizAnswer : BaseEntity
    {
        [Required]
        public Guid AttemptId { get; set; }

        [Required]
        public Guid QuestionId { get; set; }

        public Guid? SelectedOptionId { get; set; }
        public bool IsCorrect { get; set; }
    }
}
