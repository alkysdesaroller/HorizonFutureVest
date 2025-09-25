using System.ComponentModel.DataAnnotations;

namespace HorizonFutureVest.Persistence.Entities;

public class IndicadorPorPais
{
    public int Id { get; set; }
    [Required]
    public int PaisId { get; set; }
    public virtual Pais Pais { get; set; } = null!;
        
    [Required]
    public int MacroindicadorId { get; set; }
    public virtual MacroIndicador Macroindicador { get; set; } = null!;
        
    [Required(ErrorMessage = "El valor del indicador es requerido")]
    public decimal Valor { get; set; }
        
    [Required(ErrorMessage = "El año es requerido")]
    [Range(1960, 2100, ErrorMessage = "Año inválido")]
    public int Year { get; set; }
}