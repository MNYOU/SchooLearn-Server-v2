using Dal.Entities;
using Microsoft.EntityFrameworkCore;

namespace Dal.Repositories;

public interface IGroupRepository
{
    DbSet<Group> Groups { get; set; }
    
    DbSet<GroupStudent> GroupStudents { get; set; }

    int SaveChanges();
    
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken()); 
}