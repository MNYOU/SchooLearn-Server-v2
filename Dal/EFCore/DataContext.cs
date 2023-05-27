using System.Text.Json.Serialization.Metadata;
using Dal.EFCore.Configurations;
using Dal.Entities;
using Dal.Repositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Task = Dal.Entities.Task;

namespace Dal.EFCore;

public class DataContext : DbContext, IInstitutionRepository, IUserRepository, IAdministratorRepository,
    IApplicationRepository, ITeacherRepository, IStudentRepository, ITaskRepository, ISubjectRepository,
    IGroupRepository
{
    public DbSet<Institution> Institutions { get; set; }
    public DbSet<Application> Applications { get; set; }
    public DbSet<User> Users { get; set; }
    public DbSet<Admin> Admins { get; set; }
    public DbSet<Teacher> Teachers { get; set; }
    public DbSet<Group> Groups { get; set; }
    public DbSet<GroupStudent> GroupStudents { get; set; }
    public DbSet<Student> Students { get; set; }
    public DbSet<Subject> Subjects { get; set; }
    public DbSet<Task> Tasks { get; set; }
    public DbSet<SolvedTask> SolvedTasks { get; set; }

    public DbSet<FileData> FIles { get; set; }
    public DbSet<Difficulty> Difficulties { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        // optionsBuilder.UseMySQL("server=localhost;database=database;user=superarman14;password=kbnjlh*&4ngjH");
        optionsBuilder.UseNpgsql("Host=localhost;Port=5432;Database=postgres;Username=postgres;Password=jhtnmb32;");
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(InstitutionConfiguration).Assembly);
    }
}