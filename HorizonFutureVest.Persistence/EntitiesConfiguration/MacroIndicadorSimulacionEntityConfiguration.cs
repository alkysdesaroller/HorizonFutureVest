using HorizonFutureVest.Persistence.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;



namespace HorizonFutureVest.Persistence.EntitiesConfiguration
{
    public class MacroIndicadorSimulacionEntityConfiguration : IEntityTypeConfiguration<MacroIndicadorSimulacion>
    {
        public void Configure(EntityTypeBuilder<MacroIndicadorSimulacion> builder)
        {
            
            builder.HasKey(e => e.Id);
            builder.ToTable("MacroIndicadorSimulacion");

            builder.Property(e => e.PesoSimulacion).HasPrecision(18, 6);
            builder.HasOne(e => e.Macroindicador)
                .WithMany(m => m.MacroindicadoresSimulacion)
                .HasForeignKey(e => e.MacroindicadorId)
                .OnDelete(DeleteBehavior.Cascade);
            builder.HasIndex(e => new { e.MacroindicadorId }).IsUnique();
        }
    }
}
