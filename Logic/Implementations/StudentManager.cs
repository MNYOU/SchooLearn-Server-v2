using AutoMapper;
using Dal.Entities;
using Dal.Enums;
using Dal.Repositories;
using Logic.ApiModels;
using Logic.Helpers;
using Logic.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Logic.Implementations;

public class StudentManager : IStudentManager
{
    private readonly IMapper _mapper;
    private readonly IStudentRepository _repository;
    private readonly IGroupRepository _groupRepository;

    public StudentManager(IStudentRepository repository, IGroupRepository groupRepository, IMapper mapper)
    {
        _repository = repository;
        _groupRepository = groupRepository;
        _mapper = mapper;
    }

    public async Task<bool> Register(long userId, IAccountManager accountManager)
    {
        var user = await accountManager.GetAsync(userId);
        if (user is null || user.Role != Role.Student || user.Id == 0 ||
            _repository.Students.Any(s => s.UserId == user.Id))
            return false;

        var student = new Student { UserId = user.Id };
        await _repository.Students.AddAsync(student);
        await _repository.SaveChangesAsync();
        return true;
    }

    public Student? Get(long id)
    {
        return _repository.Students
            .Include(s => s.User)
            .FirstOrDefault(s => s.UserId == id);
    }

    public async Task<Student?> GetAsync(long id)
    {
        return await _repository.Students
            .Include(s => s.User)
            .FirstOrDefaultAsync(s => s.UserId == id);
    }

    public Student? GetWithDetails(long id)
    {
        return _repository.Students
            .Include(s => s.User)
            .Include(s => s.Institution)
            .Include(s => s.GroupsStudent)
            // .ThenInclude(gs => gs.Group)
            // .ThenInclude(g => g.Tasks)
            .Include(s => s.SolvedTasks)
            .FirstOrDefault(s => s.UserId == id);
    }

    public async Task<Student?> GetWithDetailsAsync(long id)
    {
        return await _repository.Students
            .Include(s => s.User)
            .Include(s => s.Institution)
            .Include(s => s.GroupsStudent)
            // .ThenInclude(gs => gs.Group)
            // .ThenInclude(g => g.Tasks)
            .Include(s => s.SolvedTasks)
            .FirstOrDefaultAsync(s => s.UserId == id);
    }

    public IEnumerable<StudentApiModel>? SearchByNickname(long userId, long? institutionId, string nickname,
        IAccountManager accountManager)
    {
        var user = accountManager.Get(userId);
        if (user is null || !UserHelper.HasAccessToUsers(user, institutionId, Role.Student))
            return null;
        return _repository.Students
            .Where(s => institutionId == null || s.InstitutionId == institutionId &&
                UserHelper.IsPartiallyEqual(nickname, s.User.Nickname))
            .OrderBy(s => s.User.Nickname)
            .Include(s => s.User)
            .Select(s => _mapper.Map<StudentApiModel>(s));
    }

    public IEnumerable<StudentApiModel>? GetAllByInstitution(long userId, long institutionId,
        IAccountManager accountManager)
    {
        var user = accountManager.Get(userId);
        if (user is null || !UserHelper.HasAccessToUsers(user, institutionId, Role.Student))
            return null;
        return _repository.Students
            .Where(s => s.InstitutionId == institutionId)
            .OrderBy(s => s.User.Nickname)
            .Include(s => s.User)
            .Select(s => _mapper.Map<StudentApiModel>(s));
    }

    public IEnumerable<GroupApiModel> GetMyGroups(long studentId)
    {
        return _groupRepository.GroupStudents
            .Include(gs => gs.Group)
            .Where(gs => gs.StudentId == studentId && gs.IsApproved)
            .Select(gs => _mapper.Map<GroupApiModel>(gs.Group));
    }

    public string? GetTeacherName(long studentId, long groupId)
    {
        var group = _groupRepository.Groups
            .Include(g => g.Teacher)
            .ThenInclude(t => t.User)
            .FirstOrDefault(g =>
                g.Id == groupId && g.GroupsStudent.Any(gs => gs.IsApproved && gs.StudentId == studentId));
        return group?.Teacher.User.Nickname;
    }


    public async Task<bool> CreateApplicationToGroup(long studentId, string invitationCode, ITeacherManager teacherManager)
    {
        var student = await _repository.Students
            .FirstOrDefaultAsync(s => s.UserId == studentId);
        var group = await _groupRepository.Groups
            .Include(g => g.Teacher)
            .FirstOrDefaultAsync(g => g.InvitationCode == invitationCode);
        if (student is null || group is null)
            return false;
        if (student.InstitutionId != null && student.InstitutionId != group.Teacher.InstitutionId ||
            _groupRepository.GroupStudents.Any(gs => gs.StudentId == studentId && gs.GroupId == group.Id))
            return false;
        var groupStudent = new GroupStudent { StudentId = studentId, GroupId = group.Id, IsApproved = false };
        await _groupRepository.GroupStudents.AddAsync(groupStudent);
        await _groupRepository.SaveChangesAsync();
                
        // TODO заявки сразу утверждаются, то есть не требуют подтверждения
        teacherManager.ConsiderApplication(group.TeacherId, group.Id, studentId, true);
        return true;
    }

    public async Task<bool> LeaveGroupAsync(long studentId, long groupId)
    {
        var groupStudent = await _groupRepository.GroupStudents
            .FirstOrDefaultAsync(gs => gs.StudentId == studentId && gs.GroupId == groupId);
        if (groupStudent != null)
        {
            _groupRepository.GroupStudents.Remove(groupStudent);
            await _groupRepository.SaveChangesAsync();
            return true;
        }

        return false;
    }
}