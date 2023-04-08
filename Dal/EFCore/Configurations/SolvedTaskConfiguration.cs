using Dal.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Dal.EFCore.Configurations;

public class SolvedTaskConfiguration: IEntityTypeConfiguration<SolvedTask>
{
    public void Configure(EntityTypeBuilder<SolvedTask> builder)
    {
        builder
            .HasKey(e => new { e.StudentId, e.TaskId });

        builder
            .Property(s => s.Answer)
            .HasColumnName("answer");

        builder
            .Property(s => s.SolveTime)
            .HasColumnName("solve_time");

        builder
            .HasOne(s => s.Student)
            .WithMany(s => s.SolvedTasks)
            .HasForeignKey(s => s.StudentId);
        
        builder
            .HasOne(s => s.Task)
            .WithMany(t => t.SolvedTasks)
            .HasForeignKey(s => s.TaskId);
    }
}