using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Services;

namespace ElearningSystem
{
    public interface IStudentExamService:IApplicationService
    {
        Task<StudentExamDto> GetAsync(Guid ExamId, Guid StudentId);
        Task<StudentExamDto> CreateAsync(CreateStudentExamDto input);
        Task DeleteAsync(Guid ExamId, Guid StudentId);
        Task<List<StudentExamDto>> GetByStudentIdAsync(Guid studentId);
        Task<List<StudentExamDto>> GetByExamIdAsync(Guid examId);
    }
}
