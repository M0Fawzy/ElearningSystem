using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace ElearningSystem.Web.Pages.Student
{
    [Authorize(Roles = "student")]
    public class LecturesModel : PageModel
    {
        private readonly IStudentService _studentService;
        private readonly IStudentCourseService _studentCourseService;
        private readonly ILectureService _lectureService;

        public LecturesModel(
            IStudentService studentService,
            IStudentCourseService studentCourseService,
            ILectureService lectureService)
        {
            _studentService = studentService;
            _studentCourseService = studentCourseService;
            _lectureService = lectureService;
        }

        public StudentDto CurrentStudent { get; set; }
        public List<CourseDto> StudentCourses { get; set; } = new();
        public Dictionary<Guid, List<LectureDto>> CourseLectures { get; set; } = new();

        public async Task<IActionResult> OnGetAsync()
        {
            CurrentStudent = await _studentService.GetCurrentStudentAsync();
            StudentCourses = await _studentCourseService.GetStudentCoursesAsync(CurrentStudent.Id);

            foreach (var course in StudentCourses)
            {
                var lectures = await _lectureService.GetByCourseIdAsync(course.Id);
                CourseLectures[course.Id] = lectures;
            }

            return Page();
        }

        public async Task<IActionResult> OnGetDownloadAsync(Guid id)
        {
            var lecture = await _lectureService.GetAsync(id);

            var filePath = System.IO.Path.Combine(
                Directory.GetCurrentDirectory(),
                "wwwroot", "uploads", "lectures",
                lecture.FilePath);

            if (!System.IO.File.Exists(filePath))
            {
                ModelState.AddModelError(string.Empty, "File not found.");
                return Page();
            }

            var fileBytes = await System.IO.File.ReadAllBytesAsync(filePath);
            var contentType = lecture.FileType == "pdf" ? "application/pdf" : "video/mp4";

            return File(fileBytes, contentType, lecture.FileName);
        }
    }
}