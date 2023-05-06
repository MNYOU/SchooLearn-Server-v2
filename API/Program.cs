using System.Text;
using Dal.EFCore;
using Dal.EFCore.Repositories;
using Dal.Repositories;
using Logic.AutoMapper;
using Logic.Implementations;
using Logic.Interfaces;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var services = builder.Services;
services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
services.AddEndpointsApiExplorer();
services.AddSwaggerGen();
services.AddAuthentication(options =>
    {
        options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
    })
    .AddJwtBearer(options =>
    {
        options.SaveToken = true;
        options.RequireHttpsMetadata = true;
        options.TokenValidationParameters = new TokenValidationParameters()
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidAudience = builder.Configuration["JWTSettings:Audience"],
            ValidIssuer = builder.Configuration["JWTSettings:Issuer"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JWTSettings:SecretKey"] ?? "123")),
        };
    });
services.AddDbContext<DataContext>(); // хотя ms используют разные контексты
// services.AddIdentity<IdentityUser>();
// services
//     .AddIdentityCore<IdentityUser>(options => {
//         options.SignIn.RequireConfirmedAccount = false;
//         options.User.RequireUniqueEmail = true;
//         options.Password.RequireDigit = false;
//         options.Password.RequiredLength = 6;
//         options.Password.RequireNonAlphanumeric = false;
//         options.Password.RequireUppercase = false;
//         options.Password.RequireLowercase = false;
//     })
//     .AddEntityFrameworkStores<DataContext>();
services.AddAutoMapper(typeof(AutoMapperProfile));

services.TryAddScoped<IUserRepository, DataContext>();
services.TryAddScoped<IAdministratorRepository, DataContext>();
services.TryAddScoped<IInstitutionRepository, DataContext>();
services.TryAddScoped<IApplicationRepository, DataContext>();
services.TryAddScoped<ITeacherRepository, DataContext>();
services.TryAddScoped<IStudentRepository, DataContext>();
services.TryAddScoped<IGroupRepository, DataContext>();
services.TryAddScoped<ITaskRepository, DataContext>();
services.TryAddScoped<ISubjectRepository, DataContext>();
services.TryAddScoped<IDifficultyRepository, DataContext>();

services.TryAddScoped<IAccountManager, AccountManager>();
services.TryAddScoped<IInstitutionManager, InstitutionManager>();
services.TryAddScoped<IProjectManager, ProjectManager>();
services.TryAddScoped<IAdministratorManager, AdministratorManager>();
services.TryAddScoped<IStudentManager, StudentManager>();
services.TryAddScoped<ITeacherManager, TeacherManager>();
services.TryAddScoped<IStudentManager, IStudentManager>();
services.TryAddScoped<ITaskManager, TaskManager>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
else
{
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();