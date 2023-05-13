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
    private readonly IAccountManager _accountManager;

    public StudentManager(IStudentRepository repository, IGroupRepository groupRepository, IMapper mapper,
        IAccountManager accountManager)
    {
        _repository = repository;
        _groupRepository = groupRepository;
        _mapper = mapper;
        _accountManager = accountManager;
    }

    public async Task<bool> Register(long userId)
    {
        var user = await _accountManager.GetAsync(userId);
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

    public IEnumerable<StudentApiModel>? SearchByNickname(long userId, long institutionId, string nickname)
    {
        var user = _accountManager.Get(userId);
        var hasAccess = UserHelper.HasAccessToUsers(user, institutionId, Role.Student);
        if (!hasAccess) return null;
        return _repository.Students
            .Where(s => s.InstitutionId == institutionId && UserHelper.IsPartiallyEqual(nickname, s.User.Nickname))
            .OrderBy(s => s.User.Nickname)
            .Include(s => s.User)
            .Select(s => _mapper.Map<StudentApiModel>(s));
    }

    public IEnumerable<StudentApiModel>? GetAllByInstitution(long userId, long institutionId)
    {
        var user = _accountManager.Get(userId);
        var hasAccess = UserHelper.HasAccessToUsers(user, institutionId, Role.Student);
        if (!hasAccess) return null;
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


    public async Task<bool> CreateApplicationToGroup(long studentId, long groupId, long invitationCode)
    {
        var student = await _repository.Students
            .FirstOrDefaultAsync(s => s.UserId == studentId);
        var group = await _groupRepository.Groups
            .Include(g => g.Teacher)
            .FirstOrDefaultAsync(g => g.Id == groupId && g.InvitationCode == invitationCode);
        if (student is null || group is null)
            return false;
        if (student.InstitutionId != null && student.InstitutionId != group.Teacher.InstitutionId)
            return false;
        var groupStudent = new GroupStudent { Student = student, Group = group, IsApproved = false };
        await _groupRepository.GroupStudents.AddAsync(groupStudent);
        await _groupRepository.SaveChangesAsync();
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