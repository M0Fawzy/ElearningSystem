using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ElearningSystem.Web.Pages.Admin
{
    [Authorize (Roles = "admin")]
    public class TeacherCoursesModel : PageModel
    {
        private readonly ITeacherCourseService _teacherCourseService;
        private readonly ICourseService _courseService;
        private readonly ITeacherService _teacherService;

        public TeacherCoursesModel(
            ITeacherCourseService teacherCourseService,
            ICourseService courseService,
            ITeacherService teacherService)
        {
            _teacherCourseService = teacherCourseService;
            _courseService = courseService;
            _teacherService = teacherService;
        }

        [BindProperty]
        public EnrollTeacherDto EnrollInput { get; set; }

        public TeacherDto CurrentTeacher { get; set; }
        public List<CourseDto> AllCourses { get; set; } = new();
        public List<CourseDto> EnrolledCourses { get; set; } = new();

        public async Task<IActionResult> OnGetAsync(Guid id)
        {
            try
            {
                CurrentTeacher = await _teacherService.GetAsync(id);
                AllCourses = await _courseService.GetListAsync();
                EnrolledCourses = await _teacherCourseService.GetTeacherCoursesAsync(id);

                EnrollInput = new EnrollTeacherDto { TeacherId = id };

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
                    return await OnGetAsync(EnrollInput.TeacherId);
                }

                await _teacherCourseService.EnrollTeacherAsync(EnrollInput);
                return RedirectToPage(new { id = EnrollInput.TeacherId });
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, $"Error: {ex.Message}");
                return await OnGetAsync(EnrollInput.TeacherId);
            }
        }

        public async Task<IActionResult> OnPostUnenrollAsync(Guid teacherId, Guid courseId)
        {
            try
            {
                await _teacherCourseService.UnenrollTeacherAsync(teacherId, courseId);
                return RedirectToPage(new { id = teacherId });
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, $"Error: {ex.Message}");
                return await OnGetAsync(teacherId);
            }
        }
    }
}
