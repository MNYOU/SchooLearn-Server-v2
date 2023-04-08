using System.Runtime.CompilerServices;
using Microsoft.EntityFrameworkCore;
using Task = Dal.Entities.Task;

namespace Dal.Repositories;

public interface ITaskRepository : IDisposable
{
    DbSet<Task> Tasks { get; set; }
    
    int SaveChanges();

    Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken());
}