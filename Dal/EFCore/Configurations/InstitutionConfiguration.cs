using Dal.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Dal.EFCore.Configurations;

public class InstitutionConfiguration : IEntityTypeConfiguration<Institution>
{
    public void Configure(EntityTypeBuilder<Institution> builder)
    {
        builder
            .ToTable("institutions");
        
        builder
            .Property(i => i.Id)
            .HasColumnName("id");

        builder
            .Property(i => i.Name)
            .HasColumnName("name");

        builder
            .Property(i => i.TIN)
            .HasMaxLength(12)
            .IsFixedLength()
            .HasColumnName("tin");

        builder
            .Property(i => i.IsConfirmed)
            .HasColumnName("is_confirmed");

        builder.Property(i => i.InvitationCodeForTeachers)
            .HasColumnName("invitation_code_for_teachers");
    }
}