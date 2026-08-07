using ElearningSystem;
using ElearningSystem.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ElearningSystem.Web.Pages.Exams
{
    [Authorize(Roles = "admin,Teacher")]
    public class CreateExamModel : PageModel
    {
        private readonly IExamService _examService;
        private readonly IQuestionService _questionService;
        private readonly IAnswerService _answerService;
        private readonly ICourseService _courseService;
        private readonly IExamQuestionService _examQuestionService;
        private readonly ITeacherService _teacherService;
        private readonly ITeacherCourseService _teacherCourseService;

        public CreateExamModel(
            IExamService examService,
            IQuestionService questionService,
            IAnswerService answerService,
            ICourseService courseService,
            IExamQuestionService examQuestionService,
            ITeacherService teacherService,
            ITeacherCourseService teacherCourseService)
        {
            _examService = examService;
            _questionService = questionService;
            _answerService = answerService;
            _courseService = courseService;
            _examQuestionService = examQuestionService;
            _teacherService = teacherService;
            _teacherCourseService = teacherCourseService;
        }

        [BindProperty]
        public CreateExamDto ExamInput { get; set; }

        [BindProperty]
        public CreateQuestionDto QuestionInput { get; set; }

        [BindProperty]
        public List<AnswerInput> AnswerInputs { get; set; } = new();

        [BindProperty]
        public string EssayCorrectAnswer { get; set; }

        public ExamDto CurrentExam { get; set; }
        public List<QuestionDto> Questions { get; set; } = new();
        public List<QuestionDto> CourseQuestions { get; set; } = new();
        public List<CourseDto> Courses { get; set; } = new();
        [BindProperty]
        public CreateExamQuestionDto ExamQuestionInput { get; set; }





        public class AnswerInput
        {
            public string Text { get; set; }
            public bool IsCorrect { get; set; }
        }

        public async Task OnGetAsync(Guid? examId)
        {
            try
            {
                ExamInput = new CreateExamDto();
                QuestionInput = new CreateQuestionDto();
                AnswerInputs = new List<AnswerInput> { new AnswerInput() };
                ExamQuestionInput= new CreateExamQuestionDto();

                // Load courses for dropdown
                if (User.IsInRole("Teacher"))
                {
                    var teacher = await _teacherService.GetCurrentTeacherAsync();
                    Courses = await _teacherCourseService.GetTeacherCoursesAsync(teacher.Id);
                }
                else
                {
                    Courses = await _courseService.GetListAsync();
                }

                // If exam ID is provided, load the exam and its questions
                if (examId.HasValue && examId != Guid.Empty)
                {
                    CurrentExam = await _examService.GetAsync(examId.Value);
                     CourseQuestions = await _questionService.GetListByCourseIdAsync(CurrentExam.CourseId);
                    Questions = await _questionService.GetListByExamIdAsync(examId.Value);
                    // Load answers for each question
                    foreach (var question in Questions)
                    {
                        question.Answers = await _answerService.GetByQuestionIdAsync(question.Id);
                    }
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, $"Error: {ex.Message}");
            }
        }

        public async Task<IActionResult> OnPostCreateAsync()
        {
            Courses = await _courseService.GetListAsync();

            // Validate manually instead of ModelState
            if (string.IsNullOrWhiteSpace(ExamInput.Title))
            {
                ModelState.AddModelError(string.Empty, "Exam title is required");
                return Page();
            }

            if (ExamInput.TotalScore <= 0)
            {
                ModelState.AddModelError(string.Empty, "Total score must be greater than 0");
                return Page();
            }

            if (ExamInput.CourseId == Guid.Empty)
            {
                ModelState.AddModelError(string.Empty, "Course is required");
                return Page();
            }

            if (ExamInput.DurationMinutes <= 0 || ExamInput.DurationMinutes > 480)
            {
                ModelState.AddModelError(string.Empty, "Duration must be between 1 and 480 minutes");
                return Page();
            }

            try
            {
                CurrentExam = await _examService.CreateAsync(ExamInput);
                Questions = new List<QuestionDto>();
               // Questions = await _questionService.GetListByCourseIdAsync(ExamInput.CourseId);
                // Redirect to the same page with examId to show questions form
                return RedirectToPage(new { examId = CurrentExam.Id });
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, $"Error: {ex.InnerException?.Message ?? ex.Message}");
                return Page();
            }
        }

        public async Task<IActionResult> OnPostAddQuestionAsync(Guid examId)
        {
            
            ExamQuestionInput.ExamId=examId;

            await _examQuestionService.CreateAsync(ExamQuestionInput);
            
            CurrentExam = await _examService.GetAsync(examId);
                
                Courses = await _courseService.GetListAsync();
            Questions=await _questionService.GetListByExamIdAsync(examId);
                return RedirectToPage(new { examId = examId });
          
        }

        public async Task<IActionResult> OnPostRemoveQuestionAsync(Guid examId, Guid questionId)
        {
            try
            {
                await _examQuestionService.DeleteAsync(examId,questionId);
                return RedirectToPage(new { examId = examId });
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, $"Error: {ex.Message}");
                await OnGetAsync(examId);
                return Page();
            }
        }
    }
}