using Dal.Entities;
using Logic.ApiModels;

namespace Logic.Interfaces;

public interface ITeacherManager
{
    Task<bool> Register(long userId, string invitationCode, IAccountManager accountManager);

    Teacher? Get(long id);
    
    Task<Teacher?> GetAsync(long id);
    
    Teacher? GetWithDetails(long id);

    Task<Teacher?> GetWithDetailsAsync(long id);
    
    IEnumerable<Group> GetMyGroups(long teacherId);
    
    IEnumerable<GroupApiModel> GetMyGroupsApiModels(long teacherId, long? subjectId);

    Task<bool> CreateGroup(long teacherId, long subjectId, GroupApiModel model);

    Task<bool> DisbandGroup(long teacherId, long groupId);

    IEnumerable<StudentApiModel> GetStudentsFromGroup(long teacherId, long groupId);

    IEnumerable<StudentApiModel> GetStudentApplications(long teacherId, long groupId);

    bool ConsiderApplication(long teacherId, long groupId, long studentId, bool isApproved);

    Task<string?> GetGroupInvitationCodeAsync(long teacherId, long groupId);

    bool GenerateNewInvitationCode(long teacherId, long groupId);

    [Obsolete]
    Task<bool> AddSubject(long teacherId, SubjectApiModel model);
}