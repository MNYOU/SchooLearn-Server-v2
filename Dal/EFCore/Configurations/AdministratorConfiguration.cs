using Dal.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Dal.EFCore.Configurations;

public class AdministratorConfiguration : IEntityTypeConfiguration<Admin>
{
    public void Configure(EntityTypeBuilder<Admin> builder)
    {
        builder
            .HasKey(a => a.UserId);

        builder
            .HasOne<User>(a => a.User)
            .WithOne();

        builder
            .HasOne(a => a.Institution)
            .WithOne(i => i.Admin);
    }
}