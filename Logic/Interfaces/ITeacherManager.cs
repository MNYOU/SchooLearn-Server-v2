using Dal.Entities;

namespace Logic.Interfaces;

public interface ITeacherManager
{
    Task<bool> Register(User user, long invitationCode);
    
}