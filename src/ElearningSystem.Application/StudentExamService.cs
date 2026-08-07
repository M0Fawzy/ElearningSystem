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
    public class StudentExamService : ApplicationService, IStudentExamService
    {
        private readonly IRepository<StudentExam,Guid> _studentExamRepository;

        public StudentExamService(IRepository<StudentExam, Guid> studentExamRepository)
        {
            _studentExamRepository = studentExamRepository;
        }

        public async Task<StudentExamDto> CreateAsync(CreateStudentExamDto input)
        {
            var studentexam=new StudentExam 
            {
            StudentId = input.StudentId,
            ExamId = input.ExamId,
            Score = input.Score,
            Percentage = input.Percentage,
            DateTaken = DateTime.Now,
            MaxScore = input.MaxScore,
            
            };
            await _studentExamRepository.InsertAsync(studentexam, autoSave: true);
            return ObjectMapper.Map<StudentExam, StudentExamDto>(studentexam);
        }

        public async Task DeleteAsync(Guid examId, Guid studentId)
        {
            var studentExam = await _studentExamRepository.FirstOrDefaultAsync(
                    se => se.StudentId == studentId && se.ExamId == examId);

            if (studentExam != null)
            {
                await _studentExamRepository.DeleteAsync(studentExam);
            }
        }

        public async Task<StudentExamDto> GetAsync(Guid studentId, Guid examId)
        {
            var studentExam = await _studentExamRepository.FirstOrDefaultAsync(
                se => se.StudentId == studentId && se.ExamId == examId);

            //if (studentExam == null)
            //    throw new Exception("Student exam result not found");

            return ObjectMapper.Map<StudentExam, StudentExamDto>(studentExam);
        }

        public async Task<List<StudentExamDto>> GetByStudentIdAsync(Guid studentId)
        {
            var results = await _studentExamRepository.GetListAsync(
                se => se.StudentId == studentId);
            return ObjectMapper.Map<List<StudentExam>, List<StudentExamDto>>(results);
        }

        public async Task<List<StudentExamDto>> GetByExamIdAsync(Guid examId)
        {
            var results = await _studentExamRepository.GetListAsync(se => se.ExamId == examId);
            return ObjectMapper.Map<List<StudentExam>, List<StudentExamDto>>(results);
        }
    }
}
