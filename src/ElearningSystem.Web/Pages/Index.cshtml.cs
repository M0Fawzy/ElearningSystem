using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.Users;

namespace ElearningSystem.Web.Pages
{
    [Authorize]  
    public class IndexModel : PageModel
    {
        private readonly IStudentCourseService _studentCourseService;
        private readonly IExamService _examService;
        private readonly IStudentService _studentService;
        private readonly IStudentExamService _studentExamService;
        private readonly ICurrentUser _currentUser;

        public IndexModel(
            IStudentCourseService studentCourseService,
            IExamService examService,
            IStudentService studentService,
            IStudentExamService studentExamService,
            ICurrentUser currentUser
            )
        {
            _studentCourseService = studentCourseService;
            _examService = examService;
            _studentService = studentService;
            _studentExamService= studentExamService;
            _currentUser = currentUser;
        }

        public StudentDto CurrentStudent { get; set; }
        public List<CourseDto> StudentCourses { get; set; } = new();
        public Dictionary<Guid,List<StudentExamDto>> StudentExamResults { get; set; }= new();
        public Dictionary<Guid, List<ExamDto>> CourseExams { get; set; } = new();
        public Dictionary<Guid,ExamDto> AllExams { get; set; } = new();

        public async Task<IActionResult> OnGetAsync()
        {
            if (_currentUser.IsInRole("admin"))
                return RedirectToPage("/Admin/Students");

            if (_currentUser.IsInRole("Teacher"))
                return RedirectToPage("/Teacher/Dashboard");
            try
            {
                // Get current logged-in student
                CurrentStudent = await _studentService.GetCurrentStudentAsync();

                // Get courses enrolled in
                StudentCourses = await _studentCourseService.GetStudentCoursesAsync(CurrentStudent.Id);

                var allexams=await _examService.GetListAsync();
                foreach (var exam in allexams) {
                    AllExams[exam.Id] = exam;
                }

                // Get exams for each course
                foreach (var course in StudentCourses)
                {
                    var exams = await _examService.GetListAsync();
                    var courseExams = exams.FindAll(e => e.CourseId == course.Id);
                    CourseExams[course.Id] = courseExams;
                }

                
                    var results = await _studentExamService.GetByStudentIdAsync(CurrentStudent.Id);
                    StudentExamResults[CurrentStudent.Id] = results;
                

                return Page();
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, $"Error: {ex.Message}");
                return Page();
            }
        }
    }
}