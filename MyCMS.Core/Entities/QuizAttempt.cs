using System;
using System.ComponentModel.DataAnnotations;

namespace MyCMS.Core.Entities
{
    public class QuizAttempt : BaseEntity
    {
        [Required]
        public Guid QuizId { get; set; }

        [Required]
        public Guid UserId { get; set; }

        public decimal Score { get; set; } // percentage
        public DateTime? CompletedAt { get; set; }
        public bool IsPassed { get; set; }
    }
}
