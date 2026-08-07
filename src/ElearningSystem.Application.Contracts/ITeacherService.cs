using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Services;

namespace ElearningSystem
{
    public interface ITeacherService: IApplicationService
    {
        Task<List<TeacherDto>> GetListAsync();
        Task<TeacherDto> GetAsync(Guid id);
        Task<TeacherDto> CreateAsync(CreateTeacherDto input);
        Task DeleteAsync(Guid id);
        Task<TeacherDto> GetByEmailAsync(string email);
        Task<TeacherDto> GetCurrentTeacherAsync(); // Get logged-in Teacher
        Task UpdateAsync(UpdateTeacherDto teacherInput);
    }
}
