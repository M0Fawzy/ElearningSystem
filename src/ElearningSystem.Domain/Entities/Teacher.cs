using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Domain.Entities.Auditing;

namespace ElearningSystem.Entities
{
    public class Teacher: AuditedAggregateRoot<Guid>
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string UserId { get; set; } // Links to ABP Identity User
        public DateTime EnrollmentDate { get; set; }

        public ICollection<TeacherCourse> TeacherCourses { get; set; } = new List<TeacherCourse>();
    }
}
