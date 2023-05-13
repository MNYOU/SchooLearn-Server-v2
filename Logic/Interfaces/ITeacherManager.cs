using Dal.Entities;
using Logic.ApiModels;

namespace Logic.Interfaces;

public interface ITeacherManager
{
    Task<bool> Register(long userId, long invitationCode);

    Teacher? Get(long id);
    
    Task<Teacher?> GetAsync(long id);
    
    Teacher? GetWithDetails(long id);

    Task<Teacher?> GetWithDetailsAsync(long id);
    
    IEnumerable<Group> GetMyGroups(long teacherId);
    
    IEnumerable<GroupApiModel> GetMyGroupsApiModels(long teacherId);

    Task<bool> CreateGroup(long teacherId, GroupApiModel model);

    Task<bool> DisbandGroup(long teacherId, long groupId);

    IEnumerable<StudentApiModel> GetStudentsFromGroup(long teacherId, long groupId);

    IEnumerable<StudentApiModel> GetStudentApplications(long teacherId, long groupId);

    bool ConsiderApplication(long teacherId, long groupId, long studentId, bool isApproved);

    Task<long?> GetGroupInvitationCodeAsync(long teacherId, long groupId);

    bool GenerateNewInvitationCode(long teacherId, long groupId);

    IEnumerable<SubjectApiModel> GetMySubjects(long teacherId);

    Task<bool> AddSubject(long teacherId, SubjectApiModel model);
}