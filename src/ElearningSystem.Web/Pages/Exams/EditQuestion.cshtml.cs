using ElearningSystem;
using ElearningSystem.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.TagHelpers.Card;

namespace ElearningSystem.Web.Pages.Exams
{
    public class EditQuestionModel : PageModel
    {
        private readonly IExamService _examService;
        private readonly ICourseService _courseService;

        public EditQuestionModel(
            IExamService examService,
            ICourseService courseService)
        {
            _examService = examService;
            _courseService = courseService;
        }

        [BindProperty]
        public CreateExamDto ExamInput { get; set; }
        public ExamDto Exams { get; set; }
        public List<CourseDto> Courses { get; set; } = new();
        public async Task OnGetAsync(Guid? id)
        {
            if (id.HasValue && id != Guid.Empty)
            {
                Exams = await _examService.GetAsync(id.Value);
                ExamInput = new CreateExamDto { Id= id.Value, Title = Exams.Title, CourseId = Exams.CourseId, TotalScore = Exams.TotalScore, DurationMinutes=Exams.DurationMinutes };
            }
            // Load courses for dropdown
            Courses = await _courseService.GetListAsync();
            
        }
        public async Task<IActionResult> OnPostUpdateExamAsync()
        {
            await _examService.UpdateExamAsync(ExamInput);
            //CurrentExam = await _examService.GetAsync(ExamInput.Id);
            return RedirectToPage(new { id = ExamInput.Id });
        }
    }
}