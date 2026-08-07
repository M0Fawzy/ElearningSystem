using System;
using Volo.Abp.Domain.Entities.Auditing;

public class Answer : AuditedEntity<Guid>
{
    public Guid QuestionId { get; set; }

    public string AnswerText { get; set; }

    public bool IsCorrect { get; set; }

    public Question Question { get; set; }
}
