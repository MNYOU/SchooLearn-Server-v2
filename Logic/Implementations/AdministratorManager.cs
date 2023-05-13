using Dal.Entities;
using Dal.Enums;
using Dal.Repositories;
using Logic.Helpers;
using Logic.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Logic.Implementations;

public class AdministratorManager : IAdministratorManager
{
    private readonly IAdministratorRepository _repository;
    private readonly IAccountManager _accountManager;
    private readonly IInstitutionManager _institutionManager;

    public AdministratorManager(IAdministratorRepository repository, IInstitutionManager institutionManager,
        IAccountManager accountManager)
    {
        _repository = repository;
        _institutionManager = institutionManager;
        _accountManager = accountManager;
    }

    public Admin? Get(long id)
    {
        return _repository.Admins
            .Include(a => a.User)
            .FirstOrDefault(a => a.UserId == id);
    }

    public Admin? GetWithDetails(long id)
    {
        return _repository.Admins
            .Include(a => a.User)
            .Include(a => a.Institution)
            .FirstOrDefault(a => a.UserId == id);
    }

    public async Task<bool> Register(long userId, long invitationCode)
    {
        var user = await _accountManager.GetAsync(userId);
        if (user is null || user.Role != Role.Admin ||
            _repository.Admins.Any(a => a.UserId == user.Id))
            return false;
        var institution = _institutionManager.GetByPrimaryInvitationCode(invitationCode);
        if (institution is null || institution.AdminId != null)
            return false;
        user.Institution = institution;
        var admin = new Admin
        {
            User = user,
            Institution = institution
        };
        await _repository.Admins.AddAsync(admin);
        await _repository.SaveChangesAsync();
        GenerateNewInvitationCode(admin.UserId);
        return true;
    }

    public long? GetInvitationCode(long adminId)
    {
        var admin = _repository.Admins
            .Include(a => a.User)
            .Include(a => a.Institution)
            .FirstOrDefault(a => a.UserId == adminId);
        return admin?.Institution.InvitationCodeForTeachers;
    }

    public bool GenerateNewInvitationCode(long adminId)
    {
        var admin = _repository.Admins
            .Include(a => a.Institution)
            .FirstOrDefault(a => a.UserId == adminId);
        var institution = admin?.Institution;
        if (institution is null) return false;
        var codes = _institutionManager
            .GetOccupiedInvitationCodes();
        var newCode = Generator.GenerateInvitationCode(codes);
        institution.InvitationCodeForTeachers = newCode;
        _repository.SaveChanges();
        return true;
    }
}