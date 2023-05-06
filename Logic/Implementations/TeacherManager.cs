using System.Data.SqlTypes;
using AutoMapper;
using Dal.Entities;
using Dal.Enums;
using Dal.Repositories;
using Logic.ApiModels;
using Logic.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace Logic.Implementations;

public class TeacherManager : ITeacherManager
{
    private readonly IMapper _mapper;
    private readonly IUserRepository _userRepository;
    private readonly ITeacherRepository _repository;
    private readonly IInstitutionManager _institutionManager;
    private readonly ISubjectRepository _subjectRepository;
    private readonly IGroupRepository _groupRepository;

    public TeacherManager(IMapper mapper, IUserRepository userRepository, ITeacherRepository repository,
        IInstitutionManager institutionManager, ISubjectRepository subjectRepository, IGroupRepository groupRepository)
    {
        _mapper = mapper;
        _userRepository = userRepository;
        _repository = repository;
        _institutionManager = institutionManager;
        _subjectRepository = subjectRepository;
        _groupRepository = groupRepository;
    }

    public async Task<bool> Register(User user, long invitationCode)
    {
        if (user.Role != Role.Teacher || !_userRepository.Users.Contains(user) ||
            _repository.Teachers.Any(t => t.UserId == user.Id))
            return false;

        var institution = _institutionManager.GetByInvitationCode(invitationCode);
        if (institution is null) return false;

        var teacher = new Teacher() { User = user, Institution = institution };
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
            .FirstOrDefault(gs => gs.GroupId == groupId && gs.StudentId == studentId);
        if (groupStudents is null) return false;
        if (isApproved)
            groupStudents.IsApproved = isApproved;
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
        throw new NotImplementedException();
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