using Dal.Entities;
using Microsoft.EntityFrameworkCore;

namespace Dal.Repositories;

public interface IDifficultyRepository
{
    DbSet<Difficulty> Difficulties { get; set; }

    int SaveChanges();
    
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken()); 

}