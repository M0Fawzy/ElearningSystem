using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Domain.Entities;

namespace ElearningSystem.Entities
{
    public class StudentExam:Entity<Guid>
    {
        public Guid ExamId { get; set; }
        public Guid StudentId { get; set; }
        public int Score { get; set; }
        public int MaxScore { get; set; }
        public double Percentage { get; set; }
        public DateTime DateTaken { get; set; }
        public int TimeTaken { get; set; }

        public Exam Exam {  get; set; }
        public Student Student { get; set; }
    }
}
