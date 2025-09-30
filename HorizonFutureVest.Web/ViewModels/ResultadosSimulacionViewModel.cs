namespace HorizonFutureVest.ViewModels;

public class ResultadosSimulacionViewModel
{
    public int Year { get; set; }
    public List<RankingResultViewModel> Resultados { get; set; } = new();
}