using System.ComponentModel.DataAnnotations;

namespace MyCMS.Core.Entities
{
    public class QuizOption : BaseEntity
    {
        [Required]
        public Guid QuestionId { get; set; }

        [Required]
        [MaxLength(1000)]
        public string OptionText { get; set; }

        public bool IsCorrect { get; set; } = false;
        public int OptionOrder { get; set; }
    }
}
