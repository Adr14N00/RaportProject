using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RaportAPI.Core.Domain.Entities;

namespace RaportAPI.Infrastructure.Persistence.Configuration
{
    public class ExporthistoryConfiguration : IEntityTypeConfiguration<ExportHistory>
    {
        public void Configure(EntityTypeBuilder<ExportHistory> entity)
        {
            entity.HasKey(e => e.Id).HasName("exporthistory_pkey");

            entity.ToTable("exporthistory");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Exportdatetime)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("exportdatetime");
            entity.Property(e => e.Exportname)
                .HasMaxLength(255)
                .HasColumnName("exportname");
            entity.Property(e => e.LocationName)
                .HasMaxLength(100)
                .HasColumnName("locationname");
            entity.Property(e => e.Username)
                .HasMaxLength(100)
                .HasColumnName("username");
        }
    }
}
