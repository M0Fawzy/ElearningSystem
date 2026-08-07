using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ElearningSystem.Web.Pages.Teacher
{
    [Authorize(Roles = "Teacher")]
    public class DashboardModel : PageModel
    {
        private readonly ITeacherService _teacherService;
        private readonly ITeacherCourseService _teacherCourseService;
        private readonly IExamService _examService;
        private readonly IStudentCourseService _studentCourseService;
        private readonly IStudentExamService _studentExamService;

        public DashboardModel(
            ITeacherService teacherService,
            ITeacherCourseService teacherCourseService,
            IExamService examService,
            IStudentCourseService studentCourseService,
            IStudentExamService studentExamService)
        {
            _teacherService = teacherService;
            _teacherCourseService = teacherCourseService;
            _examService = examService;
            _studentCourseService = studentCourseService;
            _studentExamService = studentExamService;
        }

        public TeacherDto CurrentTeacher { get; set; }
        public List<CourseDto> TeacherCourses { get; set; } = new();
        public Dictionary<Guid, int> CourseStudentCount { get; set; } = new();
        public Dictionary<Guid, List<ExamDto>> CourseExams { get; set; } = new();
        public Dictionary<Guid, List<StudentExamDto>> ExamResults { get; set; } = new();

        public async Task<IActionResult> OnGetAsync()
        {
            CurrentTeacher = await _teacherService.GetCurrentTeacherAsync();
            TeacherCourses = await _teacherCourseService.GetTeacherCoursesAsync(CurrentTeacher.Id);

            foreach (var course in TeacherCourses)
            {
                // student count per course
                var students = await _studentCourseService.GetCourseStudentsAsync(course.Id);
                CourseStudentCount[course.Id] = students.Count;

                // exams per course
                var allExams = await _examService.GetListAsync();
                CourseExams[course.Id] = allExams.Where(e => e.CourseId == course.Id).ToList();

                // results per exam
                foreach (var exam in CourseExams[course.Id])
                {
                    var results = await _studentExamService.GetByExamIdAsync(exam.Id);
                    ExamResults[exam.Id] = results;
                }
            }

            return Page();
        }
    }
}