using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Services;

namespace ElearningSystem
{
    public interface IExamService : IApplicationService
    {

        Task<List<ExamDto>> GetListAsync();
        Task<ExamDto> GetAsync(Guid id);
        Task<ExamDto> CreateAsync(CreateExamDto input);
        Task DeleteAsync(Guid id);
        Task UpdateExamAsync(CreateExamDto input);
        
    }
}
