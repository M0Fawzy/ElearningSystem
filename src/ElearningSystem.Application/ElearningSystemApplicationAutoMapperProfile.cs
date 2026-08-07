using AutoMapper;
using ElearningSystem.Entities;

namespace ElearningSystem;

public class ElearningSystemApplicationAutoMapperProfile : Profile
{
    public ElearningSystemApplicationAutoMapperProfile()
    {
        // ===== Exam =====
        CreateMap<Exam, ExamDto>();
        CreateMap<CreateExamDto, Exam>();

        // ===== Question =====
        CreateMap<Question, QuestionDto>();
        CreateMap<CreateQuestionDto, Question>();

        // ===== Answer =====
        CreateMap<Answer, AnswerDto>();
        CreateMap<CreateAnswerDto, Answer>();

        // ===== Course =====
        CreateMap<Course, CourseDto>();
        CreateMap<CreateCourseDto, Course>();

        // ===== Student =====
        CreateMap<Student, StudentDto>();

        CreateMap<CreateStudentDto, Student>();


        // ===== Teacher =====
        CreateMap<Teacher, TeacherDto>();

        CreateMap<CreateTeacherDto, Teacher>();

        // ===== StudentCourse =====
        CreateMap<StudentCourse, StudentCourseDto>();
        CreateMap<EnrollStudentDto, StudentCourse>();

        // ===== TeacherCourse =====
        CreateMap<TeacherCourse, TeacherCourseDto>();
        CreateMap<EnrollTeacherDto, TeacherCourse>();

        // ===== ExamQuestion =====
        CreateMap<ExamQuestion, ExamQuestionDto>();
        CreateMap<CreateExamQuestionDto, ExamQuestion>();

        // ===== StudentExam =====
        CreateMap<StudentExam, StudentExamDto>();
        CreateMap<CreateStudentExamDto, StudentExam>();

        // ===== Lecture =====
        CreateMap<Lecture, LectureDto>();
    }
}
