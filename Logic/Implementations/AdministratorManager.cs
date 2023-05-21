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
    private readonly IInstitutionManager _institutionManager;

    public AdministratorManager(IAdministratorRepository repository, IInstitutionManager institutionManager)
    {
        _repository = repository;
        _institutionManager = institutionManager;
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

    public async Task<bool> Register(long userId, string invitationCode, IAccountManager accountManager)
    {
        var user = await accountManager.GetWithDetailsAsync(userId);
        if (user is null || user.Role != Role.Admin || _repository.Admins.Any(a => a.UserId == user.Id))
            return false;
        var institution = _institutionManager.GetByPrimaryInvitationCode(invitationCode);
        if (institution is null || institution.Admin != null)
            return false;
        user.InstitutionId = institution.Id;
        var admin = new Admin
        {
            UserId = user.Id,
            InstitutionId = institution.Id
        };
        try
        {
            accountManager.Update(user);
            await _repository.Admins.AddAsync(admin);
            await _repository.SaveChangesAsync();
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return false;
        }

        GenerateNewInvitationCode(admin.UserId);
        return true;
    }

    public string? GetInvitationCode(long adminId)
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