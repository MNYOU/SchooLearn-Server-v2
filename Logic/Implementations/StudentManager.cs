using Dal.Entities;
using Dal.Enums;
using Dal.Repositories;
using Logic.ApiModels;
using Logic.Interfaces;

namespace Logic.Implementations;

public class StudentManager: IStudentManager
{
    private readonly IUserRepository _userRepository;
    private readonly IStudentRepository _repository;
    private readonly IInstitutionManager _manager;

    public StudentManager(IUserRepository userRepository, IStudentRepository repository, IInstitutionManager manager)
    {
        _userRepository = userRepository;
        _repository = repository;
        _manager = manager;
    }

    public async Task<bool> Register(User user)
    {
        if (user.Role != Role.Student || !_userRepository.Users.Contains(user) ||
            _repository.Students.Any(s => s.UserId == user.Id))
            return false;

        var student = new Student() { User = user };
        await _repository.Students.AddAsync(student);
        await _repository.SaveChangesAsync();
        return true;
    }

    public bool JoinInGroup(long key)
    {
        throw new NotImplementedException();
    }

    public IEnumerable<TaskApiModel> GetMyTasks()
    {
        throw new NotImplementedException();
    }

    public IEnumerable<TaskCustomApiModel> GetTaskWithAnswer()
    {
        throw new NotImplementedException();
    }
}