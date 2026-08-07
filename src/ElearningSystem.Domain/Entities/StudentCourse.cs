using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Domain.Entities;

namespace ElearningSystem.Entities
{
    public class StudentCourse : Entity<Guid>
    {
        public Guid StudentId { get; set; }
        public Guid CourseId { get; set; }
        public DateTime EnrollmentDate { get; set; }

        // Navigation
        public Student Student { get; set; }
        public Course Course { get; set; }
    }
}
