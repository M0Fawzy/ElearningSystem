using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElearningSystem
{
    public class AnswerDto
    {
        public Guid Id { get; set; }

        public string AnswerText { get; set; }
        public bool IsCorrect { get; set; }

        
    }
}
