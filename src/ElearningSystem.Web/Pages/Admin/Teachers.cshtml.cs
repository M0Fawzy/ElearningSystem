using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ElearningSystem.Web.Pages.Admin
{
    [Authorize(Roles = "admin")]
    public class TeachersModel : PageModel
    {
        private readonly ITeacherService _teacherService;

        public TeachersModel(ITeacherService teacherService)
        {
            _teacherService = teacherService;
            
        }

        [BindProperty]
        public CreateTeacherDto TeacherInput { get; set; }

        public List<TeacherDto> Teachers { get; set; } = new();

        public async Task OnGetAsync()
        {
            Teachers = await _teacherService.GetListAsync();

            TeacherInput = new CreateTeacherDto();
        }

        public async Task<IActionResult> OnPostCreateAsync()
        {
            if (!ModelState.IsValid)
            {
                await OnGetAsync();
                System.Diagnostics.Debug.WriteLine($"Teacher name: {TeacherInput.FirstName}");

                return Page();
            }

            try
            {
                await _teacherService.CreateAsync(TeacherInput);
                System.Diagnostics.Debug.WriteLine($"Teacher name: {TeacherInput.FirstName}");

                return RedirectToPage();
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                await OnGetAsync();
                return Page();
            }
        }

        public async Task<IActionResult> OnPostDeleteAsync(Guid id)
        {
            try
            {
                await _teacherService.DeleteAsync(id);
                return RedirectToPage();
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, $"Error: {ex.Message}");
                await OnGetAsync();
                return Page();
            }
        }
    }
}
