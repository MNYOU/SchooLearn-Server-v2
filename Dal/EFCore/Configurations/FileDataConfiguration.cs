using Dal.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Dal.EFCore.Configurations;

public class FileDataConfiguration: IEntityTypeConfiguration<FileData>
{
    public void Configure(EntityTypeBuilder<FileData> builder)
    {
        builder
            .ToTable("file_data");

        builder
            .Property(f => f.Id)
            .HasColumnName("id");
        
        builder
            .Property(f => f.FileName)
            .HasColumnName("file_name");
        
        builder
            .Property(f => f.Content)
            .HasColumnName("content");
        
        builder
            .Property(f => f.ContentType)
            .HasColumnName("content_type");
    }
}