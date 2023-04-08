using Logic.ApiModels;

namespace Logic.Interfaces;

public interface IAccountManager
{
    Task<bool> Register(RegistrationApiModel model);

    AuthorizedApiModel? GetAuthorizedModel(LoginApiModel model);
}