using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Volo.Abp.Users;

namespace ElearningSystem.Web.Pages.Teacher
{
    [Authorize(Roles = "Teacher")]
    public class LecturesModel : PageModel
    {
        private readonly ILectureService _lectureService;
        private readonly ITeacherService _teacherService;
        private readonly ITeacherCourseService _teacherCourseService;
        private readonly ICurrentUser _currentUser;

        public LecturesModel(
            ILectureService lectureService,
            ITeacherService teacherService,
            ITeacherCourseService teacherCourseService,
            ICurrentUser currentUser)
        {
            _lectureService = lectureService;
            _teacherService = teacherService;
            _teacherCourseService = teacherCourseService;
            _currentUser = currentUser;
        }

        public TeacherDto CurrentTeacher { get; set; }
        public List<LectureDto> Lectures { get; set; } = new();
        public List<CourseDto> TeacherCourses { get; set; } = new();

        [BindProperty]
        public CreateLectureDto LectureInput { get; set; }

        [BindProperty]
        public IFormFile UploadedFile { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            CurrentTeacher = await _teacherService.GetCurrentTeacherAsync();
            TeacherCourses = await _teacherCourseService.GetTeacherCoursesAsync(CurrentTeacher.Id);
            Lectures = await _lectureService.GetListAsync();
            return Page();
        }

        public async Task<IActionResult> OnPostUploadAsync()
        {
            CurrentTeacher = await _teacherService.GetCurrentTeacherAsync();
            TeacherCourses = await _teacherCourseService.GetTeacherCoursesAsync(CurrentTeacher.Id);

            if (UploadedFile == null || UploadedFile.Length == 0)
            {
                ModelState.AddModelError(string.Empty, "Please select a file to upload.");
                Lectures = await _lectureService.GetListAsync();
                return Page();
            }

            // Validate file type
            var extension = Path.GetExtension(UploadedFile.FileName).ToLower();
            var allowedExtensions = new[] { ".pdf", ".mp4", ".avi", ".mov", ".mkv" };
            if (!Array.Exists(allowedExtensions, e => e == extension))
            {
                ModelState.AddModelError(string.Empty, "Only PDF and video files are allowed.");
                Lectures = await _lectureService.GetListAsync();
                return Page();
            }

            // Save file to wwwroot/uploads/lectures
            var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads", "lectures");
            if (!Directory.Exists(uploadsFolder))
                Directory.CreateDirectory(uploadsFolder);

            var uniqueFileName = Guid.NewGuid().ToString() + extension;
            var filePath = Path.Combine(uploadsFolder, uniqueFileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await UploadedFile.CopyToAsync(stream);
            }

            LectureInput.TeacherId = CurrentTeacher.Id;

            await _lectureService.CreateAsync(
                LectureInput,
                UploadedFile.FileName,
                uniqueFileName,
                extension.Replace(".", ""),
                UploadedFile.Length
            );

            return RedirectToPage();
        }

        public async Task<IActionResult> OnPostDeleteAsync(Guid id)
        {
            var lecture = await _lectureService.GetAsync(id);

            // Delete physical file
            var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads", "lectures", lecture.FilePath);
            if (System.IO.File.Exists(filePath))
                System.IO.File.Delete(filePath);

            await _lectureService.DeleteAsync(id);
            return RedirectToPage();
        }
    }
}