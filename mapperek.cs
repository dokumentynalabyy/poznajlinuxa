using System;
using System.Linq;
using AutoMapper;
using Microsoft.AspNetCore.Mvc.Rendering;
using SchoolRegister.Model.DataModels;
using SchoolRegister.ViewModels.VM;
namespace SchoolRegister.Services.Configuration.AutoMapperProfiles
{
    public class MainProfile : Profile
    {
        public MainProfile()
        {
            //AutoMapper maps
            CreateMap<Subject, SubjectVm>() // map from Subject(src) to SubjectVm(dst)
                                            // custom mapping: FirstName and LastName concat string to TeacherName
            .ForMember(dest => dest.TeacherName, x => x.MapFrom(src => $"{src.Teacher.FirstName} {src.Teacher.LastName}"))
            // custom mapping: IList<Group> to IList<GroupVm>
            .ForMember(dest => dest.Groups, x => x.MapFrom(src => src.SubjectGroups.Select(y => y.Group)));
            CreateMap<AddOrUpdateSubjectVm, Subject>();
            CreateMap<Group, GroupVm>()
            .ForMember(dest => dest.Students, x => x.MapFrom(src => src.Students))
            .ForMember(dest => dest.Subjects, x => x.MapFrom(src => src.SubjectGroups.Select(s => s.Subject)));
            CreateMap<SubjectVm, AddOrUpdateSubjectVm>();
            CreateMap<Student, StudentVm>()
            .ForMember(dest => dest.GroupName, x => x.MapFrom(src => src.Group.Name))
            .ForMember(dest => dest.ParentName,
            x => x.MapFrom(src => $"{src.Parent.FirstName} {src.Parent.LastName}"));
            //....... other maps.........
            CreateMap<Teacher, TeacherVm>();
            //....
            CreateMap<Grade, GradeVm>();
            CreateMap<GradeVm, GetGradesReportVm>();
            CreateMap<GradeVm, GradesReportVm>();
            CreateMap<Student, GradesReportVm>()
                .ForMember(dest => dest.StudentLastName, x => x.MapFrom(src => src.LastName))
                .ForMember(dest => dest.StudentFirstName, x => x.MapFrom(src => src.FirstName))
                .ForMember(dest => dest.GroupName, x => x.MapFrom(src => src.Group.Name))
                .ForMember(dest => dest.ParentName, x => x.MapFrom(src => $"{src.Parent.FirstName} {src.Parent.LastName}"))
                .ForMember(dest => dest.StudentGradesPerSubject, x => x.MapFrom(src => src.Grades
                    .GroupBy(g => g.Subject.Name)
                    .Select(g => new { SubjectName = g.Key, Grades = g.Select(gl => gl.GradeValue).ToList() })
                    .ToDictionary(x => x.SubjectName, x => x.Grades)));
            CreateMap<AddGradeToStudentVm, Grade>()
                .ForMember(dest => dest.DateOfIssue, x => x.MapFrom(src => DateTime.Now));
            //
            CreateMap<AddOrUpdateGroupVm, Group>();
            CreateMap<AddOrUpdateGroupVm, GroupVm>();
        }
    }
}
