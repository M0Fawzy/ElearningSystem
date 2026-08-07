using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ElearningSystem.Web.Pages.Exams
{
    [Authorize(Roles = "Teacher")]
    public class TeacherExamsModel : PageModel
    {
        
        private readonly ITeacherService _teacherService;
        private readonly ITeacherCourseService _teacherCourseService;
        private readonly IExamService _examService;
        private readonly ICourseService _courseService;
        public TeacherExamsModel(ITeacherService teacherService, ITeacherCourseService teacherCourseService, IExamService examService, ICourseService courseService)
        {
            _teacherService = teacherService;
            _teacherCourseService = teacherCourseService;
            _examService = examService;
            _courseService = courseService;
        }

        public TeacherDto CurrentTeacher { get; set; }
        public  List<ExamDto> AllExams { get; set; } = new();
        public List<CourseDto> TeacherCourses { get; set; } = new();
        public List<ExamDto> TeacherExams { get; set; } = new();
        public async Task <IActionResult> OnGetAsync()
        {
            CurrentTeacher=await _teacherService.GetCurrentTeacherAsync();
            AllExams = await _examService.GetListAsync();
            TeacherCourses = await _teacherCourseService.GetTeacherCoursesAsync(CurrentTeacher.Id);
            var TeacherCourseIds=TeacherCourses.Select(c=>c.Id).ToList();
            TeacherExams = AllExams.Where(e=>TeacherCourseIds.Contains(e.CourseId)).ToList();
            return Page();
        }
        public async Task<IActionResult> OnPostDeleteAsync(Guid id)
        {
            await _examService.DeleteAsync(id);
            return RedirectToPage();
        }
    }
}
