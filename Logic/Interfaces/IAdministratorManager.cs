using Dal.Entities;

namespace Logic.Interfaces;

public interface IAdministratorManager
{
    Admin? Get(long id);

    Admin? GetWithDetails(long id);

    Task<bool> Register(long userId, string invitationCode, IAccountManager accountManager);

    string? GetInvitationCode(long adminId);
    
    bool GenerateNewInvitationCode(long adminId);
}