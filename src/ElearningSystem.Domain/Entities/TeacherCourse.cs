using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Domain.Entities;
using Volo.Abp.Domain.Entities.Auditing;

namespace ElearningSystem.Entities
{
    public class TeacherCourse : AuditedAggregateRoot<Guid>
    {
        public Guid TeacherId { get; set; }
        public Guid CourseId { get; set; }
    }
}
