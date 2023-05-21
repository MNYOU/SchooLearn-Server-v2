using Dal.Enums;

namespace Logic.ApiModels;

public record AuthorizedApiModel
{
    public string Email { get; set; }

    public string NickName { get; set; }

    public string Login { get; set; }
    
    public Role Role { get; set; }

    public InstitutionApiModel? Institution { get; set; }

    public string Token { get; set; }

    public long LifeTime { get; set; }
}