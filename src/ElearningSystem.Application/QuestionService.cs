using ElearningSystem.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;

namespace ElearningSystem
{
    public class QuestionService : ApplicationService, IQuestionService
    {
        private readonly IRepository<Question, Guid> _QuestionRepository;
        private readonly IRepository<ExamQuestion, Guid> _ExamQuestionRepository;

        public QuestionService(IRepository<Question, Guid> QuestionRepository,IRepository<ExamQuestion,Guid> ExamRepository)
        {
            _QuestionRepository = QuestionRepository;
            _ExamQuestionRepository = ExamRepository;
        }

        public async Task<QuestionDto> CreateAsync(CreateQuestionDto input)
        {

            var question = new Question
            {
                CourseId = input.CourseId,
                QuestionText = input.QuestionText,
                QuestionType = input.QuestionType,
                Score = input.Score,
                ImagePath=input.ImagePath
            };
            await _QuestionRepository.InsertAsync(question,autoSave:true);
            return ObjectMapper.Map<Question,QuestionDto>(question);
        }

        public async Task DeleteAsync(Guid id)
        {
            await _QuestionRepository.DeleteAsync(id);
        }

        public async Task<List<QuestionDto>> GetListByExamIdAsync(Guid examId)
        {
            // Get ExamQuestions for this exam, ordered by OrderIndex
            var examQuestions = await _ExamQuestionRepository.GetListAsync(
                eq => eq.ExamId == examId);

            // Order by OrderIndex
            var ordered = examQuestions.OrderBy(eq => eq.OrderIndex);

            // Get the actual questions and map to DTOs
            var questions = new List<QuestionDto>();
            foreach (var examQuestion in ordered)
            {
                var question = await _QuestionRepository.GetAsync(examQuestion.QuestionId);
                questions.Add(ObjectMapper.Map<Question, QuestionDto>(question));
            }

            return questions;
        }
        public async Task<List<QuestionDto>> GetListByCourseIdAsync(Guid courseId)
        {
            var questions = await _QuestionRepository
                .GetListAsync(q => q.CourseId == courseId);

            return ObjectMapper.Map<List<Question>, List<QuestionDto>>(questions);
        }
        public async Task<QuestionDto> GetAsync(Guid id)
        {
            var question= await _QuestionRepository.GetAsync(id);
            return ObjectMapper.Map<Question, QuestionDto>(question);
        }

        public async Task UpdateQuestionAsync(Guid id,CreateQuestionDto input)
        {
            var question = await _QuestionRepository.GetAsync(id);
            question.CourseId = input.CourseId;
            question.QuestionText = input.QuestionText;
            question.QuestionType = input.QuestionType;
            question.Score = input.Score;
            question.ImagePath = input.ImagePath;
            await _QuestionRepository.UpdateAsync(question, autoSave: true);
        }
    }
}
