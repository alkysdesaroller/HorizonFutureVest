using HorizonFutureVest.Persistence.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HorizonFutureVest.Persistence.EntitiesConfiguration
{
    public class MacroIndicadorEntityConfiguration : IEntityTypeConfiguration<MacroIndicador>
    {
        public void Configure(EntityTypeBuilder<MacroIndicador> builder)
        {
            builder.HasKey(e => e.Id);
            builder.ToTable("MacroIndicador");
            builder.Property(e => e.Nombre).IsRequired().HasMaxLength(100);
            builder.Property(e => e.Peso).HasPrecision(5, 4);
        }
    }
}
