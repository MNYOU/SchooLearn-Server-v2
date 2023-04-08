namespace Logic.ApiModels;

public record AuthorizedApiModel
{
    public string Email { get; set; }

    public string NickName { get; set; }

    public string Login { get; set; }

    public string Institution { get; set; } // только название?

    public string Token { get; set; }

    public long LifeTime { get; set; }
};