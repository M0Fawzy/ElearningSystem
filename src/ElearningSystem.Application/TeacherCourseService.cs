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
    public class TeacherCourseService: ApplicationService, ITeacherCourseService
    {
        private readonly IRepository<TeacherCourse, Guid> _teacherCourseRepository;
        private readonly IRepository<Course, Guid> _courseRepository;
        private readonly IRepository<Teacher, Guid> _teacherRepository;

        public TeacherCourseService(
            IRepository<TeacherCourse, Guid> teacherCourseRepository,
            IRepository<Course, Guid> courseRepository,
            IRepository<Teacher, Guid> teacherRepository)
        {
            _teacherCourseRepository = teacherCourseRepository;
            _courseRepository = courseRepository;
            _teacherRepository = teacherRepository;
        }

        public async Task EnrollTeacherAsync(EnrollTeacherDto input)
        {
            var isEnrolled = await IsTeacherEnrolledAsync(input.TeacherId, input.CourseId);
            if (isEnrolled)
                throw new Exception("Teacher already enrolled in this course");

            var teacherCourse = new TeacherCourse
            {
                TeacherId = input.TeacherId,
                CourseId = input.CourseId
            };

            await _teacherCourseRepository.InsertAsync(teacherCourse, autoSave: true);
        }

        public async Task UnenrollTeacherAsync(Guid teacherId, Guid courseId)
        {
            var enrollment = await _teacherCourseRepository.FirstOrDefaultAsync(
                tc => tc.TeacherId == teacherId && tc.CourseId == courseId);

            if (enrollment != null)
            {
                await _teacherCourseRepository.DeleteAsync(enrollment);
            }
        }

        public async Task<List<CourseDto>> GetTeacherCoursesAsync(Guid teacherId)
        {
            var enrollments = await _teacherCourseRepository.GetListAsync(
                sc => sc.TeacherId == teacherId);

            var courseIds = enrollments.Select(sc => sc.CourseId).ToList();
            var courses = await _courseRepository.GetListAsync(c => courseIds.Contains(c.Id));

            return ObjectMapper.Map<List<Course>, List<CourseDto>>(courses);
        }

        public async Task<List<TeacherDto>> GetCourseTeachersAsync(Guid courseId)
        {
            var enrollments = await _teacherCourseRepository.GetListAsync(
                sc => sc.CourseId == courseId);

            var teacherIds = enrollments.Select(tc => tc.TeacherId).ToList();
            var teachers = await _teacherRepository.GetListAsync(s => teacherIds.Contains(s.Id));

            return ObjectMapper.Map<List<Teacher>, List<TeacherDto>>(teachers);
        }

        public async Task<bool> IsTeacherEnrolledAsync(Guid teacherId, Guid courseId)
        {
            var enrollment = await _teacherCourseRepository.FirstOrDefaultAsync(
                tc => tc.TeacherId == teacherId && tc.CourseId == courseId);

            return enrollment != null;
        }
    }
}
