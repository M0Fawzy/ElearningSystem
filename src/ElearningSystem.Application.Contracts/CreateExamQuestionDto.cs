using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElearningSystem
{
    public class CreateExamQuestionDto
    {
        public Guid ExamId { get; set; }
        public Guid QuestionId { get; set; }
        public int OrderIndex { get; set; }
    }
}
