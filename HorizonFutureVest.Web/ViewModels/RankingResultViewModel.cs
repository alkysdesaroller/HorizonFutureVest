namespace HorizonFutureVest.ViewModels;

public class RankingResultViewModel
{
    public string NombrePais { get; set; } = null!;
    public string CodigoPais { get; set; } = null!;
    public decimal Puntaje { get; set; }
    public int TasaRetornoEstimada  { get; set; }
    public int Posicion { get; set; }

    public string PuntajeFormateado => $"{Puntaje:F2}";
    public string TasaFormateada => $"{TasaRetornoEstimada:P2}";
}
