using System.ComponentModel.DataAnnotations;

namespace HorizonFutureVest.ViewModels;

public class MacroIndicadorViewModel
{
    public int Id { get; set; }
    
    [Required(ErrorMessage = "El nombre del macroindicador es requerido")]
    [Display(Name = "Nombre del macroindicador")]
    public string Nombre { get; set; } = string.Empty;
    
    [Required(ErrorMessage = "El peso es requerido")]
    [Range(0.01, 1, ErrorMessage = "El peso debe estar entre 0.01 y 1")]
    [Display(Name = "Peso")]
    public decimal Peso { get; set; }
    
    [Display(Name = "¿Es mejor un valor más alto?")]
    public bool EsMejorMasAlto { get; set; }
    
    public decimal SumaPesosActual { get; set; }
    public decimal PesoOriginal { get; set; }
}