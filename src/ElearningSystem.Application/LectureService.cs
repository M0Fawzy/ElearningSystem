using ElearningSystem.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;

namespace ElearningSystem
{
    public class LectureService : ApplicationService, ILectureService
    {
        private readonly IRepository<Lecture, Guid> _lectureRepository;

        public LectureService(IRepository<Lecture, Guid> lectureRepository)
        {
            _lectureRepository = lectureRepository;
        }

        public async Task<List<LectureDto>> GetListAsync()
        {
            var lectures = await _lectureRepository.GetListAsync();
            return ObjectMapper.Map<List<Lecture>, List<LectureDto>>(lectures);
        }

        public async Task<List<LectureDto>> GetByCourseIdAsync(Guid courseId)
        {
            var lectures = await _lectureRepository.GetListAsync(l => l.CourseId == courseId);
            return ObjectMapper.Map<List<Lecture>, List<LectureDto>>(lectures);
        }

        public async Task<LectureDto> GetAsync(Guid id)
        {
            var lecture = await _lectureRepository.GetAsync(id);
            return ObjectMapper.Map<Lecture, LectureDto>(lecture);
        }

        public async Task<LectureDto> CreateAsync(CreateLectureDto input, string fileName, string filePath, string fileType, long fileSize)
        {
            var lecture = new Lecture
            {
                Title = input.Title,
                CourseId = input.CourseId,
                TeacherId = input.TeacherId,
                FileName = fileName,
                FilePath = filePath,
                FileType = fileType,
                FileSize = fileSize
            };

            await _lectureRepository.InsertAsync(lecture, autoSave: true);
            return ObjectMapper.Map<Lecture, LectureDto>(lecture);
        }

        public async Task DeleteAsync(Guid id)
        {
            await _lectureRepository.DeleteAsync(id);
        }
    }
}