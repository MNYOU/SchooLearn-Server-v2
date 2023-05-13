using AutoMapper;
using Dal.Entities;
using Dal.Enums;
using Dal.Repositories;
using Logic.ApiModels;
using Logic.Helpers;
using Logic.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Logic.Implementations;

public class TeacherManager : ITeacherManager
{
    private readonly IMapper _mapper;
    private readonly ITeacherRepository _repository;
    private readonly ISubjectRepository _subjectRepository;
    private readonly IGroupRepository _groupRepository;
    private readonly IAccountManager _accountManager;
    private readonly IInstitutionManager _institutionManager;

    public TeacherManager(IMapper mapper, ITeacherRepository repository, IInstitutionManager institutionManager,
        ISubjectRepository subjectRepository, IGroupRepository groupRepository, IAccountManager accountManager)
    {
        _mapper = mapper;
        _repository = repository;
        _institutionManager = institutionManager;
        _subjectRepository = subjectRepository;
        _groupRepository = groupRepository;
        _accountManager = accountManager;
    }

    public async Task<bool> Register(long userId, long invitationCode)
    {
        var user = await _accountManager.GetAsync(userId);
        if (user is null || user.Role != Role.Teacher || _repository.Teachers.Any(t => t.UserId == user.Id))
            return false;

        var institution = _institutionManager.GetByInvitationCode(invitationCode);
        if (institution is null) return false;

        user.Institution = institution;
        var teacher = new Teacher { User = user, Institution = institution };
        await _repository.Teachers.AddAsync(teacher);
        await _repository.SaveChangesAsync();
        return true;
    }

    public Teacher? Get(long id)
    {
        return _repository.Teachers
            .Include(t => t.User)
            .FirstOrDefault(t => t.UserId == id);
    }

    public async Task<Teacher?> GetAsync(long id)
    {
        return await _repository.Teachers
            .Include(t => t.User)
            .FirstOrDefaultAsync(t => t.UserId == id);
    }

    public Teacher? GetWithDetails(long id)
    {
        return _repository.Teachers
            .Include(t => t.User)
            .Include(t => t.Institution)
            .Include(t => t.Subjects)
            .Include(t => t.Groups)
            .FirstOrDefault(t => t.UserId == id);
    }

    public async Task<Teacher?> GetWithDetailsAsync(long id)
    {
        return await _repository.Teachers
            .Include(t => t.User)
            .Include(t => t.Institution)
            .Include(t => t.Subjects)
            .Include(t => t.Groups)
            .FirstOrDefaultAsync(t => t.UserId == id);
    }

    public IEnumerable<Group> GetMyGroups(long teacherId)
    {
        return _groupRepository.Groups
            .Where(g => g.TeacherId == teacherId);
    }

    public IEnumerable<GroupApiModel> GetMyGroupsApiModels(long teacherId)
    {
        return GetMyGroups(teacherId)
            .Select(g => _mapper.Map<GroupApiModel>(g));
    }

    public async Task<bool> CreateGroup(long teacherId, GroupApiModel model)
    {
        var teacher = await GetAsync(teacherId);
        if (teacher is null) return false;
        var group = _mapper.Map<Group>(model);
        teacher.Groups.Add(group);
        _repository.Teachers.Update(teacher);
        return true;
    }


    public async Task<bool> DisbandGroup(long teacherId, long groupId)
    {
        var teacher = await _repository.Teachers
            .Include(t => t.Groups)
            .FirstOrDefaultAsync(t => t.UserId == teacherId);
        var group = teacher?.Groups
            .FirstOrDefault(g => g.Id == groupId);
        if (teacher != null && group != null)
        {
            teacher.Groups.Remove(group);
            await _repository.SaveChangesAsync();
            return true;
        }

        return false;
    }

    private bool TeacherIsCreatorOfGroup(long teacherId, long groupId)
    {
        return _groupRepository.Groups
            .Any(g => g.Id == groupId && g.TeacherId == teacherId);
    }

    private Group? GetGroup(long teacherId, long groupId)
    {
        return _groupRepository.Groups
            .Include(g => g.GroupsStudent)
            .ThenInclude(gs => gs.Student)
            .FirstOrDefault(g => g.Id == groupId && g.TeacherId == teacherId);
    }

    public IEnumerable<StudentApiModel> GetStudentsFromGroup(long teacherId, long groupId)
    {
        var group = GetGroup(teacherId, groupId);
        if (group is null) return Array.Empty<StudentApiModel>();
        return group
            .GroupsStudent
            .Where(gs => gs.IsApproved)
            .Select(gs => _mapper.Map<StudentApiModel>(gs.Student));
    }

    public IEnumerable<StudentApiModel> GetStudentApplications(long teacherId, long groupId)
    {
        var group = GetGroup(teacherId, groupId);
        if (group is null) return Array.Empty<StudentApiModel>();
        return group
            .GroupsStudent
            .Where(gs => gs.IsApproved)
            .Select(gs => _mapper.Map<StudentApiModel>(gs.Student));
    }

    public bool ConsiderApplication(long teacherId, long groupId, long studentId, bool isApproved)
    {
        if (!TeacherIsCreatorOfGroup(teacherId, groupId)) return false;
        var groupStudents = _groupRepository.GroupStudents
            .Include(gs => gs.Group)
            .ThenInclude(gs => gs.Teacher)
            .ThenInclude(t => t.Institution)
            .Include(gs => gs.Student)
            .ThenInclude(s => s.User)
            .FirstOrDefault(gs => gs.GroupId == groupId && gs.StudentId == studentId);
        if (groupStudents is null) return false;
        if (isApproved)
        {
            var student = groupStudents.Student;
            var institution = groupStudents.Group.Teacher.Institution;
            groupStudents.IsApproved = isApproved;
            if (student.InstitutionId is null)
            {
                student.Institution = institution;
                student.IsConfirmed = true;
                student.User.Institution = institution;
            }
        }
        else
            _groupRepository.GroupStudents.Remove(groupStudents);

        _groupRepository.SaveChanges();
        return true;
    }

    public async Task<long?> GetGroupInvitationCodeAsync(long teacherId, long groupId)
    {
        var group = await _groupRepository.Groups
            .FirstOrDefaultAsync(g => g.Id == groupId && g.TeacherId == teacherId);
        return group?.InvitationCode;
    }

    public bool GenerateNewInvitationCode(long teacherId, long groupId)
    {
        var teacher = _repository.Teachers
            .Include(t => t.Groups)
            .FirstOrDefault(t => t.UserId == teacherId);
        var group = teacher?.Groups
            .FirstOrDefault(g => g.Id == groupId);
        if (group is null) return false;
        var codes = _groupRepository.Groups
            .Select(g => g.InvitationCode);
        var newCode = Generator.GenerateInvitationCode(codes);
        group.InvitationCode = newCode;
        _repository.SaveChanges();
        return true;
    }


    public IEnumerable<SubjectApiModel> GetMySubjects(long teacherId)
    {
        return _subjectRepository.Subjects
            .Where(s => s.TeacherId == teacherId)
            .Select(s => _mapper.Map<SubjectApiModel>(s));
    }

    public async Task<bool> AddSubject(long teacherId, SubjectApiModel model)
    {
        var teacher = await GetAsync(teacherId);
        if (teacher is null) return false;
        var subject = _mapper.Map<Subject>(model);
        teacher.Subjects.Add(subject);
        _repository.Teachers.Update(teacher);
        return true;
    }
}