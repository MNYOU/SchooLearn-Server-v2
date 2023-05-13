using Dal.Entities;
using Dal.Repositories;
using Logic.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Logic.Implementations;

public class ProjectManager: IProjectManager
{
    private readonly IApplicationRepository _repository;
    private readonly IInstitutionManager _institutionManager;
    
    public ProjectManager(IApplicationRepository repository, IInstitutionManager institutionManager)
    {
        _repository = repository;
        _institutionManager = institutionManager;
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
        // TODO отправка письма на почту
        var application = _repository.Applications
            .Include(a => a.Institution)
            .FirstOrDefault(a => a.Id == applicationId);
        if (application == null || application.IsReviewed)
            return false;
        if (isConfirmed)
        {
            var institution = application.Institution;
            institution.IsConfirmed = isConfirmed;
            _institutionManager.GeneratePrimaryInvitationCode(institution.Id);
        }
        application.IsReviewed = true;
        application.ApplicationResult = isConfirmed;
        _repository.SaveChanges();
        return true;
    }
}