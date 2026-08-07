using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ElearningSystem.Web.Pages.Admin
{
    [Authorize(Roles = "admin")]
    public class StudentsModel : PageModel
    {
        private readonly IStudentService _studentService;
        private readonly IStudentExamService _studentExamService;
        private readonly IExamService _examService;

        public StudentsModel(IStudentService studentService, IStudentExamService studentExamService, IExamService examService)
        {
            _studentService = studentService;
            _studentExamService = studentExamService;
            _examService = examService;
        }

        [BindProperty]
        public CreateStudentDto StudentInput { get; set; }

        public List<StudentDto> Students { get; set; } = new();

        public Dictionary<Guid, List<StudentExamDto>> StudentExamResults { get; set; } = new();
        public Dictionary<Guid, ExamDto> AllExams { get; set; } = new();
        public async Task OnGetAsync()
        {
            Students = await _studentService.GetListAsync();

            var allExams = await _examService.GetListAsync();
            foreach (var exam in allExams)
            {
                AllExams[exam.Id] = exam;
            }

            foreach (var student in Students)
            {
                var results = await _studentExamService.GetByStudentIdAsync(student.Id);
                StudentExamResults[student.Id] = results;
            }
            StudentInput = new CreateStudentDto();
        }

        public async Task<IActionResult> OnPostCreateAsync()
        {
            if (!ModelState.IsValid)
            {
                await OnGetAsync();
                System.Diagnostics.Debug.WriteLine($"Student name: {StudentInput.FirstName}");

                return Page();
            }

            try
            {
                await _studentService.CreateAsync(StudentInput);
                System.Diagnostics.Debug.WriteLine($"Student name: {StudentInput.FirstName}");

                return RedirectToPage();
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                await OnGetAsync();
                return Page();
            }
        }

        public async Task<IActionResult> OnPostDeleteAsync(Guid id)
        {
            try
            {
                await _studentService.DeleteAsync(id);
                return RedirectToPage();
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, $"Error: {ex.Message}");
                await OnGetAsync();
                return Page();
            }
        }
    }
}