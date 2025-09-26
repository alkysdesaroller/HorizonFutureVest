using HorizonFutureVest.Persistence.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;


namespace HorizonFutureVest.Persistence.EntitiesConfiguration
{
    public class PaisEntityConfiguration : IEntityTypeConfiguration<Pais>
    {
        public void Configure(EntityTypeBuilder<Pais> builder)
        {

            builder.HasKey(e => e.Id);
            builder.ToTable("Pais");
            builder.Property(e => e.Nombre).IsRequired().HasMaxLength(100);
            builder.Property(e => e.CodigoIso).IsRequired().HasMaxLength(3);
            builder.HasIndex(e => e.CodigoIso).IsUnique();
        }
    }
}
