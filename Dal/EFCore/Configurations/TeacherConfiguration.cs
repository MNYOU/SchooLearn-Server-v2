using Dal.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Dal.EFCore.Configurations;

public class TeacherConfiguration: IEntityTypeConfiguration<Teacher>
{
    public void Configure(EntityTypeBuilder<Teacher> builder)
    {
        builder
            .ToTable("teachers");
        
        builder
            .HasKey(t => t.UserId);

        builder
            .HasOne<User>(t => t.User)
            .WithOne();

        builder
            .HasOne(t => t.Institution)
            .WithMany(i => i.Teachers)
            .HasForeignKey(s => s.InstitutionId);
    }
}