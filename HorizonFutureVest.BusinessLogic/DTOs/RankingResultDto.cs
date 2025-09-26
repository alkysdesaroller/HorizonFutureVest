namespace HorizonFutureVest.BusinessLogic.DTOs;

public class RankingResultDto
{
    public string NombrePais { get; set; } = string.Empty;
    public string CodigoIso { get; set; } = string.Empty;
    public decimal Puntaje { get; set; }
    public decimal TasaRetornoEstimada { get; set; }
    public int Posicion { get; set; }
}