using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace HorizonFutureVest.ViewModels;

public class IndicadorPorPaisViewModel
{
    public int Id { get; set; }
    
    [Required(ErrorMessage = "Debe seleccionar un país")]
    [Display(Name = "País")]
    public int PaisId { get; set; }
        
    [Required(ErrorMessage = "Debe seleccionar un macroindicador")]
    [Display(Name = "Macroindicador")]
    public int MacroindicadorId { get; set; }
        
    [Required(ErrorMessage = "El valor del indicador es requerido")]
    [Display(Name = "Valor del Indicador")]
    public decimal Valor { get; set; }
        
    [Required(ErrorMessage = "El año es requerido")]
    [Range(1900, 9999, ErrorMessage = "Año inválido")]
    [Display(Name = "Año")]
    public int Year { get; set; }
        
    // Para dropdowns
    public SelectList Paises { get; set; } = null!;
    public SelectList Macroindicadores { get; set; } = null!;
        
    // Para mostrar en lista
    public string NombrePais { get; set; } = null!;
    public string NombreMacroindicador { get; set; } = null!;
        
    // Para filtros
    public int? FiltrarPorPaisId { get; set; }
    public int? FiltrarPorYear { get; set; }
    public SelectList PaisesFiltro { get; set; } = null!;
}