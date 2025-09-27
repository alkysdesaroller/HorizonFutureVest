using System.ComponentModel.DataAnnotations;

namespace HorizonFutureVest.ViewModels;

public class PaisViewModel
{
    public int Id { get; set; }

    [Required(ErrorMessage = "El nombre del país es requerido")]
    [Display(Name = "Nombre del país")]
    public string Nombre { get; set; } = string.Empty;
    
    [Required(ErrorMessage = "El código ISO es requerido")]
    [StringLength(3, ErrorMessage = "El código ISO debe tener máximo 3 caracteres")]
    [Display(Name = "Código ISO")]
    public string CodigoIso { get; set; } = string.Empty;
}