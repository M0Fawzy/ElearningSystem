using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Services;

namespace ElearningSystem
{
    public interface ICourseService : IApplicationService
    {
        Task<List<CourseDto>> GetListAsync();
        Task<CourseDto> GetAsync(Guid id);
        Task<CourseDto> CreateAsync(CreateCourseDto input);
        Task UpdateAsync(Guid id, CreateCourseDto input);
        Task DeleteAsync(Guid id);
    }
}
