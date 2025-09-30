using Microsoft.AspNetCore.Mvc.Rendering;

namespace HorizonFutureVest.ViewModels;

public class SimulacionViewModel
{
    public List<MacroIndicadorSimulacionViewModel> MacroIndicadoresSimulacion { get; set; } = new List<MacroIndicadorSimulacionViewModel>();
    public int YearSelected { get; set; }
    public SelectList YearsAvailable { get; set; } = null!;
    public List<RankingResultViewModel> Resultados { get; set; } = new List<RankingResultViewModel>();
    public string MensajeError { get; set; } = string.Empty;
    public bool PuedeAgregarMas { get; set; } 
    public decimal SumaPesosActual { get; set; }
}