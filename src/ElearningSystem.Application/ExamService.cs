using AutoMapper.Internal.Mappers;
using ElearningSystem;
using ElearningSystem.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;

public class ExamService : ApplicationService, IExamService
{
    private readonly IRepository<Exam, Guid> _examRepository;

    public ExamService(IRepository<Exam, Guid> examRepository)
    {
        _examRepository = examRepository;
    }

    public async Task<ExamDto> CreateAsync(CreateExamDto input)
    {
        var exam = new Exam
        {
            Title = input.Title,
            TotalScore = input.TotalScore,
            CreatedDate = DateTime.Now,
            CourseId = input.CourseId,
            DurationMinutes=input.DurationMinutes
        };

        await _examRepository.InsertAsync(exam, autoSave: true);

        return ObjectMapper.Map<Exam, ExamDto>(exam);
    }

    public async Task DeleteAsync(Guid id)
    {
        await _examRepository.DeleteAsync(id);
    }

    public async Task<List<ExamDto>> GetListAsync()
    {
        var exams = await _examRepository.GetListAsync();
        return ObjectMapper.Map<List<Exam>, List<ExamDto>>(exams);
    }

    public async Task<ExamDto> GetAsync(Guid id)
    {
        var exam = await _examRepository.GetAsync(id);
        return ObjectMapper.Map<Exam, ExamDto>(exam);
    }

    

    public async Task UpdateExamAsync( CreateExamDto input)
    {
        var exam = await _examRepository.GetAsync(input.Id);
        exam.Title = input.Title;
        exam.TotalScore = input.TotalScore;
        exam.CreatedDate = DateTime.Now;
        exam.CourseId = input.CourseId;
        exam.DurationMinutes = input.DurationMinutes;

        await _examRepository.UpdateAsync(exam, autoSave: true);
        
    }
}
