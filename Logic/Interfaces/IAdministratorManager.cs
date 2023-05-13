using Dal.Entities;

namespace Logic.Interfaces;

public interface IAdministratorManager
{
    Admin? Get(long id);

    Admin? GetWithDetails(long id);

    Task<bool> Register(long userId, long invitationCode);

    long? GetInvitationCode(long adminId);
    
    bool GenerateNewInvitationCode(long adminId);
}