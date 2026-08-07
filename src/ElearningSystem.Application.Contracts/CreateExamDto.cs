using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElearningSystem
{
    public class CreateExamDto
    {
        public Guid Id { get; set; }  
        public string Title { get; set; }
        public int TotalScore { get; set; }
        public Guid CourseId { get; set; }
        [Required]
        [Range(1, 480, ErrorMessage = "Duration must be between 5 and 480 minutes")]
        public int DurationMinutes { get; set; } = 60;
    }

}
