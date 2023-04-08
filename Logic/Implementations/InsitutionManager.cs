using Dal.Entities;
using Dal.Repositories;
using Logic.ApiModels;
using Logic.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Logic.Implementations;

public class InstitutionManager : IInstitutionManager
{
    private readonly IInstitutionRepository _repository;

    public InstitutionManager(IInstitutionRepository repository)
    {
        _repository = repository;
    }

    public async Task<Application?> CreateApplication(InstitutionApiModel model)
    {
        var institution = new Institution();
        if (_repository.Institutions.Any(i => IsPartiallyEqual(institution, i)))
            return null;

        var application = new Application() { Institution = institution, IsReviewed = false };
        await _repository.Institutions.AddAsync(institution);
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

    public IEnumerable<InstitutionApiModel> GetAllConfirmed()
    {
        return _repository.Institutions
            .Where(i => i.IsConfirmed)
            // mapping
            .Select(i => new InstitutionApiModel())
            .AsEnumerable();
    }

    public Institution? GetByPrimaryInvitationCode(long code)
    {
        var institution =
            _repository.Institutions.FirstOrDefault(i => i.IsConfirmed && i.PrimaryInvitationCode == code);
        return institution;
    }

    public Institution? GetByInvitationCode(long code)
    {
        var institution =
            _repository.Institutions.FirstOrDefault(i => i.IsConfirmed && i.InvitationCodeForTeachers == code);
        return institution;
    }

    public static bool IsPartiallyEqual(Institution a, Institution b)
    {
        return a.Id == b.Id
               || a.Name == b.Name
               || a.TIN == b.TIN
               || a.WebAddress == b.WebAddress;
    }
}