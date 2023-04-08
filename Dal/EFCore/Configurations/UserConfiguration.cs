using Dal.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Dal.EFCore.Configurations;

public class UserConfiguration: IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder
            .Property(u => u.Id)
            .HasColumnName("id");
        
        builder
            .Property(u => u.Nickname)
            .HasMaxLength(100) // <??>
            .HasColumnName("nickname");
        
        builder
            .Property(u => u.Login)
            .HasColumnName("login");
        
        builder
            .Property(u => u.Password)
            .HasColumnName("password");

        builder
            .Property(u => u.Email)
            .HasColumnName("email");
        
        builder
            .Property(u => u.Role)
            .HasColumnName("role");
    }
}