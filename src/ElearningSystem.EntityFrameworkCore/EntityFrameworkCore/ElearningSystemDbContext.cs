using ElearningSystem.Entities;
using Microsoft.EntityFrameworkCore;
using Volo.Abp.AuditLogging.EntityFrameworkCore;
using Volo.Abp.BackgroundJobs.EntityFrameworkCore;
using Volo.Abp.Data;
using Volo.Abp.DependencyInjection;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.FeatureManagement.EntityFrameworkCore;
using Volo.Abp.Identity;
using Volo.Abp.Identity.EntityFrameworkCore;
using Volo.Abp.OpenIddict.EntityFrameworkCore;
using Volo.Abp.PermissionManagement.EntityFrameworkCore;
using Volo.Abp.SettingManagement.EntityFrameworkCore;
using Volo.Abp.TenantManagement;
using Volo.Abp.TenantManagement.EntityFrameworkCore;

namespace ElearningSystem.EntityFrameworkCore;

[ReplaceDbContext(typeof(IIdentityDbContext))]
[ReplaceDbContext(typeof(ITenantManagementDbContext))]
[ConnectionStringName("Default")]
public class ElearningSystemDbContext :
    AbpDbContext<ElearningSystemDbContext>,
    IIdentityDbContext,
    ITenantManagementDbContext
{
    /* Add DbSet properties for your Aggregate Roots / Entities here. */

    // Exam System
    public DbSet<Exam> Exams { get; set; }
    public DbSet<Question> Questions { get; set; }
    public DbSet<Answer> Answers { get; set; }
    public DbSet<ExamQuestion> ExamQuestions { get; set; }
    public DbSet<StudentExam> StudentExams { get; set; }

    public DbSet<Lecture> Lectures { get; set; }

    // Course and Student System
    public DbSet<Course> Courses { get; set; }
    public DbSet<Student> Students { get; set; }
    public DbSet<StudentCourse> StudentCourses { get; set; }

    public DbSet<Teacher> Teachers { get; set; }
    public DbSet<TeacherCourse> TeacherCourses { get; set; }

    #region Entities from the modules
    //Identity
    public DbSet<IdentityUser> Users { get; set; }
    public DbSet<IdentityRole> Roles { get; set; }
    public DbSet<IdentityClaimType> ClaimTypes { get; set; }
    public DbSet<OrganizationUnit> OrganizationUnits { get; set; }
    public DbSet<IdentitySecurityLog> SecurityLogs { get; set; }
    public DbSet<IdentityLinkUser> LinkUsers { get; set; }
    public DbSet<IdentityUserDelegation> UserDelegations { get; set; }
    public DbSet<IdentitySession> Sessions { get; set; }

    // Tenant Management
    public DbSet<Tenant> Tenants { get; set; }
    public DbSet<TenantConnectionString> TenantConnectionStrings { get; set; }
    #endregion

    public ElearningSystemDbContext(DbContextOptions<ElearningSystemDbContext> options)
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        /* Include modules to your migration db context */
        builder.ConfigurePermissionManagement();
        builder.ConfigureSettingManagement();
        builder.ConfigureBackgroundJobs();
        builder.ConfigureAuditLogging();
        builder.ConfigureIdentity();
        builder.ConfigureOpenIddict();
        builder.ConfigureFeatureManagement();
        builder.ConfigureTenantManagement();

        /* Configure your own tables/entities inside here */

        // ============================================
        // EXAM AND EXAMQUESTION (UPDATED)
        // ============================================

        // Exam → ExamQuestion (One to Many)
        builder.Entity<Exam>(b =>
        {
            b.HasMany(e => e.ExamQuestions)
             .WithOne(eq => eq.Exam)
             .HasForeignKey(eq => eq.ExamId)
             .OnDelete(DeleteBehavior.Cascade);
        });

        // ExamQuestion Composite Key
        builder.Entity<ExamQuestion>(b =>
        {
            b.HasKey(eq => new { eq.ExamId, eq.QuestionId });
        });
        /////////////////////////
        ////STUDENTEXAM
        /////////////////////////
        builder.Entity<Student>(b => 
        { 
        b.HasMany(s=> s.StudentExams)
            .WithOne(se => se.Student)
            .HasForeignKey(se=>se.StudentId)
            .OnDelete(DeleteBehavior.Cascade);
        });

        builder.Entity<Exam>(b =>
            {
                b.HasMany(e => e.StudentExams)
                .WithOne(se => se.Exam)
                .HasForeignKey(se => se.ExamId)
                .OnDelete(DeleteBehavior.Cascade);

        });
        // StudentExam Composite Key
        builder.Entity<StudentExam>(b =>
            {
                b.HasKey(se => new { se.StudentId, se.ExamId });
        });

        // ============================================
        // COURSE AND QUESTION (UPDATED)
        // ============================================

        // Course → Question (One to Many) - CHANGED
        builder.Entity<Course>(b =>
        {
            b.HasMany(c => c.Questions)
             .WithOne(q => q.Course)
             .HasForeignKey(q => q.CourseId)
             .OnDelete(DeleteBehavior.NoAction);
        });

        // Question → Answer (One to Many)
        builder.Entity<Question>(b =>
        {
            b.HasMany(q => q.Answers)
             .WithOne(a => a.Question)
             .HasForeignKey(a => a.QuestionId)
             .OnDelete(DeleteBehavior.Cascade);
        });

        // ============================================
        // COURSE, EXAM, STUDENT (UNCHANGED)
        // ============================================

        // Course → Exams (One to Many)
        builder.Entity<Course>(b =>
        {
            b.HasMany(c => c.Exams)
             .WithOne(e => e.Course)
             .HasForeignKey(e => e.CourseId)
             .OnDelete(DeleteBehavior.Cascade);
        });

        // Course → StudentCourses (One to Many)
        builder.Entity<Course>(b =>
        {
            b.HasMany(c => c.StudentCourses)
             .WithOne(sc => sc.Course)
             .HasForeignKey(sc => sc.CourseId)
             .OnDelete(DeleteBehavior.Cascade);
        });

        // Student → StudentCourses (One to Many)
        builder.Entity<Student>(b =>
        {
            b.HasMany(s => s.StudentCourses)
             .WithOne(sc => sc.Student)
             .HasForeignKey(sc => sc.StudentId)
             .OnDelete(DeleteBehavior.Cascade);
        });

        // StudentCourse Composite Key
        builder.Entity<StudentCourse>(b =>
        {
            b.HasKey(sc => new { sc.StudentId, sc.CourseId });
        });
    }
}