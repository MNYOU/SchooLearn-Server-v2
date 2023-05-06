using Dal.Entities;
using Microsoft.EntityFrameworkCore;
using Task = Dal.Entities.Task;

namespace Dal.Repositories;

public interface ITaskRepository : IDisposable
{
    DbSet<Task> Tasks { get; set; }
    
    DbSet<SolvedTask> SolvedTasks { get; set; }

    // DbSet<SolvedExtendedTask> SolvedExtendedTasks { get; set; }

    int SaveChanges();

    Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken());
}