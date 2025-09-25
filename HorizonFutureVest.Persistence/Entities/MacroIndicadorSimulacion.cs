using System.ComponentModel.DataAnnotations;

namespace HorizonFutureVest.Persistence.Entities;

public class MacroIndicadorSimulacion
{
    public int Id { get; set; }
    
    [Required]
    public int MacroindicadorId { get; set; }
    public virtual MacroIndicador Macroindicador { get; set; } = null!;
    
    [Required(ErrorMessage = "El peso es requerido")]
    [Range(0, 1, ErrorMessage = "El peso debe estar entre 0 y 1")]
    public decimal PesoSimulacion { get; set; }
}