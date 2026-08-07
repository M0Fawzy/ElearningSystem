// Pages/Exams/AttemptExam.cshtml.cs
using ElearningSystem;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp.Users;

namespace ElearningSystem.Web.Pages.Exams
{
    public class AttemptExamModel : PageModel
    {
        private readonly IExamService _examService;
        private readonly IQuestionService _questionService;
        private readonly IAnswerService _answerService;
        private readonly IStudentService _studentService;
        private readonly IStudentExamService _studentExamService;

        public AttemptExamModel(
            IExamService examService,
            IQuestionService questionService,
            IAnswerService answerService,
            IStudentService studentService,
            IStudentExamService studentExamService
            )
        {
            _examService = examService;
            _questionService = questionService;
            _answerService = answerService;
            _studentService = studentService;
            _studentExamService= studentExamService;
        }

        public ExamDto Exam { get; set; }
        public List<QuestionDto> Questions { get; set; } = new();

        [BindProperty]
        public Dictionary<string, string> UserAnswers { get; set; } = new();

        public int Score { get; set; }
        public double Percentage { get; set; }
        public bool ExamSubmitted { get; set; }


        // Timer properties
        public DateTime ExamStartTime { get; set; }
        public int DurationSeconds { get; set; }

        public async Task OnGetAsync(Guid id)
        {
            Exam = await _examService.GetAsync(id);
            Questions = await _questionService.GetListByExamIdAsync(id);

            // Load answers for each question
            foreach (var question in Questions)
            {
                question.Answers = await _answerService.GetByQuestionIdAsync(question.Id);
            }

            // Set timer
            ExamStartTime = DateTime.Now;
            DurationSeconds = Exam.DurationMinutes * 60;

            ExamSubmitted = false;
        }

        public async Task<IActionResult> OnPostSubmitAsync(Guid examId)
        {

            Exam = await _examService.GetAsync(examId);
            Questions = await _questionService.GetListByExamIdAsync(examId);

          var currentuser=  await _studentService.GetCurrentStudentAsync();
            // Load answers for each question
            foreach (var question in Questions)
            {
                question.Answers = await _answerService.GetByQuestionIdAsync(question.Id);
            }

            Score = 0;

            // Calculate score
            foreach (var question in Questions)
            {
                var answerKey = $"answer_{question.Id}";

                if (!UserAnswers.ContainsKey(answerKey) || string.IsNullOrEmpty(UserAnswers[answerKey]))
                {
                    continue;
                }

                var selectedAnswerId = UserAnswers[answerKey];

                // For essays
                if (question.QuestionType.ToString() == "Essay")
                {
                    var correctAnswer = question.Answers.FirstOrDefault(a => a.IsCorrect);

                    if (correctAnswer != null && !string.IsNullOrWhiteSpace(correctAnswer.AnswerText))
                    {
                        string trimmedCorrect = correctAnswer.AnswerText.Trim();
                        string trimmedStudent = selectedAnswerId.Trim();

                        if (trimmedStudent.Equals(trimmedCorrect, StringComparison.OrdinalIgnoreCase))
                        {
                            Score += question.Score;
                        }
                    }
                }
                else
                {
                    // For multiple choice and true/false
                    if (Guid.TryParse(selectedAnswerId, out var answerGuid))
                    {
                        var selectedAnswer = question.Answers.FirstOrDefault(a => a.Id == answerGuid);

                        if (selectedAnswer != null && selectedAnswer.IsCorrect)
                        {
                            Score += question.Score;
                        }
                    }
                }
            }

            Percentage = Exam.TotalScore > 0 ? Math.Round((double)Score / Exam.TotalScore * 100, 2) : 0;

            // Check if student already took this exam
            try
            {
                var existing = await _studentExamService.GetAsync(currentuser.Id, Exam.Id);
                // Already exists - delete old result first
                await _studentExamService.DeleteAsync(Exam.Id, currentuser.Id);
            }
            catch
            {
                // Doesn't exist yet - that's fine, continue
            }

            await _studentExamService.CreateAsync(new CreateStudentExamDto
                    {
                        Score = Score,
                        ExamId = Exam.Id,
                        StudentId = currentuser.Id,
                        MaxScore=Exam.TotalScore,
                        Percentage=Percentage,
                        DateTaken=DateTime.Now,

            });
            ExamSubmitted = true;
           // Response.Redirect("Students.cshtml?Percentage=" + Percentage.ToString() );
            return Page();
        }
    }
}