using Dal.Entities;
using Logic.ApiModels;

namespace Logic.Interfaces;

public interface IStudentManager
{
    Task<bool> Register(long userId);

    Student? Get(long id);

    Task<Student?> GetAsync(long id);

    Student? GetWithDetails(long id);
    
    Task<Student?> GetWithDetailsAsync(long id);

    IEnumerable<StudentApiModel>? SearchByNickname(long userId, long institutionId, string nickname);

    IEnumerable<StudentApiModel>? GetAllByInstitution(long userId, long institutionId);

    IEnumerable<GroupApiModel> GetMyGroups(long studentId);

    Task<bool> CreateApplicationToGroup(long studentId, long groupId, long invitationCode);

    Task<bool> LeaveGroupAsync(long studentId, long groupId);
}