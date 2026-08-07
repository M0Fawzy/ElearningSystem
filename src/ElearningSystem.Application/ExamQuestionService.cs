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
    public class ExamQuestionService : ApplicationService, IExamQuestionService
    {
        private readonly IRepository<ExamQuestion,Guid> _examQuestionRepository;

        public ExamQuestionService(IRepository<ExamQuestion, Guid> examQuestionRepository)
        {
            _examQuestionRepository = examQuestionRepository;
        }

        public async Task<ExamQuestionDto> CreateAsync(CreateExamQuestionDto input)
        {
            var examquestion=new ExamQuestion 
            {
            ExamId = input.ExamId,
            QuestionId = input.QuestionId,
            OrderIndex = input.OrderIndex,
            };
            await _examQuestionRepository.InsertAsync(examquestion,autoSave:true);
            return ObjectMapper.Map<ExamQuestion, ExamQuestionDto>(examquestion);
        }

        public async Task DeleteAsync(Guid ExamId, Guid QuestionId)
        {
            var examquestion=await _examQuestionRepository.FirstOrDefaultAsync(eq=>eq.ExamId == ExamId &&eq.QuestionId==QuestionId);
            if (examquestion!=null) {
            await _examQuestionRepository.DeleteAsync(examquestion);
                }
        }

        public async Task<ExamQuestionDto> GetAsync(Guid ExamId, Guid QuestionId)
        {
            var examquestion=await _examQuestionRepository.FirstOrDefaultAsync(eq=>eq.ExamId==ExamId&&eq.QuestionId==QuestionId);
            if (examquestion == null)
                throw new Exception($"ExamQuestion not found for Exam: {ExamId}, Question: {QuestionId}");
            return ObjectMapper.Map<ExamQuestion, ExamQuestionDto>(examquestion);
        }

        public async Task<List<ExamQuestionDto>> GetByExamIdAsync(Guid ExamId)
        {
            var examquestion = await _examQuestionRepository.GetListAsync(
                eq => eq.ExamId== ExamId
                );
            return ObjectMapper.Map<List<ExamQuestion>, List<ExamQuestionDto>>(examquestion);
        }
        public async Task<int> GetMaxOrderIndexAsync(Guid examId)
        {
            var examQuestions = await _examQuestionRepository.GetListAsync(
                eq => eq.ExamId == examId
            );

            if (examQuestions.Count == 0)
                return 0;

            return examQuestions.Max(eq => eq.OrderIndex);
        }

        public async Task<bool> ExistsAsync(Guid ExamId, Guid QuestionId)
        {
            return await _examQuestionRepository.AnyAsync(
              eq => eq.ExamId == ExamId && eq.QuestionId == QuestionId
                );
        }
    }
}
