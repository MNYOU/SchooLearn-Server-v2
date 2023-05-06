using AutoMapper;
using Dal.Entities;
using Dal.Enums;
using Dal.Repositories;
using Logic.ApiModels;
using Logic.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Logic.Implementations;

public class StudentManager : IStudentManager
{
    private readonly IMapper _mapper;
    private readonly IUserRepository _userRepository;
    private readonly IStudentRepository _repository;
    private readonly IGroupRepository _groupRepository;
    private readonly IInstitutionManager _institutionManager;

    public StudentManager(IUserRepository userRepository, IStudentRepository repository,
        IInstitutionManager institutionManager, IGroupRepository groupRepository, IMapper mapper)
    {
        _userRepository = userRepository;
        _repository = repository;
        _institutionManager = institutionManager;
        _groupRepository = groupRepository;
        _mapper = mapper;
    }

    public async Task<bool> Register(User user)
    {
        if (user.Role != Role.Student || user.Id == 0 || !_userRepository.Users.Contains(user) ||
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
            //.ThenInclude(g => g.Tasks)
            //.ThenInclude(g => g.Difficulty)
            .Include(s => s.SolvedTasks)
            .FirstOrDefault(s => s.UserId == id);
    }

    public async Task<Student?> GetWithDetailsAsync(long id)
    {
        return await _repository.Students
            .Include(s => s.User)
            .Include(s => s.Institution)
            .Include(s => s.GroupsStudent)
            .Include(s => s.SolvedTasks)
            .FirstOrDefaultAsync(s => s.UserId == id);
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
            .FirstOrDefaultAsync(g => g.Id == groupId && g.InvitationCode == invitationCode);
        if (student != null && group != null)
        {
            var groupStudent = new GroupStudent { Student = student, Group = group, IsApproved = false };
            await _groupRepository.GroupStudents.AddAsync(groupStudent);
            await _groupRepository.SaveChangesAsync();
            return true;
        }

        return false;
    }

    public async Task<bool> LeaveGroupAsync(long studentId, long groupId)
    {
        var groupStudent = await _groupRepository.GroupStudents
            .FirstOrDefaultAsync(gs => gs.StudentId == studentId && gs.GroupId == gs.GroupId);
        if (groupStudent != null)
        {
            _groupRepository.GroupStudents.Remove(groupStudent);
            await _groupRepository.SaveChangesAsync();
            return true;
        }

        return false;
    }
}