using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using AutoMapper;
using Dal.Entities;
using Dal.Enums;
using Dal.Repositories;
using Logic.ApiModels;
using Logic.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace Logic.Implementations;

public class AccountManager : IAccountManager
{
    private readonly IConfiguration _config;
    private readonly IMapper _mapper;
    private readonly IUserRepository _repository;
    private readonly IAdministratorManager _administratorManager;
    private readonly ITeacherManager _teacherManager;
    private readonly IStudentManager _studentManager;

    public AccountManager(IConfiguration config, IUserRepository repository, IAdministratorManager administratorManager,
        ITeacherManager teacherManager, IStudentManager studentManager, IMapper mapper)
    {
        _config = config;
        _repository = repository;
        _administratorManager = administratorManager;
        _teacherManager = teacherManager;
        _studentManager = studentManager;
        _mapper = mapper;
    }

    public User? Get(long id)
    {
        return _repository.Users
            .FirstOrDefault(u => u.Id == id);
    }

    public async Task<User?> GetAsync(long id)
    {
        return await _repository.Users
            .FirstOrDefaultAsync(u => u.Id == id);
    }

    public User? GetWithDetails(long id)
    {
        return _repository.Users
            .Include(u => u.Institution)
            .FirstOrDefault(u => u.Id == id);
    }

    public async Task<User?> GetWithDetailsAsync(long id)
    {
        return await _repository.Users
            .Include(u => u.Institution)
            .FirstOrDefaultAsync(u => u.Id == id);
    }

    public async Task<bool> Register(RegistrationApiModel model)
    {
        var user = await RegisterBaseUser(model);
        if (user is null) return false;
        var institutionCode = model.InvitationCode ?? 0;
        var result = model.Role switch
        {
        Role.Admin => await _administratorManager.Register(user.Id, institutionCode),
        Role.Teacher => await _teacherManager.Register(user.Id, institutionCode),
        Role.Student => await _studentManager.Register(user.Id),
        _ => user != null,
        };

        return result;
    }

    private async Task<User?> RegisterBaseUser(RegistrationApiModel model)
    {
        if (_repository.Users.Any(u => u.Email == model.Email || u.Login == model.Login))
            return null;
        var user = _mapper.Map<User>(model);
        var hasher = new PasswordHasher<User>();
        user.Password = hasher.HashPassword(user, user.Password);
        await _repository.Users.AddAsync(user);
        await _repository.SaveChangesAsync();
        return user;
    }

    public AuthorizedApiModel? GetAuthorizedModel(LoginApiModel model)
    {
        var user = Authenticate(model);
        if (user is null)
            return null;
        var claims = new List<Claim>()
        {
            new Claim("TaskId", user.Email),
            new Claim(ClaimTypes.Name, user.Nickname),
            new Claim(ClaimTypes.Email, user.Email),
            new Claim(ClaimTypes.Role, nameof(user.Role)),
        };
        var accessToken = GetAccessToken(claims);
        var apiModel = new AuthorizedApiModel() { Token = accessToken };
        return apiModel;
    }

    private User? Authenticate(LoginApiModel model)
    {
        var user = _repository.Users.FirstOrDefault(u => u.Login == model.Login);
        if (user == null) return null;

        var hasher = new PasswordHasher<User>();
        var verificationResult = hasher.VerifyHashedPassword(user, user.Password, model.Password);
        return verificationResult == PasswordVerificationResult.Success
            ? user
            : null;
    }

    private string GetAccessToken(IEnumerable<Claim> claims)
    {
        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["JWTSettings:Key"] ?? "123"));
        var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
        var token = new JwtSecurityToken(
            _config["JWTSettings:Issuer"],
            _config["JWTSettings:Audience"],
            claims,
            expires: DateTime.Now.AddMilliseconds(long.Parse(_config["JWTSettings:LifeTime"] ?? "100000")),
            signingCredentials: credentials);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}