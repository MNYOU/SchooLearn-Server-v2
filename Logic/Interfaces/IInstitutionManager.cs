using System.Collections;
using Dal.Entities;
using Logic.ApiModels;

namespace Logic.Interfaces;

public interface IInstitutionManager
{
    Institution? Get(long id);
    
    Task<Application?> CreateApplication(InstitutionApiRequest request, IProjectManager projectManager);

    bool AddAdmin(Admin admin);

    IEnumerable<InstitutionApiModel> GetAllConfirmed();

    IEnumerable<string> GetPrimaryInvitationCodes();

    IEnumerable<string> GetOccupiedInvitationCodes();

    bool GeneratePrimaryInvitationCode(long institutionId);

    Institution? GetByPrimaryInvitationCode(string code);
    
    Institution? GetByInvitationCode(string code);
}