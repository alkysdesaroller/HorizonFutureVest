using Microsoft.AspNetCore.Mvc.Rendering;

namespace HorizonFutureVest.ViewModels;

public class RankingViewModel
{
    public int YearSelected { get; set; }
    public SelectList YearsAvailable { get; set; } = null!;
    public List<RankingResultViewModel> Resultados { get; set; } = new List<RankingResultViewModel>();
    public string MensajeError { get; set; } = string.Empty;
    public bool TieneResultado => Resultados.Any();
}