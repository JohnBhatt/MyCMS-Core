using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MyCMS.Core.Entities
{
    public class Quiz : BaseEntity
    {
        [Required]
        [MaxLength(1000)]
        public string Title { get; set; }

        public string Description { get; set; }
        public DateTime? StartTime { get; set; }
        public DateTime? EndTime { get; set; }
        public int? TimeLimitMinutes { get; set; }
        public decimal PassingScore { get; set; } = 60; // percentage

        public List<QuizQuestion> Questions { get; set; } = new();
    }
}
