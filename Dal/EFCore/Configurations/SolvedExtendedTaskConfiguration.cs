using Dal.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Dal.EFCore.Configurations;

// public class SolvedExtendedTaskConfiguration : IEntityTypeConfiguration<SolvedExtendedTask>
// {
    // public void Configure(EntityTypeBuilder<SolvedExtendedTask> builder)
    // {
        // builder
            // .HasKey(s => new { s.StudentId, s.TaskId });

        // builder
            // .Property(s => s.FinalGrade)
            // .HasDefaultValue(false)
            // .HasColumnName("final_grade");

        // builder
            // .Property(s => s.IsChecked)
            // .HasDefaultValue(false)
            // .HasColumnName("is_checked");

        // builder
            // .Property(s => s.SolveTime)
            // .HasColumnName("solve_time");

        // builder.Property(s => s.AnswerAsFile)
            // .HasColumnName("answer_as_file");

        // builder
            // .Property(s => s.ContentType)
            // .HasDefaultValue("application/pdf")
            // .HasColumnName("content_type");

        // builder
            // .HasOne(s => s.Student)
            // .WithMany(s => s.SolvedExtendedTasks)
            // .HasForeignKey(s => s.StudentId);

        // builder
            // .HasOne(s => s.Task)
            // .WithMany(t => t.SolvedExtendedTasks)
            // .HasForeignKey(s => s.TaskId);
    // }
// }