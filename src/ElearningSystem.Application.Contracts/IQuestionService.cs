using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Services;

namespace ElearningSystem
{
    public interface IQuestionService : IApplicationService
    {
        Task<List<QuestionDto>> GetListByExamIdAsync(Guid ExamId);
        Task<List<QuestionDto>> GetListByCourseIdAsync(Guid courseId);
        Task<QuestionDto> CreateAsync(CreateQuestionDto input);
        Task DeleteAsync(Guid id);
        Task<QuestionDto> GetAsync(Guid id);
        Task UpdateQuestionAsync(Guid id,CreateQuestionDto input);
    }
}
