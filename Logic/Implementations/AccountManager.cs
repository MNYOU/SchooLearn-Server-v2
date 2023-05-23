using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using AutoMapper;
using Dal.Entities;
using Dal.Enums;
using Dal.Repositories;
using Logic.ApiModels;
using Logic.Interfaces;
using Microsoft.AspNetCore.Authentication;
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
    private readonly IInstitutionManager _institutionManager;
    private readonly IAdministratorManager _administratorManager;
    private readonly ITeacherManager _teacherManager;
    private readonly IStudentManager _studentManager;

    public AccountManager(IConfiguration config, IUserRepository repository, IAdministratorManager administratorManager,
        ITeacherManager teacherManager, IStudentManager studentManager, IMapper mapper,
        IInstitutionManager institutionManager)
    {
        _config = config;
        _repository = repository;
        _administratorManager = administratorManager;
        _teacherManager = teacherManager;
        _studentManager = studentManager;
        _mapper = mapper;
        _institutionManager = institutionManager;
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

    public void Update(User user)
    {
        _repository.Users.Update(user);
        _repository.SaveChanges();
    }

    public bool Rename(long id, string newName)
    {
        if (!CanUseNickname(newName)) return false;
        var user = Get(id);
        if (user is null) return false;
        user.Nickname = newName;
        _repository.SaveChanges();
        return true;
    }


    public async Task<bool> Register(RegistrationApiModel model)
    {
        var invitationCode = model.InvitationCode ?? "0";
        var user = await RegisterBaseUser(model);
        if (user is null) return false;
        var result = model.Role switch
        {
            Role.Admin => await _administratorManager.Register(user.Id, invitationCode, this),
            Role.Teacher => await _teacherManager.Register(user.Id, invitationCode, this),
            Role.Student => await _studentManager.Register(user.Id, this),
            _ => user != null,
        };
        if (!result)
        {
            try
            {
                _repository.Users.Remove(user);
                await _repository.SaveChangesAsync();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return false;
            }
        }

        return result;
    }

    private async Task<User?> RegisterBaseUser(RegistrationApiModel model)
    {
        if (_repository.Users.Any(u => u.Email == model.Email || u.Login == model.Login) ||
            !CanUseNickname(model.Nickname))
            return null;
        var user = _mapper.Map<User>(model);
        if (user.Role == Role.SuperManager &&
            !user.Nickname.Contains("armanarman", StringComparison.CurrentCultureIgnoreCase))
            return null;
        var hasher = new PasswordHasher<User>();
        user.Password = hasher.HashPassword(user, user.Password);
        await _repository.Users.AddAsync(user);
        await _repository.SaveChangesAsync();
        return user;
    }

    private bool CanUseNickname(string name)
    {
        if (name == "" || name.Split().Length == 0) return false;
        if (_repository.Users.Any(u => u.Nickname == name)) return false;
        return name
            .Split()
            .All(word => word
                .All(char.IsLetterOrDigit));
    }

    public AuthorizedApiModel? GetAuthorizedModel(LoginApiModel model, IInstitutionManager institutionManager)
    {
        var user = Authenticate(model);
        if (user is null)
            return null;
        var claims = new List<Claim>()
        {
            new("Id", user.Id.ToString()),
            new(ClaimTypes.Name, user.Nickname),
            new(ClaimTypes.Email, user.Email),
            new(ClaimTypes.Role, user.Role.ToString()),
        };
        var institution = _mapper.Map<InstitutionApiModel>(institutionManager.Get(user.InstitutionId ?? 0));
        return new AuthorizedApiModel
        {
            NickName = user.Nickname,
            Login = user.Login,
            Email = user.Email,
            Role = user.Role,
            Institution = institution,
            Token = GetAccessToken(claims),
            LifeTime = 3600000,
        };
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
        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["JWTSettings:SecretKey"] ?? "123"));
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