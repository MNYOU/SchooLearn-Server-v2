using AutoMapper;
using Dal.Entities;
using Logic.ApiModels;
using Logic.Models;
using Org.BouncyCastle.Crypto.Modes;
using Task = Dal.Entities.Task;

namespace Logic.AutoMapper;

public class AutoMapperProfile: Profile
{
    public AutoMapperProfile()
    {
        CreateMap<InstitutionApiRequest, Institution>().ReverseMap();
        // CreateMap<TaskApiModel, Task>();
        CreateMap<GroupApiModel, Group>().ReverseMap();
        CreateMap<Student, StudentApiModel>()
            .ForMember(d => d.Id, opt => opt.MapFrom(s => s.UserId))
            .ForMember(d => d.Nickname, opt => opt.MapFrom(s => s.User.Nickname));
        CreateMap<User, AuthorizedApiModel>();
        // CreateMap<RegistrationApiModel, RegistrationModel>();
        CreateMap<RegistrationApiModel, User>();
        CreateMap<Task, TaskPreviewApiModel>();
        CreateMap<Task, TaskResponseModel>()
            // .AfterMap((s, d) => )
            // .ForMember(t => t.Difficulty, opt => opt.MapFrom(d => d.Difficulty))
            .ForPath(t => t.Difficulty.Tasks, opt => opt.Ignore());
            
        CreateMap<LoginApiModel, LoginModel>();
        // CreateMap<TaskApiModel, Task>();
        // наверное нужно разделить task модели на респонсе и реквест
        // CreateMap<TaskCustomApiModel, TaskWithDetailedAnswer>();
        // CreateMap<TaskApiModel, Task>();
        
        CreateMap<InstitutionApiRequest, Institution>();
        CreateMap<Institution, InstitutionApiModel>();

        CreateMap<Subject, SubjectApiModel>().ReverseMap();
        CreateMap<Teacher, TeacherApiModel>();
    }
}