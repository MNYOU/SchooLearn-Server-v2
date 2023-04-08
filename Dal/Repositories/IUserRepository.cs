﻿using Dal.Entities;
using Microsoft.EntityFrameworkCore;

namespace Dal.Repositories;

public interface IUserRepository
{
    DbSet<User> Users { get; set; }

    int SaveChanges();
    
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken()); 
}