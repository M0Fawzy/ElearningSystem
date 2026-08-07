using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ElearningSystem.Web.Pages.Admin
{
    [Authorize(Roles = "admin")]
    public class CoursesModel : PageModel
    {
        private readonly ICourseService _courseService;

        public CoursesModel(ICourseService courseService)
        {
            _courseService = courseService;
        }

        [BindProperty]
        public CreateCourseDto CourseInput { get; set; }

        public List<CourseDto> Courses { get; set; } = new();

        public int CurrentPage { get; set; } = 1;     // NEW
        public int PageSize { get; set; } = 10;       // NEW (10 exams per page)
        public int TotalCount { get; set; }
        public async Task OnGetAsync(int currentPage = 1)
        {
            CurrentPage = currentPage;

            var allCourses = await _courseService.GetListAsync();
            CourseInput = new CreateCourseDto();

            // Total count
            TotalCount = allCourses.Count;

            // Calculate skip
            var skipCount = (CurrentPage - 1) * PageSize;

            // Paginate in memory
            Courses = allCourses
                .Skip(skipCount)
                .Take(PageSize)
                .ToList();
        }

        public async Task<IActionResult> OnPostCreateAsync()
        {
            if (!ModelState.IsValid)
            {
                await OnGetAsync();
                return Page();
            }

            try
            {
                await _courseService.CreateAsync(CourseInput);
                return RedirectToPage();
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, $"Error: {ex.Message}");
                await OnGetAsync();
                return Page();
            }
        }

        public async Task<IActionResult> OnPostDeleteAsync(Guid id)
        {
            try
            {
                await _courseService.DeleteAsync(id);
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