using System.ComponentModel.DataAnnotations;

namespace HorizonFutureVest.Persistence.Entities;

public class ConfiguracionTasas
{
    public int Id { get; set; }
    
    [Required(ErrorMessage = "La tasa minima es requerida")]
    [Range(0, 100, ErrorMessage = "La tasa minima debe estar entre 0 y 100")]
    public decimal TasaMinima { get; set; }
    
    [Required(ErrorMessage = "La tasa maxima es requerida")]
    [Range(0, 100, ErrorMessage = "La tasa maxima debe estar entre 0 y 100")]
    public decimal TasaMaxima { get; set; }
}