using Dal.Entities;
using Dal.Repositories;
using Logic.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Logic.Implementations;

public class ProjectManager: IProjectManager
{
    private readonly IApplicationRepository _repository;
    
    public ProjectManager(IApplicationRepository repository)
    {
        _repository = repository;
    }

    public bool DoSomething()
    {
        throw new NotImplementedException();
    }

    public async Task<Application> CreateApplication(Institution institution)
    {
        var application = new Application { Institution = institution, IsReviewed = false, ApplicationResult = false};
        await _repository.Applications.AddAsync(application);
        await _repository.SaveChangesAsync();
        return application;
    }

    public IEnumerable<Application> GetNotReviewedApplications()
    {
        return _repository.Applications
            .Where(a => !a.IsReviewed)
            .AsEnumerable();
    }

    public bool ConsiderApplication(long applicationId, bool isConfirmed)
    {
        var application = _repository.Applications
            .Include(a => a.Institution)
            .FirstOrDefault(a => a.Id == applicationId);
        if (application == null || application.IsReviewed)
            return false;
        
        // где-то должна быть генерация кодов
        application.Institution.IsConfirmed = true;
        application.IsReviewed = isConfirmed;
        application.ApplicationResult = isConfirmed;
        return true;
    }
}