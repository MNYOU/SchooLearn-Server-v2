using AutoMapper;
using Dal.Entities;
using Logic.ApiModels;
using Logic.Models;
using Task = System.Threading.Tasks.Task;

namespace Logic.AutoMapper;

public class AutoMapperProfile: Profile
{
    public AutoMapperProfile()
    {
        CreateMap<InstitutionApiRequest, Institution>().ReverseMap();
        CreateMap<TaskApiModel, Task>();
        CreateMap<GroupApiModel, Group>().ReverseMap();
        CreateMap<User, AuthorizedApiModel>();
        // CreateMap<RegistrationApiModel, RegistrationModel>();
        CreateMap<RegistrationApiModel, User>();
            
        CreateMap<LoginApiModel, LoginModel>();
        CreateMap<TaskApiModel, Task>();
        // наверное нужно разделить task модели на респонсе и реквест
        // CreateMap<TaskCustomApiModel, TaskWithDetailedAnswer>();
        CreateMap<TaskApiModel, Task>();
        
        CreateMap<InstitutionApiRequest, Institution>();
        CreateMap<Institution, InstitutionApiModel>();

        CreateMap<Subject, SubjectApiModel>();
    }
}