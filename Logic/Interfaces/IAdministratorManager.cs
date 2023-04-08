using Dal.Entities;

namespace Logic.Interfaces;

public interface IAdministratorManager
{
    Task<bool> Register(User user, long invitationCode);

    long GetInvitationCode();
    
    bool GenerateNewInvitationCode();
}