using LoginAuthenticatorCode.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LoginAuthenticatorCode.Data.Mapping;

public class PermissionMap : IEntityTypeConfiguration<Permission>
{
    public void Configure(EntityTypeBuilder<Permission> builder)
    {
        builder.ToTable("Permission");
        builder.HasKey(p => p.Id);
        builder.Property(p => p.Name).HasMaxLength(100).IsRequired();
        builder.Property(p => p.TypeAccess).IsRequired();
        builder.Property(p => p.Description).HasMaxLength(250);
        // Define relationships if necessary
        builder.HasMany(p => p.Users).WithOne(c => c.Permission).HasForeignKey(c => c.PermissionId);
    }
}