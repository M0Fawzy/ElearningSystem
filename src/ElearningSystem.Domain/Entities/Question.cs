using ElearningSystem.Entities;
using System;
using System.Collections.Generic;
using Volo.Abp.Domain.Entities.Auditing;

public class Question : AuditedEntity<Guid>
{
    public Guid CourseId { get; set; }

    public string QuestionText { get; set; }

    public QuestionType QuestionType { get; set; }

    public int Score { get; set; }

    public string? ImagePath { get; set; }

    public Course Course { get; set; }
    public ICollection<ExamQuestion>ExamQuestions { get; set; }
    public ICollection<Answer> Answers { get; set; }
}
