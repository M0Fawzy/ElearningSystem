using System.Threading.Tasks;
using ElearningSystem.Localization;
using ElearningSystem.MultiTenancy;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Identity.Web.Navigation;
using Volo.Abp.SettingManagement.Web.Navigation;
using Volo.Abp.TenantManagement.Web.Navigation;
using Volo.Abp.UI.Navigation;

namespace ElearningSystem.Web.Menus;

public class ElearningSystemMenuContributor : IMenuContributor
{
    public async Task ConfigureMenuAsync(MenuConfigurationContext context)
    {
        if (context.Menu.Name == StandardMenus.Main)
        {
            await ConfigureMainMenuAsync(context);
        }
    }

    private async Task ConfigureMainMenuAsync(MenuConfigurationContext context)
    {
        var currentUser = context.ServiceProvider.GetRequiredService<Volo.Abp.Users.ICurrentUser>();

        if (currentUser.IsInRole("admin"))
        {
            context.Menu.Items.Add(new ApplicationMenuItem(
                "StudentDashboard", "Dashboard", "/Index", icon: "fas fa-home"
            ));
            context.Menu.Items.Add(new ApplicationMenuItem(
                "Courses", "Courses", "/Admin/Courses", icon: "fas fa-book"
            ));
            context.Menu.Items.Add(new ApplicationMenuItem(
                "Students", "Students", "/Admin/Students", icon: "fas fa-users"
            ));
            context.Menu.Items.Add(new ApplicationMenuItem(
                "Teachers", "Teachers", "/Admin/Teachers", icon: "fas fa-chalkboard-teacher"
            ));
            context.Menu.Items.Add(new ApplicationMenuItem(
                "Exams", "Exams", "/Exams/Exam", icon: "fas fa-file-alt"
            ));
        }
        else if (currentUser.IsInRole("Teacher"))
        {
            context.Menu.Items.Add(new ApplicationMenuItem(
                "Dashboard", "Dashboard", "/Teacher/Dashboard", icon: "fas fa-tachometer-alt"
            ));

            context.Menu.Items.Add(new ApplicationMenuItem(
                "TeacherLectures", "Lectures", "/Teacher/Lectures", icon: "fas fa-video"
            ));

            context.Menu.Items.Add(new ApplicationMenuItem(
                "TeacherExams", "Exams", "/Exams/TeacherExams", icon: "fas fa-clipboard-list"
            ));

            context.Menu.Items.Add(new ApplicationMenuItem(
                "Results", "Results", "/Teacher/Dashboard", icon: "fas fa-chart-bar"
            ));
        }
        else
        {
            // student
            context.Menu.Items.Add(new ApplicationMenuItem(
                "StudentDashboard", "Dashboard", "/Index", icon: "fas fa-home"
            ));

            
            context.Menu.Items.Add(new ApplicationMenuItem(
                "StudentLectures", "Lectures", "/Student/Lectures", icon: "fas fa-video"
            ));
        }
    }
}