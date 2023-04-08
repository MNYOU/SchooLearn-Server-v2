using Dal.Entities;
using Dal.Enums;
using Dal.Repositories;
using Logic.Interfaces;

namespace Logic.Implementations;

public class TeacherManager : ITeacherManager
{
    private readonly IUserRepository _userRepository;
    private readonly ITeacherRepository _repository;
    private readonly IInstitutionManager _institutionManager;

    public TeacherManager(IUserRepository userRepository, ITeacherRepository repository,
        IInstitutionManager institutionManager)
    {
        _userRepository = userRepository;
        _repository = repository;
        _institutionManager = institutionManager;
    }

    public async Task<bool> Register(User user, long invitationCode)
    {
        if (user.Role != Role.Teacher || !_userRepository.Users.Contains(user) ||
            _repository.Teachers.Any(t => t.UserId == user.Id))
            return false;
        
        var institution = _institutionManager.GetByInvitationCode(invitationCode);
        if (institution is null) return false;

        var teacher = new Teacher() { User = user, Institution = institution};
        await _repository.Teachers.AddAsync(teacher);
        await _repository.SaveChangesAsync();
        return true;
    }
}