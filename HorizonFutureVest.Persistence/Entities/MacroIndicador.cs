using System.ComponentModel.DataAnnotations;

namespace HorizonFutureVest.Persistence.Entities;

public class MacroIndicador
{
    public int Id { get; set; }

    [Required(ErrorMessage = "El nombre del macroindicador es requerido")]
    [StringLength(100)]
    public string Nombre { get; set; } = string.Empty;

    [Required(ErrorMessage = "El peso es requerido")]
    [Range(0, 1, ErrorMessage = "El peso debe estar entre 0 y 1")]
    public decimal Peso { get; set; }

    public bool EsMejorMasAlto { get; set; }

    public virtual ICollection<IndicadorPorPais> IndicadoresPorPais { get; set; } = new List<IndicadorPorPais>();

    public virtual ICollection<MacroIndicadorSimulacion> MacroindicadoresSimulacion { get; set; } =
        new List<MacroIndicadorSimulacion>();
}