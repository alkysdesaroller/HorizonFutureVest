using HorizonFutureVest.Persistence.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;


namespace HorizonFutureVest.Persistence.EntitiesConfiguration
{
    public class ConfiguracionTasasEntityConfiguration : IEntityTypeConfiguration<ConfiguracionTasas>
    {
        public void Configure(EntityTypeBuilder<ConfiguracionTasas> builder)
        {
            builder.HasKey(e => e.Id);
            builder.ToTable("ConfiguracionTasas");
            builder.Property(e => e.TasaMinima).HasPrecision(5, 2);
            builder.Property(e => e.TasaMaxima).HasPrecision(5, 2);
        }
    }
}
