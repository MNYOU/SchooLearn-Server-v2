using System.Net.Mime;
using AutoMapper;
using Dal.Entities;
using Dal.Repositories;
using Logic.ApiModels;
using Logic.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Logic.Implementations;

public class InstitutionManager : IInstitutionManager
{
    private readonly IMapper _mapper;
    private readonly IInstitutionRepository _repository;
    private readonly IProjectManager _projectManager;

    public InstitutionManager(IMapper mapper,IInstitutionRepository repository, IProjectManager projectManager)
    {
        _mapper = mapper;
        _repository = repository;
        _projectManager = projectManager;
    }

    public async Task<Application?> CreateApplication(InstitutionApiModel model)
    {
        var institution = _mapper.Map<Institution>(model);
        if (_repository.Institutions.Any(i => IsPartiallyEqual(i, institution)))
            return null;
        await _repository.Institutions.AddAsync(institution);
        var application = await _projectManager.CreateApplication(institution);
        await _repository.SaveChangesAsync();
        return application;
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