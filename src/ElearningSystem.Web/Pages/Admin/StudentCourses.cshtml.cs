using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ElearningSystem.Web.Pages.Admin
{
    [Authorize(Roles = "admin")]
    public class StudentCoursesModel : PageModel
    {
        private readonly IStudentCourseService _studentCourseService;
        private readonly ICourseService _courseService;
        private readonly IStudentService _studentService;

        public StudentCoursesModel(
            IStudentCourseService studentCourseService,
            ICourseService courseService,
            IStudentService studentService)
        {
            _studentCourseService = studentCourseService;
            _courseService = courseService;
            _studentService = studentService;
        }

        [BindProperty]
        public EnrollStudentDto EnrollInput { get; set; }

        public StudentDto CurrentStudent { get; set; }
        public List<CourseDto> AllCourses { get; set; } = new();
        public List<CourseDto> EnrolledCourses { get; set; } = new();

        public async Task<IActionResult> OnGetAsync(Guid id)
        {
            try
            {
                CurrentStudent = await _studentService.GetAsync(id);
                AllCourses = await _courseService.GetListAsync();
                EnrolledCourses = await _studentCourseService.GetStudentCoursesAsync(id);

                EnrollInput = new EnrollStudentDto { StudentId = id };

                return Page();
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, $"Error: {ex.Message}");
                return Page();
            }
        }

        public async Task<IActionResult> OnPostEnrollAsync()
        {
            try
            {
                if (EnrollInput.CourseId == Guid.Empty)
                {
                    ModelState.AddModelError(string.Empty, "Please select a course");
                    return await OnGetAsync(EnrollInput.StudentId);
                }

                await _studentCourseService.EnrollStudentAsync(EnrollInput);
                return RedirectToPage(new { id = EnrollInput.StudentId });
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, $"Error: {ex.Message}");
                return await OnGetAsync(EnrollInput.StudentId);
            }
        }

        public async Task<IActionResult> OnPostUnenrollAsync(Guid studentId, Guid courseId)
        {
            try
            {
                await _studentCourseService.UnenrollStudentAsync(studentId, courseId);
                return RedirectToPage(new { id = studentId });
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, $"Error: {ex.Message}");
                return await OnGetAsync(studentId);
            }
        }
    }
}