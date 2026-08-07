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
    public class EditTeacherModel : PageModel
    {
        public TeacherDto Teachers { get; set; }
        [BindProperty]
        public UpdateTeacherDto TeacherInput { get; set; }

        private readonly ITeacherService _teacherService;
        public EditTeacherModel(ITeacherService teacherService)
        {
            _teacherService = teacherService;
        }
        public async Task OnGetAsync(Guid id)
        {
            Teachers = await _teacherService.GetAsync(id);
            System.Diagnostics.Debug.WriteLine($"Teacher name: {Teachers.FirstName}");

            TeacherInput = new UpdateTeacherDto
            {
                Id = Teachers.Id,
                UserId = Teachers.UserId,
                Email = Teachers.Email,
                FirstName = Teachers.FirstName
            ,
                LastName = Teachers.LastName
            };
        }
        public async Task<IActionResult> OnPostUpdateAsync()
        {

            await _teacherService.UpdateAsync(TeacherInput);
            return RedirectToPage(new { id = TeacherInput.Id });

        }
    }
}
