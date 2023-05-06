using Dal.Entities;
using Microsoft.EntityFrameworkCore;

namespace Dal.Repositories;

public interface ISubjectRepository
{
    DbSet<Subject> Subjects { get; set; }

    int SaveChanges();
    
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken()); 
}