using System.Runtime.InteropServices.JavaScript;
using Dal.Entities;
using Logic.ApiModels;

namespace Logic.Interfaces;

public interface IStudentManager
{
    Task<bool> Register(User user);

    Student? Get(long id);

    Task<Student?> GetAsync(long id);

    Student? GetWithDetails(long id);
    
    Task<Student?> GetWithDetailsAsync(long id);

    IEnumerable<GroupApiModel> GetMyGroups(long studentId);

    Task<bool> CreateApplicationToGroup(long studentId, long groupId, long invitationCode);

    Task<bool> LeaveGroupAsync(long studentId, long groupId);
}