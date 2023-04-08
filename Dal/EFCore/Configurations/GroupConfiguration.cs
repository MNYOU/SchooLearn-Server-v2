using Dal.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Dal.EFCore.Configurations;

public class GroupConfiguration: IEntityTypeConfiguration<Group>
{
    public void Configure(EntityTypeBuilder<Group> builder)
    {
        builder
            .Property(g => g.Id)
            .HasColumnName("id");

        builder
            .Property(g => g.Name)
            .HasColumnName("name");

        builder.Property(g => g.Description)
            .HasColumnName("description");

        builder
            .HasIndex(b => b.InvitationCode)
            .IsUnique();

        builder
            .Property(g => g.InvitationCode)
            .HasColumnName("invitation_code");

        builder
            .HasOne(b => b.Teacher)
            .WithMany(t => t.Groups)
            .HasForeignKey(b => b.TeacherId);

        builder.HasOne(b => b.Subject)
            .WithMany(s => s.Groups)
            .HasForeignKey(b => b.SubjectId);
    }
}