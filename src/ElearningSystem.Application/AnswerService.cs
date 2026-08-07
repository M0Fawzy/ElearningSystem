using AutoMapper.Internal.Mappers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;

namespace ElearningSystem
{
    public class AnswerService : ApplicationService, IAnswerService
    {
        private readonly IRepository<Answer, Guid> _answerRepository;

        public AnswerService(IRepository<Answer, Guid> answerRepository)
        {
            _answerRepository = answerRepository;
        }

        public async Task<AnswerDto> CreateAsync(CreateAnswerDto input)
        {
            var answer = new Answer
            {
                QuestionId = input.QuestionId,
                AnswerText = input.AnswerText,
                IsCorrect = input.IsCorrect
            };

            await _answerRepository.InsertAsync(answer, autoSave: true);

            return ObjectMapper.Map<Answer, AnswerDto>(answer);
        }

        public async Task<List<AnswerDto>> GetByQuestionIdAsync(Guid questionId)
        {
            var answers = await _answerRepository
                .GetListAsync(a => a.QuestionId == questionId);

            return ObjectMapper.Map<List<Answer>, List<AnswerDto>>(answers);
        }

        public async Task DeleteAsync(Guid id)
        {
            await _answerRepository.DeleteAsync(id);
        }

        public async Task UpdateAnswerAsync( CreateAnswerDto input)
        {
            var answer = await _answerRepository.GetAsync(input.QuestionId);
            answer.QuestionId=input.QuestionId;
            answer.AnswerText = input.AnswerText;
            answer.IsCorrect = input.IsCorrect;
            await _answerRepository.UpdateAsync(answer,autoSave: true);
        }

        public async Task<AnswerDto> GetByIdAsync(Guid id)
        {
            var answer=await _answerRepository .GetAsync(id);
            return ObjectMapper.Map<Answer,AnswerDto>(answer);
        }
    }

}
