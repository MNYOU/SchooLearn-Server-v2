using Dal.Entities;

namespace Logic.Interfaces;

public interface IProjectManager
{
    bool DoSomething();
    
    Task<Application> CreateApplication(Institution institution);
    
    IEnumerable<Application> GetNotReviewedApplications();
    
    bool ConsiderApplication(long applicationId, bool isConfirmed);
}