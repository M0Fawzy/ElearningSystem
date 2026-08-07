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
    public class CourseService : ApplicationService, ICourseService
    {
        private readonly IRepository<Course, Guid> _courseRepository;

        public CourseService(IRepository<Course, Guid> courseRepository)
        {
            _courseRepository = courseRepository;
        }

        public async Task<CourseDto> CreateAsync(CreateCourseDto input)
        {
            var course = new Course
            {
                Name = input.Name,
                Description = input.Description,
                CreatedDate = DateTime.Now
            };
            await _courseRepository.InsertAsync(course, autoSave: true);
            return ObjectMapper.Map<Course, CourseDto>(course);
        }

        public async Task DeleteAsync(Guid id)
        {
            await _courseRepository.DeleteAsync(id);
        }

        public async Task<List<CourseDto>> GetListAsync()
        {
            var courses = await _courseRepository.GetListAsync();
            return ObjectMapper.Map<List<Course>, List<CourseDto>>(courses);
        }

        public async Task<CourseDto> GetAsync(Guid id)
        {
            var course = await _courseRepository.GetAsync(id);
            return ObjectMapper.Map<Course, CourseDto>(course);
        }

        public async Task UpdateAsync(Guid id, CreateCourseDto input)
        {
            var course = await _courseRepository.GetAsync(id);
            course.Name = input.Name;
            course.Description = input.Description;
            await _courseRepository.UpdateAsync(course, autoSave: true);
        }
    }
}
