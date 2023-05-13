using Dal.Entities;

namespace Logic.Interfaces;

public interface IProjectManager
{
    Task<Application> CreateApplication(Institution institution);
    
    IEnumerable<Application> GetNotReviewedApplications();
    
    bool ConsiderApplication(long applicationId, bool isConfirmed);
}