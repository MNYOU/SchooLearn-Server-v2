using AutoMapper;
using Dal.Entities;
using Dal.Repositories;
using Logic.ApiModels;
using Logic.Helpers;
using Logic.Interfaces;

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

    public Institution? Get(long id)
    {
        return _repository.Institutions
            .FirstOrDefault(i => i.Id == id);
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
            .Select(i => _mapper.Map<InstitutionApiModel>(i))
            .AsEnumerable();
    }

    public IEnumerable<long> GetPrimaryInvitationCodes()
    {
        return _repository.Institutions
            .Where(i => i.IsConfirmed && i.PrimaryInvitationCode != null)
            .Select(i => i.PrimaryInvitationCode.Value);
    }

    public IEnumerable<long> GetOccupiedInvitationCodes()
    {
        return _repository.Institutions
            .Where(i => i.IsConfirmed && i.InvitationCodeForTeachers != null)
            .Select(i => i.InvitationCodeForTeachers.Value);
    }

    public bool GeneratePrimaryInvitationCode(long institutionId)
    {
        var institution = _repository.Institutions
            .FirstOrDefault(i => i.Id == institutionId);
        if (institution is { IsConfirmed: true })
        {
            var codes = GetPrimaryInvitationCodes();
            var newCode = Generator.GenerateInvitationCode(codes);
            institution.PrimaryInvitationCode = newCode;
            _repository.SaveChanges();
            return true;
        }

        return false;
    }

    public Institution? GetByPrimaryInvitationCode(long code)
    {
        var institution = _repository.Institutions
                .FirstOrDefault(i => i.IsConfirmed && i.PrimaryInvitationCode == code);
        return institution;
    }

    public Institution? GetByInvitationCode(long code)
    {
        var institution =
            _repository.Institutions
                .FirstOrDefault(i => i.IsConfirmed && i.InvitationCodeForTeachers == code);
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