using Dal.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Dal.EFCore.Configurations;

public class AdministratorConfiguration : IEntityTypeConfiguration<Admin>
{
    public void Configure(EntityTypeBuilder<Admin> builder)
    {
        builder
            .ToTable("admins");
            
        builder
            .HasKey(a => a.UserId);

        builder
            .HasOne(a => a.User)
            .WithOne();

        builder
            .HasOne(a => a.Institution)
            .WithOne(i => i.Admin)
            .HasForeignKey<Institution>(i => i.AdminId);
    }
}