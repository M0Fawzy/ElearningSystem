//using ElearningSystem;
//using Microsoft.AspNetCore.Mvc;
//using Microsoft.AspNetCore.Mvc.RazorPages;
//using System;
//using System.Collections.Generic;
//using System.Threading.Tasks;

//{
//    public class QuestionModel : PageModel
//{
//    private readonly IExamService _examService;
//    private readonly IQuestionService _questionService;
//    private readonly IAnswerService _answerService;
//    private readonly ICourseService _courseService;

//    public QuestionModel(
//        IExamService examService,
//        IQuestionService questionService,
//        IAnswerService answerService,
//        ICourseService courseService)
//    {
//        _examService = examService;
//        _questionService = questionService;
//        _answerService = answerService;
//        _courseService = courseService;
//    }

//    [BindProperty]
//    public CreateExamDto ExamInput { get; set; }

//    public CreateQuestionDto QuestionInput { get; set; }
//    public CreateAnswerDto AnswerInputss { get; set; }
//    public List<AnswerInput> AnswerInputs { get; set; } = new();
//    public string EssayCorrectAnswer { get; set; }

//    public ExamDto Exams { get; set; }

//    public QuestionDto Questionss { get; set; }

//    // public List<AnswerDto> Answers { get; set; }
//    public ExamDto CurrentExam { get; set; }
//    public List<QuestionDto> Questions { get; set; } = new();
//    public List<CourseDto> Courses { get; set; } = new();
//    public Guid idd { get; set; }

//    public async Task OnGetAsync(Guid? id)
//    {
//        if (id.HasValue && id != Guid.Empty)
//        {
//            Exams = await _examService.GetAsync(id.Value);
//            ExamInput = new CreateExamDto { Id = id.Value, Title = Exams.Title, CourseId = Exams.CourseId, TotalScore = Exams.TotalScore };
//            // Questions = await _questionService.GetListByExamIdAsync(id.Value);
//            // Answers = await _answerService.GetByQuestionIdAsync(Questions[0].Id);
//            //foreach (var question in Questions)
//            //{
//            //    question.Answers = await _answerService.GetByQuestionIdAsync(question.Id);
//            //}

//            // QuestionInput = new CreateQuestionDto { ExamId = Questions[0].ExamId, QuestionText = Questions[0].QuestionText, QuestionType = Questions[0].QuestionType, Score = Questions[0].Score };
//            //  AnswerInputss = new CreateAnswerDto { /*QuestionId = Answers[0].QuestionId,*/ AnswerText = Answers[0].AnswerText, IsCorrect = Answers[0].IsCorrect };
//            //CurrentExam = await _examService.GetAsync(id.Value);
//            idd = id.Value;
//        }
//        //Questions = await _questionService.GetListByExamIdAsync(idd);
//        // QuestionInput = new CreateQuestionDto { ExamId = Questions[0].ExamId, QuestionText = Questions[0].QuestionText, QuestionType = Questions[0].QuestionType, Score = Questions[0].Score };
//        foreach (var question in Questions)
//        {
//            question.Answers = await _answerService.GetByQuestionIdAsync(question.Id);
//        }
//        // Load courses for dropdown
//        Courses = await _courseService.GetListAsync();

//    }


//    public async Task<IActionResult> OnPostRemoveQuestionAsync(Guid examId, Guid questionId)
//    {
//        try
//        {
//            await _questionService.DeleteAsync(questionId);
//            return RedirectToPage(new { examId = examId });
//        }
//        catch (Exception ex)
//        {
//            ModelState.AddModelError(string.Empty, $"Error: {ex.Message}");
//            await OnGetAsync(examId);
//            return Page();
//        }
//    }

//    public async Task<IActionResult> OnPostUpdateQuestionAsync()
//    {
//        await _questionService.UpdateQuestionAsync(QuestionInput);
//        return RedirectToPage();
//    }

    
//}

