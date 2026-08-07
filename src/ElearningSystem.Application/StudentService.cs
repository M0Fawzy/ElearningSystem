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
    public class StudentService : ApplicationService, IStudentService
    {
        private readonly IRepository<Student, Guid> _studentRepository;
        private readonly ICurrentUser _currentUser;
        private readonly IdentityUserManager _userManager;

        public StudentService(
            IRepository<Student, Guid> studentRepository,
            ICurrentUser currentUser,
            IdentityUserManager userManager)
        {
            _studentRepository = studentRepository;
            _currentUser = currentUser;
            _userManager = userManager;
        }

        public async Task<StudentDto> CreateAsync(CreateStudentDto input)
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
            var student = new Student
            {
                FirstName = input.FirstName,
                LastName = input.LastName,
                Email = input.Email,
                UserId = user.Id.ToString(),
                EnrollmentDate = DateTime.Now
            };
            System.Diagnostics.Debug.WriteLine($"Student name: {input.FirstName}");

            await _studentRepository.InsertAsync(student, autoSave: true);
            return ObjectMapper.Map<Student, StudentDto>(student);
        }
        
        public async Task DeleteAsync(Guid id)
        {
            await _studentRepository.DeleteAsync(id);
        }

        public async Task<List<StudentDto>> GetListAsync()
        {
            var students = await _studentRepository.GetListAsync();
            return ObjectMapper.Map<List<Student>, List<StudentDto>>(students);
        }

        public async Task<StudentDto> GetAsync(Guid id)
        {
            var student = await _studentRepository.GetAsync(id);
            return ObjectMapper.Map<Student, StudentDto>(student);
        }

        
        public async Task UpdateAsync(UpdateStudentDto input)
        {
            var user = await _userManager.GetByIdAsync(new Guid ( input.UserId));

            var student = await _studentRepository.GetAsync(input.Id);

            user.Name = input.FirstName;
            user.Surname = input.LastName;                        

            student.FirstName = input.FirstName;
            student.LastName = input.LastName;
            student.Email= input.Email;
            await _studentRepository.UpdateAsync(student);

            if (input.Password != null)
            {
                await _userManager.RemovePasswordAsync(user);
            }

            await _userManager.SetUserNameAsync(user,input.FirstName+input.LastName);
            await _userManager.SetEmailAsync(user, input.Email);
            await _userManager.AddPasswordAsync(user, input.Password);
            //await _userManager.CheckPasswordAsync(user, input.Password);
            
            await _userManager.UpdateNormalizedUserNameAsync(user);
            await _userManager.UpdateNormalizedEmailAsync(user);
            await _userManager.UpdateAsync(user);
            


        }

        public async Task<StudentDto> GetByEmailAsync(string email)
        {
            var student = await _studentRepository.FirstOrDefaultAsync(s => s.Email == email);
            if (student == null)
                throw new Exception("Student not found");
            return ObjectMapper.Map<Student, StudentDto>(student);
        }

        public async Task<StudentDto> GetCurrentStudentAsync()
        {
            if (!_currentUser.IsAuthenticated)
                throw new Exception("User not authenticated");

            var userId = _currentUser.Id.ToString();
            var student = await _studentRepository.FirstOrDefaultAsync(s => s.UserId == userId);

            if (student == null)
                throw new Exception("Student not found");

            return ObjectMapper.Map<Student, StudentDto>(student);
        }
    }
}
