using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace HorizonFutureVest.ViewModels;

public class MacroIndicadorSimulacionViewModel
{
    public int Id { get; set; }
    public int MacroindicadorId { get; set; }
    public string NombreMacroindicador { get; set; } = null!;
    public decimal PesoSimulacion { get; set; }
    public bool EsMejorMasAlto { get; set; }
    
    [Required(ErrorMessage = "Debe seleccionar un macroindicador")]
    [Display(Name = "Macroindicador")]
    public int MacroindicadorSeleccionadoId { get; set; }
        
    [Required(ErrorMessage = "El peso es requerido")]
    [Range(0.01, 1.0, ErrorMessage = "El peso debe estar entre 0.01 y 1")]
    [Display(Name = "Peso para Simulación")]
    public decimal NuevoPeso { get; set; }
        
    public SelectList? MacroindicadoresDisponibles { get; set; }
}