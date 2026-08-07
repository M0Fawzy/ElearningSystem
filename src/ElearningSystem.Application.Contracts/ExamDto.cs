using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElearningSystem
{
    public class ExamDto
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public int TotalScore { get; set; }
        public DateTime CreatedDate { get; set; }
        public Guid CourseId { get; set; }
        public int DurationMinutes { get; set; } = 60;
    }
}
