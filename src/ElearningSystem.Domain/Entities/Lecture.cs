using System;
using Volo.Abp.Domain.Entities.Auditing;

namespace ElearningSystem.Entities
{
    public class Lecture : AuditedAggregateRoot<Guid>
    {
        public string Title { get; set; }
        public string FileName { get; set; }
        public string FilePath { get; set; }
        public string FileType { get; set; }
        public Guid CourseId { get; set; }
        public Guid TeacherId { get; set; }
        public long FileSize { get; set; }
    }
}