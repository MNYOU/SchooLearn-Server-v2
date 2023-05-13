using Dal.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Dal.EFCore.Configurations;

public class GroupStudentConfiguration: IEntityTypeConfiguration<GroupStudent>
{
    public void Configure(EntityTypeBuilder<GroupStudent> builder)
    {
        builder
            .HasKey(gs => new { gs.GroupId, gs.StudentId });

        builder
            .Property(gs => gs.IsApproved)
            .HasDefaultValue(false)
            .HasColumnName("is_approved");
        
        builder
            .HasOne(gs => gs.Group)
            .WithMany(g => g.GroupsStudent)
            .HasForeignKey(gs => gs.GroupId);
        
        builder
            .HasOne(gs => gs.Student)
            .WithMany(g => g.GroupsStudent)
            .HasForeignKey(gs => gs.StudentId);
    }
}