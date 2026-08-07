using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Services;

namespace ElearningSystem
{
    public interface IStudentCourseService : IApplicationService
    {
        Task<List<CourseDto>> GetStudentCoursesAsync(Guid studentId);
        Task<List<StudentDto>> GetCourseStudentsAsync(Guid courseId);
        Task EnrollStudentAsync(EnrollStudentDto input);
        Task UnenrollStudentAsync(Guid studentId, Guid courseId);
        Task<bool> IsStudentEnrolledAsync(Guid studentId, Guid courseId);
    }
}
