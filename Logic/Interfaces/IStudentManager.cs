using Dal.Entities;
using Logic.ApiModels;

namespace Logic.Interfaces;

public interface IStudentManager
{
    Task<bool> Register(long userId, IAccountManager accountManager);

    Student? Get(long id);

    Task<Student?> GetAsync(long id);

    Student? GetWithDetails(long id);
    
    Task<Student?> GetWithDetailsAsync(long id);

    IEnumerable<StudentApiModel>? SearchByNickname(long userId, long? institutionId, string nickname, IAccountManager accountManager);

    IEnumerable<StudentApiModel>? GetAllByInstitution(long userId, long institutionId, IAccountManager accountManager);

    IEnumerable<GroupApiModel> GetMyGroups(long studentId, long? subjectId);

    string? GetTeacherName(long studentId, long groupId);

    Task<bool> CreateApplicationToGroup(long studentId, string invitationCode, ITeacherManager teacherManager);

    Task<bool> LeaveGroupAsync(long studentId, long groupId);
}