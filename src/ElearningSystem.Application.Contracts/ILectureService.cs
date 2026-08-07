using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Services;

namespace ElearningSystem
{
    public interface ILectureService : IApplicationService
    {
        Task<List<LectureDto>> GetListAsync();
        Task<List<LectureDto>> GetByCourseIdAsync(Guid courseId);
        Task<LectureDto> GetAsync(Guid id);
        Task<LectureDto> CreateAsync(CreateLectureDto input, string fileName, string filePath, string fileType, long fileSize);
        Task DeleteAsync(Guid id);
    }
}
