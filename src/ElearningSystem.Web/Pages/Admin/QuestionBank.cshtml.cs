using ElearningSystem.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace ElearningSystem.Web.Pages.Admin
{
    [Authorize(Roles = "admin")]
    public class QuestionBankModel : PageModel
    {
        private readonly ICourseService _courseService;
        private readonly IQuestionService _questionService;
        private readonly IAnswerService _answerService;

        public QuestionBankModel(ICourseService courseService, IQuestionService questionService, IAnswerService answerService)
        {
            _courseService = courseService;
            _questionService = questionService;
            _answerService = answerService;
        }


        [BindProperty]
        public CreateQuestionDto QuestionInput { get; set; }

        [BindProperty]
        public List<AnswerInput> AnswerInputs { get; set; } = new();

        [BindProperty]
        public string? EssayCorrectAnswer { get; set; }


        public ExamDto CurrentExam { get; set; }
        public List<QuestionDto> Questions { get; set; } = new();
        public CourseDto Courses { get; set; } 
        

        public class AnswerInput
        {
            
            public string? Text { get; set; }
            public bool IsCorrect { get; set; }
        }

        public async Task OnGetAsync(Guid id)
        {
            Courses = await _courseService.GetAsync(id);
            QuestionInput = new CreateQuestionDto();
            AnswerInputs = new List<AnswerInput> { new AnswerInput() };
            Questions = await _questionService.GetListByCourseIdAsync(id);
            foreach (var question in Questions)
            {
                question.Answers = await _answerService.GetByQuestionIdAsync(question.Id);
            }

        }

        public async Task<IActionResult> OnPostCreateAsync(Guid id)
        {
            // Manually bind question data
            if (!Request.Form.TryGetValue("QuestionInput.QuestionText", out var questionText))
            {
                questionText = "";
            }
            if (!Request.Form.TryGetValue("QuestionInput.Score", out var scoreStr))
            {
                scoreStr = "0";
            }
            if (!Request.Form.TryGetValue("QuestionInput.QuestionType", out var questionType))
            {
                questionType = "MultipleChoice";
            }

            // Handle file upload
            string? imagePath = null;
            var imageFile = Request.Form.Files.GetFile("QuestionImage");

            if (imageFile != null && imageFile.Length > 0)
            {
                // Validate file type
                var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".gif" };
                var extension = Path.GetExtension(imageFile.FileName).ToLower();

                if (!allowedExtensions.Contains(extension))
                {
                    ModelState.AddModelError(string.Empty, "Only image files are allowed");
                    await OnGetAsync(id);
                    return Page();
                }

                // Validate file size (5MB max)
                if (imageFile.Length > 5 * 1024 * 1024)
                {
                    ModelState.AddModelError(string.Empty, "Image size must be less than 5MB");
                    await OnGetAsync(id);
                    return Page();
                }

                // Create directory if doesn't exist
                var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images", "questions");
                Directory.CreateDirectory(uploadsFolder);

                // Generate unique filename
                var fileName = Guid.NewGuid().ToString() + extension;
                var filePath = Path.Combine(uploadsFolder, fileName);

                // Save file
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await imageFile.CopyToAsync(stream);
                }

                // Store relative path for database
                imagePath = $"/images/questions/{fileName}";
            }

            if (string.IsNullOrWhiteSpace(questionText))
            {
                ModelState.AddModelError(string.Empty, "Please fill in question text");
                await OnGetAsync(id);
                return Page();
            }

            if (!int.TryParse(scoreStr, out var score) || score <= 0)
            {
                ModelState.AddModelError(string.Empty, "Please adjust score to be more than 0");
                await OnGetAsync(id);
                return Page();
            }
            if (!Request.Form.TryGetValue("AnswerInputs[0].Text", out var Text))
            {
                Text = "";
            }
            if (QuestionInput.QuestionType == QuestionType.MultipleChoice)
            {
                if (string.IsNullOrWhiteSpace(Text))
                {
                    ModelState.AddModelError(string.Empty, "Please fill in Answer");
                    await OnGetAsync(id);
                    return Page();
                }
            }
            if (!Request.Form.TryGetValue("EssayCorrectAnswer", out var essayText))
            {
                essayText = ""; 
            }
            if (QuestionInput.QuestionType==QuestionType.Essay) {
                if (string.IsNullOrWhiteSpace(essayText))
                {
                    ModelState.AddModelError(string.Empty, "Please fill in EssayAnswer");
                    await OnGetAsync(id);
                    return Page();
                }
            }
            try
            {
                QuestionInput = new CreateQuestionDto
                {
                    CourseId = id,
                    QuestionText = questionText.ToString(),
                    QuestionType = (QuestionType)Enum.Parse(typeof(QuestionType), questionType.ToString()),
                    Score = score,
                    ImagePath=imagePath
                };
                 var createdQuestion = await _questionService.CreateAsync(QuestionInput);
                // Create answers if not essay type
                if (QuestionInput.QuestionType != QuestionType.Essay)
                {
                    var answerTexts = Request.Form.Where(x => x.Key.StartsWith("AnswerInputs[") && x.Key.Contains("].Text")).ToList();

                    if (answerTexts.Count == 0)
                    {
                        await _questionService.DeleteAsync(createdQuestion.Id);
                        ModelState.AddModelError(string.Empty, "Please add at least one answer");
                        await OnGetAsync(id);
                        return Page();
                    }

                    for (int i = 0; i < answerTexts.Count; i++)
                    {
                        var answerText = answerTexts[i].Value.ToString();
                        var isCorrectKey = $"AnswerInputs[{i}].IsCorrect";
                        var isCorrect = Request.Form.ContainsKey(isCorrectKey);

                        if (!string.IsNullOrWhiteSpace(answerText))
                        {
                            await _answerService.CreateAsync(new CreateAnswerDto
                            {
                                QuestionId = createdQuestion.Id,
                                AnswerText = answerText,
                                IsCorrect = isCorrect
                            });
                        }
                    }
                }
                else
                {
                    // For essay
                    if (!Request.Form.TryGetValue("EssayCorrectAnswer", out var essayAnswer) || string.IsNullOrWhiteSpace(essayAnswer))
                    {
                        await _questionService.DeleteAsync(createdQuestion.Id);
                        ModelState.AddModelError(string.Empty, "Please enter the correct essay answer");
                        await OnGetAsync(id);
                        return Page();
                    }

                    await _answerService.CreateAsync(new CreateAnswerDto
                    {
                        QuestionId = createdQuestion.Id,
                        AnswerText = essayAnswer.ToString(),
                        IsCorrect = true
                    });

                }
                
                return RedirectToPage(new { id = createdQuestion.CourseId });
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, $"Error: {ex.InnerException?.Message ?? ex.Message}");
                await OnGetAsync(id);
                return Page();
            }
        }
        public async Task<IActionResult> OnPostUpdateQuestionAsync(Guid id,Guid questionId)
        {
            // Manually bind question data
            if (!Request.Form.TryGetValue("QuestionInput.QuestionText", out var questionText))
            {
                questionText = "";
            }
            if (!Request.Form.TryGetValue("QuestionInput.Score", out var scoreStr))
            {
                scoreStr = "0";
            }
            if (!Request.Form.TryGetValue("QuestionInput.QuestionType", out var questionType))
            {
                questionType = "MultipleChoice";
            }

            // Handle file upload for UPDATE
            string? imagePath = null;
            var imageFile = Request.Form.Files.GetFile("QuestionImage");

            if (imageFile != null && imageFile.Length > 0)
            {
                // Get the existing question to find old image
                var existingQuestion = await _questionService.GetAsync(questionId);

                // Delete old image if exists
                if (!string.IsNullOrEmpty(existingQuestion.ImagePath))
                {
                    var oldImagePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", existingQuestion.ImagePath.TrimStart('/'));
                    if (System.IO.File.Exists(oldImagePath))
                    {
                        System.IO.File.Delete(oldImagePath);
                    }
                }

                // Same validation and save logic as in Create
                var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".gif" };
                var extension = Path.GetExtension(imageFile.FileName).ToLower();

                if (!allowedExtensions.Contains(extension))
                {
                    ModelState.AddModelError(string.Empty, "Only image files are allowed");
                    await OnGetAsync(id);
                    return Page();
                }

                if (imageFile.Length > 5 * 1024 * 1024)
                {
                    ModelState.AddModelError(string.Empty, "Image size must be less than 5MB");
                    await OnGetAsync(id);
                    return Page();
                }

                var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images", "questions");
                Directory.CreateDirectory(uploadsFolder);

                var fileName = Guid.NewGuid().ToString() + extension;
                var filePath = Path.Combine(uploadsFolder, fileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await imageFile.CopyToAsync(stream);
                }

                imagePath = $"/images/questions/{fileName}";
            }
            else
            {
                // No new file uploaded - keep existing image
                var existingQuestion = await _questionService.GetAsync(questionId);
                imagePath = existingQuestion.ImagePath;
            }

            if (string.IsNullOrWhiteSpace(questionText))
            {
                ModelState.AddModelError(string.Empty, "Please fill in question text");
                await OnGetAsync(id);
                return Page();
            }

            if (!int.TryParse(scoreStr, out var score) || score <= 0)
            {
                ModelState.AddModelError(string.Empty, "Please adjust score to be more than 0");
                await OnGetAsync(id);
                return Page();
            }
            if (!Request.Form.TryGetValue("AnswerInputs[0].Text", out var Text))
            {
                Text = "";
            }
            if (QuestionInput.QuestionType == QuestionType.MultipleChoice)
            {
                if (string.IsNullOrWhiteSpace(Text))
                {
                    ModelState.AddModelError(string.Empty, "Please fill in Answer");
                    await OnGetAsync(id);
                    return Page();
                }
            }
            if (!Request.Form.TryGetValue("EssayCorrectAnswer", out var essayText))
            {
                essayText = "";
            }
            if (QuestionInput.QuestionType == QuestionType.Essay)
            {
                if (string.IsNullOrWhiteSpace(essayText))
                {
                    ModelState.AddModelError(string.Empty, "Please fill in EssayAnswer");
                    await OnGetAsync(id);
                    return Page();
                }
            }
            try
            {
                var oldanswers=await _answerService.GetByQuestionIdAsync(questionId);
                foreach(var Ans in oldanswers)
                {
                    await _answerService.DeleteAsync(Ans.Id);
                }
                Courses= await _courseService.GetAsync(id);

                QuestionInput = new CreateQuestionDto
                {
                    CourseId = id,
                    QuestionText = questionText.ToString(),
                    QuestionType = (QuestionType)Enum.Parse(typeof(QuestionType), questionType.ToString()),
                    Score = score,
                    ImagePath = imagePath
                };

                await _questionService.UpdateQuestionAsync(questionId,QuestionInput);
                // Create answers if not essay type
                if (QuestionInput.QuestionType != QuestionType.Essay)
                {
                    var answerTexts = Request.Form.Where(x => x.Key.StartsWith("AnswerInputs[") && x.Key.Contains("].Text")).ToList();

                    if (answerTexts.Count == 0)
                    {
                        await _questionService.DeleteAsync(questionId);
                        ModelState.AddModelError(string.Empty, "Please add at least one answer");
                        await OnGetAsync(id);
                        return Page();
                    }

                    for (int i = 0; i < answerTexts.Count; i++)
                    {
                        var answerText = answerTexts[i].Value.ToString();
                        var isCorrectKey = $"AnswerInputs[{i}].IsCorrect";
                        var isCorrect = Request.Form.ContainsKey(isCorrectKey);

                        if (!string.IsNullOrWhiteSpace(answerText))
                        {
                            await _answerService.CreateAsync(new CreateAnswerDto
                            {
                                QuestionId = questionId,
                                AnswerText = answerText,
                                IsCorrect = isCorrect
                            });
                        }
                    }
                }
                else
                {
                    // For essay
                    if (!Request.Form.TryGetValue("EssayCorrectAnswer", out var essayAnswer) || string.IsNullOrWhiteSpace(essayAnswer))
                    {
                        await _questionService.DeleteAsync(questionId);
                        ModelState.AddModelError(string.Empty, "Please enter the correct essay answer");
                        await OnGetAsync(id);
                        return Page();
                    }

                    await _answerService.CreateAsync(new CreateAnswerDto
                    {
                        QuestionId = questionId,
                        AnswerText = essayAnswer.ToString(),
                        IsCorrect = true
                    });

                }

                return RedirectToPage(new { id = Courses.Id });
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, $"Error: {ex.InnerException?.Message ?? ex.Message}");
                await OnGetAsync(id);
                return Page();
            }
        }
        
        public async Task<IActionResult> OnPostRemoveQuestionAsync(Guid courseId, Guid questionId)
        {
            try
            {
                Courses=await _courseService.GetAsync(courseId);
                await _questionService.DeleteAsync(questionId);
                return RedirectToPage(new { id = Courses.Id });
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, $"Error: {ex.Message}");
                await OnGetAsync(courseId);
                return Page();
            }
        }
    }
}
