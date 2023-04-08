using System.Xml.Schema;
using Dal.Entities;
using Dal.Enums;
using Dal.Repositories;
using Logic.Interfaces;

namespace Logic.Implementations;

public class AdministratorManager : IAdministratorManager
{
    private readonly IUserRepository _userRepository;
    private readonly IAdministratorRepository _repository;
    private readonly IInstitutionManager _institutionManager;

    public AdministratorManager(IUserRepository userRepository, IAdministratorRepository repository,
        IInstitutionManager institutionManager)
    {
        _userRepository = userRepository;
        _repository = repository;
        _institutionManager = institutionManager;
    }

    public async Task<bool> Register(User user, long invitationCode)
    {
        if (user.Role != Role.Admin || !_userRepository.Users.Contains(user) ||
            _repository.Admins.Any(a => a.UserId == user.Id))
            return false;
        var institution = _institutionManager.GetByPrimaryInvitationCode(invitationCode);
        if (institution is null) return false;

        // mapping
        var admin = new Admin()
        {
            User = user,
            Institution = institution
        };
        await _repository.Admins.AddAsync(admin);
        await _repository.SaveChangesAsync();
        return true;
    }

    public long GetInvitationCode()
    {
        throw new NotImplementedException();
    }

    public bool GenerateNewInvitationCode()
    {
        throw new NotImplementedException();
    }

    public long GetNewCode()
    {
        // переименовать на код для учителей
        // здесь должен проксироваться запрос к institutionManager
        throw new NotImplementedException();
    }
}