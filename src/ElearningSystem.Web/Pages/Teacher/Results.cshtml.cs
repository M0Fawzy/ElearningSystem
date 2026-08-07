using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ElearningSystem.Web.Pages.Teacher
{
    [Authorize(Roles = "Teacher")]
    public class ResultsModel : PageModel
    {
        private readonly ITeacherService _teacherService;
        private readonly ITeacherCourseService _teacherCourseService;
        private readonly IStudentService _studentService;
        private readonly IExamService _examService;
        private readonly IStudentExamService _studentExamService;
        public ResultsModel(ITeacherService teacherService, ITeacherCourseService teacherCourseService, IStudentService studentService, IStudentExamService studentExamService,IExamService examService)
        {
            _teacherService = teacherService;
            _teacherCourseService = teacherCourseService;
            _studentService = studentService;
            _studentExamService = studentExamService;
            _examService = examService;
        }

        public TeacherDto CurrentTeacher { get; set; }
        public List<CourseDto> TeacherCourses { get; set; } = new();
        public List<ExamDto> AllExams { get; set; } = new();
        public List<ExamDto> TeacherExams { get; set; } = new();
        public List<StudentDto> Students { get; set; } = new();
        public List<StudentExamDto> StudentScores { get; set; } = new();
        public async Task<IActionResult> OnGetAsync()
        {
            CurrentTeacher=await _teacherService.GetCurrentTeacherAsync();
            TeacherCourses = await _teacherCourseService.GetTeacherCoursesAsync(CurrentTeacher.Id);
            Students = await _studentService.GetListAsync();
            AllExams =await _examService.GetListAsync();
            var TeacherCourseIds=TeacherCourses.Select(c=>c.Id).ToList();
            TeacherExams=AllExams.Where(e=>TeacherCourseIds.Contains(e.CourseId)).ToList();
            var teacherExamIds = TeacherExams.Select(e => e.Id).ToList();
            foreach (var examId in teacherExamIds)
            {
                var results = await _studentExamService.GetByExamIdAsync(examId);
                StudentScores.AddRange(results);
            }
            return Page();
        }
    }
}
