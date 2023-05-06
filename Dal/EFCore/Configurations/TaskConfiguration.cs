using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Task = Dal.Entities.Task;

namespace Dal.EFCore.Configurations;

public class TaskConfiguration : IEntityTypeConfiguration<Task>
{
    public void Configure(EntityTypeBuilder<Task> builder)
    {
        builder
            .ToTable("tasks");

        builder
            .Property(t => t.Id)
            .HasColumnName("id");

        builder
            .Property(t => t.Name)
            .HasColumnName("name");

        builder
            .Property(t => t.Description)
            .HasColumnName("description");
        
        builder
            .Property(t => t.Answer)
            .HasColumnName("answer");

        builder
            .HasOne(t => t.Subject)
            .WithMany(s => s.Tasks)
            .HasForeignKey(t => t.SubjectId);
        
        builder
            .HasOne(t => t.Difficulty)
            .WithMany(s => s.Tasks)
            .HasForeignKey(t => t.DifficultyId);

        builder
            .Property(t => t.CreationDateTime)
            .HasColumnName("creation_datetime");

        builder
            .Property(t => t.ExecutionPeriod)
            .HasColumnName("execution_period");

        builder
            .HasMany(t => t.Groups)
            .WithMany(g => g.Tasks);
    }
}