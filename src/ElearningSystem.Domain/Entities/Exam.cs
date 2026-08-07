using System;
using System.Collections.Generic;
using Volo.Abp.Domain.Entities.Auditing;

namespace ElearningSystem.Entities
{
    public class Exam : AuditedAggregateRoot<Guid>
    {
        public string Title { get; set; }
        public int TotalScore { get; set; }
        public DateTime CreatedDate { get; set; }
        public Guid CourseId { get; set; }
        public int DurationMinutes { get; set; } = 60;
        // Navigation
        public Course Course { get; set; }

        public ICollection<ExamQuestion> ExamQuestions { get; set; }
        public ICollection<StudentExam> StudentExams{ get; set; }

    }
}