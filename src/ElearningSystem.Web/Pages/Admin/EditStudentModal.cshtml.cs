using AutoMapper.Internal.Mappers;
using ElearningSystem.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp.ObjectMapping;

namespace ElearningSystem.Web.Pages.Admin
{
    public class EditStudentModalModel : PageModel
    {    
        public StudentDto Students { get; set; } 
        [BindProperty]
        public UpdateStudentDto StudentInput { get; set; }

        private readonly IStudentService _studentService;
        public EditStudentModalModel(IStudentService studentService)
        {
            _studentService = studentService;
        }
        public async Task OnGetAsync(Guid id)
        {
            Students = await _studentService.GetAsync(id);
            System.Diagnostics.Debug.WriteLine($"Student name: {Students.FirstName}");

            StudentInput = new UpdateStudentDto
            { Id = Students.Id , UserId= Students.UserId , Email = Students.Email, FirstName=Students.FirstName
            ,LastName=Students.LastName};
        }
        public async Task<IActionResult> OnPostUpdateAsync()
        {
            
                await _studentService.UpdateAsync(StudentInput);
            return RedirectToPage(new { id = StudentInput.Id });
            
        }
    }
}
