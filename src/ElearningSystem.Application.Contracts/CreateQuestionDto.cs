using ElearningSystem.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElearningSystem
{
    public class CreateQuestionDto
    {
        public Guid CourseId { get; set; }
        public string QuestionText { get; set; }
        public QuestionType QuestionType { get; set; }
        public int Score { get; set; }
        public string? ImagePath { get; set; }

    }
}
