using Dal.Entities;
using Logic.ApiModels;

namespace Logic.Interfaces;

public interface IInstitutionManager
{
    Task<Application?> CreateApplication(InstitutionApiModel model);

    IEnumerable<Application> GetNotReviewedApplications();

    bool ConsiderApplication(long applicationId, bool isConfirmed);

    IEnumerable<InstitutionApiModel> GetAllConfirmed();

    Institution? GetByPrimaryInvitationCode(long code);
    
    Institution? GetByInvitationCode(long code);
}