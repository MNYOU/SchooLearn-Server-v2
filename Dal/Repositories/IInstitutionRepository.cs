using Dal.Entities;
using Microsoft.EntityFrameworkCore;

namespace Dal.Repositories;

public interface IInstitutionRepository
{
    
    DbSet<Institution> Institutions { get; set; }
    DbSet<Application> Applications { get; set; }

    int SaveChanges();

    Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken());
}