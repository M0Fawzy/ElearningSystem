using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Services;

namespace ElearningSystem
{
     public interface ITeacherCourseService : IApplicationService
     {
        Task<List<CourseDto>> GetTeacherCoursesAsync(Guid teacherId);
        Task<List<TeacherDto>> GetCourseTeachersAsync(Guid courseId);
        Task EnrollTeacherAsync(EnrollTeacherDto input);
        Task UnenrollTeacherAsync(Guid seacherId, Guid courseId);
        Task<bool> IsTeacherEnrolledAsync(Guid teacherId, Guid courseId);
     }
}
