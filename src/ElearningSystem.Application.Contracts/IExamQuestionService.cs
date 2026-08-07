using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Services;

namespace ElearningSystem
{
    public interface IExamQuestionService:IApplicationService
    {
        Task<bool> ExistsAsync(Guid ExamId, Guid QuestionId);
        Task<int> GetMaxOrderIndexAsync(Guid examId);
        Task<List<ExamQuestionDto>> GetByExamIdAsync(Guid ExamId);
        Task<ExamQuestionDto> GetAsync(Guid ExamId,Guid QuestionId);
        Task<ExamQuestionDto> CreateAsync(CreateExamQuestionDto input);
        Task DeleteAsync(Guid ExamId, Guid QuestionId);

    }
}
