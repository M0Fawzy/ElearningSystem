using AutoMapper.Internal.Mappers;
using ElearningSystem.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;

namespace ElearningSystem
{
    public class StudentCourseService : ApplicationService, IStudentCourseService
    {
        private readonly IRepository<StudentCourse, Guid> _studentCourseRepository;
        private readonly IRepository<Course, Guid> _courseRepository;
        private readonly IRepository<Student, Guid> _studentRepository;

        public StudentCourseService(
            IRepository<StudentCourse, Guid> studentCourseRepository,
            IRepository<Course, Guid> courseRepository,
            IRepository<Student, Guid> studentRepository)
        {
            _studentCourseRepository = studentCourseRepository;
            _courseRepository = courseRepository;
            _studentRepository = studentRepository;
        }

        public async Task EnrollStudentAsync(EnrollStudentDto input)
        {
            var isEnrolled = await IsStudentEnrolledAsync(input.StudentId, input.CourseId);
            if (isEnrolled)
                throw new Exception("Student already enrolled in this course");

            var studentCourse = new StudentCourse
            {
                StudentId = input.StudentId,
                CourseId = input.CourseId,
                EnrollmentDate = DateTime.Now
            };

            await _studentCourseRepository.InsertAsync(studentCourse, autoSave: true);
        }

        public async Task UnenrollStudentAsync(Guid studentId, Guid courseId)
        {
            var enrollment = await _studentCourseRepository.FirstOrDefaultAsync(
                sc => sc.StudentId == studentId && sc.CourseId == courseId);

            if (enrollment != null)
            {
                await _studentCourseRepository.DeleteAsync(enrollment);
            }
        }

        public async Task<List<CourseDto>> GetStudentCoursesAsync(Guid studentId)
        {
            var enrollments = await _studentCourseRepository.GetListAsync(
                sc => sc.StudentId == studentId);

            var courseIds = enrollments.Select(sc => sc.CourseId).ToList();
            var courses = await _courseRepository.GetListAsync(c => courseIds.Contains(c.Id));

            return ObjectMapper.Map<List<Course>, List<CourseDto>>(courses);
        }

        public async Task<List<StudentDto>> GetCourseStudentsAsync(Guid courseId)
        {
            var enrollments = await _studentCourseRepository.GetListAsync(
                sc => sc.CourseId == courseId);

            var studentIds = enrollments.Select(sc => sc.StudentId).ToList();
            var students = await _studentRepository.GetListAsync(s => studentIds.Contains(s.Id));

            return ObjectMapper.Map<List<Student>, List<StudentDto>>(students);
        }

        public async Task<bool> IsStudentEnrolledAsync(Guid studentId, Guid courseId)
        {
            var enrollment = await _studentCourseRepository.FirstOrDefaultAsync(
                sc => sc.StudentId == studentId && sc.CourseId == courseId);

            return enrollment != null;
        }
    }
}
