using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Domain.Entities;

namespace ElearningSystem.Entities
{
    public class ExamQuestion : Entity<Guid>
    {
        public Guid ExamId { get; set; }
        public Guid QuestionId { get; set; }
        public int OrderIndex { get; set; }  // Question order in exam

        // Navigation
        public Exam Exam { get; set; }
        public Question Question { get; set; }
    }
}
