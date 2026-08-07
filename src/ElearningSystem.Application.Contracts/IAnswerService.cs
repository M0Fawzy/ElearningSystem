using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Services;

namespace ElearningSystem
{
    public interface IAnswerService : IApplicationService
    {
        Task<AnswerDto> CreateAsync(CreateAnswerDto input);
        Task<List<AnswerDto>> GetByQuestionIdAsync(Guid questionId);
        Task DeleteAsync(Guid id);
        Task <AnswerDto> GetByIdAsync(Guid id);
        Task UpdateAnswerAsync(CreateAnswerDto input);
    }


}
