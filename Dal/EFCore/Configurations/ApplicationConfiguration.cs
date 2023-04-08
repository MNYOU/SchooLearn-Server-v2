using Dal.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Dal.EFCore.Configurations;

public class ApplicationConfiguration : IEntityTypeConfiguration<Application>
{
    public void Configure(EntityTypeBuilder<Application> builder)
    {
        builder
            .Property(a => a.Id)
            .HasColumnName("id");

        builder
            .Property(a => a.IsReviewed)
            .HasDefaultValue(false)
            .HasColumnName("is_reviewed");

        builder
            .HasOne(a => a.Institution);

        builder
            .Property(a => a.ApplicationResult)
            .HasDefaultValue(false)
            .HasColumnName("result");
    }
}