using ElearningSystem;
using ElearningSystem.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore.Migrations.Operations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp.Application.Services;

namespace ElearningSystem.Web.Pages.Exams
{
    [Authorize(Roles = "admin,Teacher")]
    public class ExamModel : PageModel
    {
        private readonly IExamService _examService;
        private readonly IExamQuestionService _examQuestionService;
        private readonly IQuestionService _questionService;
        private readonly ICourseService _courseService;
        public ExamModel(IExamService examService, IExamQuestionService examQuestionService, IQuestionService questionService, ICourseService courseService)
        {
            _examService = examService;
            _examQuestionService = examQuestionService;
            _questionService = questionService;
            _courseService = courseService;
        }

        public List<ExamDto> Exams { get; set; } = new();
        public List<CourseDto> Courses { get; set; }= new();
        public int CurrentPage { get; set; } = 1;     // NEW
        public int PageSize { get; set; } = 10;       // NEW (10 exams per page)
        public int TotalCount { get; set; }

        public async Task OnGetAsync(int currentPage = 1)
        {
            CurrentPage = currentPage;

            // Get all exams
            var allExams = await _examService.GetListAsync();


            Courses=await _courseService.GetListAsync();
            // Total count
            TotalCount = allExams.Count;

            // Calculate skip
            var skipCount = (CurrentPage - 1) * PageSize;

            // Paginate in memory
            Exams = allExams
                .Skip(skipCount)
                .Take(PageSize)
                .ToList();
        }

        public async Task<IActionResult> OnGetLoadQuestionsAsync(Guid id)
        {
            // Get exam details
            var exam = await _examService.GetAsync(id);

            // Get questions IN this exam
            var examQuestions = await _examQuestionService.GetByExamIdAsync(id);

            // Get question IDs that are in the exam
            var questionIdsInExam = examQuestions.Select(eq => eq.QuestionId).ToList();

            // Load full question details for questions in exam
            var questionsInExam = new List<QuestionDto>();
            foreach (var eq in examQuestions.OrderBy(eq => eq.OrderIndex))
            {
                var question = await _questionService.GetAsync(eq.QuestionId);
                questionsInExam.Add(question);
            }

            // Get ALL questions for this exam's course
            var allCourseQuestions = await _questionService.GetListByCourseIdAsync(exam.CourseId);

            // Filter to get questions NOT in exam (available to add)
            var availableQuestions = allCourseQuestions
                .Where(q => !questionIdsInExam.Contains(q.Id))
                .ToList();

            // Return both lists as JSON
            return new JsonResult(new
            {
                questionsInExam = questionsInExam,
                availableQuestions = availableQuestions,
                examTitle = exam.Title
            });
        }

        public async Task<IActionResult> OnPostAddQuestionsAsync(Guid id,Guid QuestionId)
        {
            try
            {
                var exists = await _examQuestionService.ExistsAsync(id, QuestionId);

                if (exists)
                {
                    return new JsonResult(new
                    {
                        success = false,
                        message = "Question Already Exists"
                    });
                }
                var maxOrder = await _examQuestionService.GetMaxOrderIndexAsync(id);
                await _examQuestionService.CreateAsync(new CreateExamQuestionDto
                {
                    ExamId = id,
                    QuestionId = QuestionId,
                    OrderIndex = maxOrder + 1
                });

                return new JsonResult(new
                {
                    success = true,
                    message = "Question Added Successfully"
                });
            }
            catch (Exception ex) {

                return new JsonResult(new
                {
                    success = false,
                    message = "error" + ex.Message
                });
            }

        }

        public async Task<IActionResult> OnPostRemoveQuestionsAsync(Guid id,Guid QuestionId)
        {
            try
            {
                await _examQuestionService.DeleteAsync(id, QuestionId);
                return new JsonResult(new
                {
                    success = true,
                    message = "Question Deleted Successfully"
                }
                    );

            }
            catch (Exception ex) {
                return new JsonResult(new
                {
                    success = false,
                    message = "error" + ex.Message
                });
            }
        }
        public async Task<IActionResult> OnPostDeleteAsync(Guid id)
        {
            await _examService.DeleteAsync(id);
            return RedirectToPage();
        }
    }
}