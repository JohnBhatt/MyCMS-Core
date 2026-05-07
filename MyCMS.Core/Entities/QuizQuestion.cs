using System.ComponentModel.DataAnnotations;

namespace MyCMS.Core.Entities
{
    public class QuizQuestion : BaseEntity
    {
        [Required]
        public Guid QuizId { get; set; }

        [Required]
        [MaxLength(2000)]
        public string QuestionText { get; set; }

        [Required]
        [MaxLength(50)]
        public string QuestionType { get; set; } // SingleChoice, MultipleChoice, TrueFalse
        public int QuestionOrder { get; set; }
    }
}
