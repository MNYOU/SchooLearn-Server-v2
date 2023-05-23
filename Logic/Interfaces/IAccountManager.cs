using Dal.Entities;
using Logic.ApiModels;

namespace Logic.Interfaces;

public interface IAccountManager
{
    User? Get(long id);

    Task<User?> GetAsync(long id);

    User? GetWithDetails(long id);

    Task<User?> GetWithDetailsAsync(long id);

    void Update(User user);

    bool Rename(long id, string newName);

    Task<bool> Register(RegistrationApiModel model);

    AuthorizedApiModel? GetAuthorizedModel(LoginApiModel model, IInstitutionManager institutionManager);
}