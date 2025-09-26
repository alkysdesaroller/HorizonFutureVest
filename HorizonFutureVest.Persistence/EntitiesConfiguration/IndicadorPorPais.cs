using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using HorizonFutureVest.Persistence.Entities;


namespace HorizonFutureVest.Persistence.EntitiesConfiguration
{
    public class IndicadorPorPaisEntityConfiguration : IEntityTypeConfiguration<IndicadorPorPais>
    {
        public void Configure(EntityTypeBuilder<IndicadorPorPais> builder)
        {
            builder.HasKey(e => e.Id);
            builder.ToTable("IndicadorPorPais");

            builder.Property(e => e.Valor).HasPrecision(18, 6);
            builder.HasOne(e => e.Pais)
                .WithMany(p => p.IndicadoresPorPais)
                .HasForeignKey(e => e.PaisId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(e => e.Macroindicador)
                .WithMany(m => m.IndicadoresPorPais)
                .HasForeignKey(e => e.MacroindicadorId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasIndex(e => new { e.PaisId, e.MacroindicadorId, e.Year })
                .IsUnique();
        }
    }
}
