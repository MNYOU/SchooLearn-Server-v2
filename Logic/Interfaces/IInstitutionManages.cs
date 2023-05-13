using System.Collections;
using Dal.Entities;
using Logic.ApiModels;

namespace Logic.Interfaces;

public interface IInstitutionManager
{
    Institution? Get(long id);
    
    Task<Application?> CreateApplication(InstitutionApiModel model);

    IEnumerable<InstitutionApiModel> GetAllConfirmed();

    IEnumerable<long> GetPrimaryInvitationCodes();

    IEnumerable<long> GetOccupiedInvitationCodes();

    bool GeneratePrimaryInvitationCode(long institutionId);

    Institution? GetByPrimaryInvitationCode(long code);
    
    Institution? GetByInvitationCode(long code);
}