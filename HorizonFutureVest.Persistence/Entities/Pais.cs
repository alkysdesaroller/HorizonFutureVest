using System.ComponentModel.DataAnnotations;

namespace HorizonFutureVest.Persistence.Entities;

public class Pais
{
    [Required(ErrorMessage = "El nombre del país es requerido")]
    [StringLength(100)]
    public string Nombre { get; set; } = string.Empty;

    [Required(ErrorMessage = "El código ISO es requerido")]
    [StringLength(3)]
    public string CodigoIso { get; set; } = string.Empty;

    public virtual ICollection<IndicadorPorPais> IndicadoresPorPais { get; set; } = new List<IndicadorPorPais>();
}