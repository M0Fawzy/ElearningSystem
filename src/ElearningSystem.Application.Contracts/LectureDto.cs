using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElearningSystem
{
    public class LectureDto
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string FileName { get; set; }
        public string FilePath { get; set; }
        public string FileType { get; set; }
        public Guid CourseId { get; set; }
        public Guid TeacherId { get; set; }
        public long FileSize { get; set; }
    }

    public class CreateLectureDto
    {
        public string Title { get; set; }
        public Guid CourseId { get; set; }
        public Guid TeacherId { get; set; }
    }
}
