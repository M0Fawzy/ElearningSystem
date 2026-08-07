using AutoMapper.Internal.Mappers;
using ElearningSystem.Entities;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Guids;
using Volo.Abp.Identity;
using Volo.Abp.ObjectMapping;
using Volo.Abp.Users;

namespace ElearningSystem
{
    public class TeacherService : ApplicationService, ITeacherService
    {
        private readonly IRepository<Teacher, Guid> _teacherRepository;
        private readonly ICurrentUser _currentUser;
        private readonly IdentityUserManager _userManager;

        public TeacherService(
            IRepository<Teacher, Guid> teacherRepository,
            ICurrentUser currentUser,
            IdentityUserManager userManager)
        {
            _teacherRepository = teacherRepository;
            _currentUser = currentUser;
            _userManager = userManager;
        }

        public async Task<TeacherDto> CreateAsync(CreateTeacherDto input)
        {
            // Create Identity User
            var user = new IdentityUser(GuidGenerator.Create(), input.FirstName + input.LastName, input.Email)
            {
                Name = input.FirstName,
                Surname = input.LastName

            };

            var result = await _userManager.CreateAsync(user, input.Password);
            if (!result.Succeeded)
            {
                var errors = string.Join(", ", result.Errors.Select(e => e.Description));
                throw new Exception($"Failed to create user: {errors}");
            }


            // Create Student
            var teacher = new Teacher
            {
                FirstName = input.FirstName,
                LastName = input.LastName,
                Email = input.Email,
                UserId = user.Id.ToString(),
                EnrollmentDate = DateTime.Now
            };
            System.Diagnostics.Debug.WriteLine($"Teacher name: {input.FirstName}");

            await _teacherRepository.InsertAsync(teacher, autoSave: true);
            return ObjectMapper.Map<Teacher, TeacherDto>(teacher);
        }

        public async Task DeleteAsync(Guid id)
        {
            await _teacherRepository.DeleteAsync(id);
        }

        public async Task<List<TeacherDto>> GetListAsync()
        {
            var teachers = await _teacherRepository.GetListAsync();
            return ObjectMapper.Map<List<Teacher>, List<TeacherDto>>(teachers);
        }

        public async Task<TeacherDto> GetAsync(Guid id)
        {
            var teacher = await _teacherRepository.GetAsync(id);
            return ObjectMapper.Map<Teacher, TeacherDto>(teacher);
        }


        public async Task UpdateAsync(UpdateTeacherDto input)
        {
            var user = await _userManager.GetByIdAsync(new Guid(input.UserId));

            var teacher = await _teacherRepository.GetAsync(input.Id);

            user.Name = input.FirstName;
            user.Surname = input.LastName;

            teacher.FirstName = input.FirstName;
            teacher.LastName = input.LastName;
            teacher.Email = input.Email;
            await _teacherRepository.UpdateAsync(teacher);

            if (input.Password != null)
            {
                await _userManager.RemovePasswordAsync(user);
            }

            await _userManager.SetUserNameAsync(user, input.FirstName + input.LastName);
            await _userManager.SetEmailAsync(user, input.Email);
            await _userManager.AddPasswordAsync(user, input.Password);
            //await _userManager.CheckPasswordAsync(user, input.Password);

            await _userManager.UpdateNormalizedUserNameAsync(user);
            await _userManager.UpdateNormalizedEmailAsync(user);
            await _userManager.UpdateAsync(user);



        }

        public async Task<TeacherDto> GetByEmailAsync(string email)
        {
            var teacher = await _teacherRepository.FirstOrDefaultAsync(s => s.Email == email);
            if (teacher == null)
                throw new Exception("Teacher not found");
            return ObjectMapper.Map<Teacher, TeacherDto>(teacher);
        }

        public async Task<TeacherDto> GetCurrentTeacherAsync()
        {
            if (!_currentUser.IsAuthenticated)
                throw new Exception("User not authenticated");

            var userId = _currentUser.Id.ToString();
            var teacher = await _teacherRepository.FirstOrDefaultAsync(s => s.UserId == userId);

            if (teacher == null)
                throw new Exception("Teacher not found");

            return ObjectMapper.Map<Teacher, TeacherDto>(teacher);
        }
    }
}
