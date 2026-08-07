using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Services;

namespace ElearningSystem
{
    public interface IStudentService : IApplicationService
    {
        Task<List<StudentDto>> GetListAsync();
        Task<StudentDto> GetAsync(Guid id);
        Task<StudentDto> CreateAsync(CreateStudentDto input);
        //Task UpdateAsync(Guid id, CreateStudentDto input);
        Task DeleteAsync(Guid id);
        Task<StudentDto> GetByEmailAsync(string email);
        Task<StudentDto> GetCurrentStudentAsync(); // Get logged-in student
        Task UpdateAsync(UpdateStudentDto studentInput);
    }
}
