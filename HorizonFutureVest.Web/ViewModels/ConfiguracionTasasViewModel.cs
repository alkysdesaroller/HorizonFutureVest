using System.ComponentModel.DataAnnotations;

namespace HorizonFutureVest.ViewModels;

public class ConfiguracionTasasViewModel : IValidatableObject
{
    public int Id { get; set; }
    
    [Required(ErrorMessage = "La tasa es requerida")]
    [Range(0, 100, ErrorMessage = "La tasa debe estar entre 0 y 100")]
    [Display(Name = "Tasa Minimo Estimado de Retorno(%)")]
    public decimal TasaMinima { get; set; }
    
    [Required(ErrorMessage = "La tasa es requerida")]
    [Range(0, 100, ErrorMessage = "La tasa debe estar entre 0 y 100")]
    [Display(Name = "Tasa Máximo Estimado de Retorno(%)")]
    public decimal TasaMaxima { get; set; }


    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        if (TasaMinima >= TasaMaxima)
        {
            yield return new ValidationResult(
                "La tasa mínima debe ser menor que la tasa máxima",
                new[] { nameof(TasaMinima) });
        }
    }
}