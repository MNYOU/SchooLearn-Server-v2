using Dal.Entities;

namespace Logic.Interfaces;

public interface IAdministratorManager
{
    // TODO добавить генерацию кодов приглашения
    
    Task<bool> Register(User user, long invitationCode);

    long? GetInvitationCode(long adminId);
    
    bool GenerateNewInvitationCode(long adminId);
}