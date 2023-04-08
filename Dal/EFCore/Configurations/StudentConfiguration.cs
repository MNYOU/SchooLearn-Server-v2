using Dal.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Dal.EFCore.Configurations;

public class StudentConfiguration: IEntityTypeConfiguration<Student>
{
    
    public void Configure(EntityTypeBuilder<Student> builder)
    {
        builder
            .HasKey(s => s.UserId);

        builder
            .HasOne<User>(s => s.User)
            .WithOne();

        builder
            .HasOne(s => s.Institution)
            .WithMany(i => i.Students)
            .HasForeignKey(s => s.InstitutionId);

        builder
            .Property(s => s.IsConfirmed)
            .HasColumnName("is_confirmed");
    }
}