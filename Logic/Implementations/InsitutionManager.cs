using AutoMapper;
using Dal.Entities;
using Dal.Repositories;
using Logic.ApiModels;
using Logic.Helpers;
using Logic.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Logic.Implementations;

public class InstitutionManager : IInstitutionManager
{
    private readonly IMapper _mapper;
    private readonly IInstitutionRepository _repository;

    public InstitutionManager(IMapper mapper,IInstitutionRepository repository)
    {
        _mapper = mapper;
        _repository = repository;
    }

    public Institution? Get(long id)
    {
        return _repository.Institutions
            .FirstOrDefault(i => i.Id == id);
    }

    public async Task<Application?> CreateApplication(InstitutionApiRequest request, IProjectManager projectManager)
    {
        var institution = _mapper.Map<Institution>(request);
        if (_repository.Institutions.AsEnumerable().Any(i => IsPartiallyEqual(i, institution)))
            return null;
        await _repository.Institutions.AddAsync(institution);
        await _repository.SaveChangesAsync();
        var application = await projectManager.CreateApplication(institution);
        return application;
    }

    public bool AddAdmin(Admin admin)
    {
        throw new NotImplementedException();
    }


    public IEnumerable<InstitutionApiModel> GetAllConfirmed()
    {
        return _repository.Institutions
            .Where(i => i.IsConfirmed)
            .Select(i => _mapper.Map<InstitutionApiModel>(i))
            .AsEnumerable();
    }

    public IEnumerable<string> GetPrimaryInvitationCodes()
    {
        return _repository.Institutions
            .Where(i => i.IsConfirmed && i.PrimaryInvitationCode != null)
            .Select(i => i.PrimaryInvitationCode ?? "");
    }

    public IEnumerable<string> GetOccupiedInvitationCodes()
    {
        return _repository.Institutions
            .Where(i => i.IsConfirmed && i.InvitationCodeForTeachers != null)
            .Select(i => i.InvitationCodeForTeachers ?? "");
    }

    public bool GeneratePrimaryInvitationCode(long institutionId)
    {
        var institution = _repository.Institutions
            .FirstOrDefault(i => i.Id == institutionId);
        if (institution != null)
        {
            var codes = GetPrimaryInvitationCodes();
            var newCode = Generator.GenerateInvitationCode(codes);
            institution.PrimaryInvitationCode = newCode;
            _repository.SaveChanges();
            return true;
        }

        return false;
    }

    public Institution? GetByPrimaryInvitationCode(string code)
    {
        var institution = _repository.Institutions
            .Include(i => i.Admin)
            .ThenInclude(a => a!.User)
            .FirstOrDefault(i => i.IsConfirmed && i.PrimaryInvitationCode == code);
        return institution;
    }

    public Institution? GetByInvitationCode(string code)
    {
        var institution =
            _repository.Institutions
                .Include(i => i.Admin)
                .Include(i => i.Teachers)
                .Include(i => i.Students)
                .FirstOrDefault(i => i.IsConfirmed && i.InvitationCodeForTeachers == code);
        return institution;
    }

    public static bool IsPartiallyEqual(Institution a, Institution b)
    {
        var comparisonResult = a.Id == b.Id
                               || a.Name == b.Name
                               || a.TIN == b.TIN
                               || a.WebAddress == b.WebAddress;
        return comparisonResult;
    }
}