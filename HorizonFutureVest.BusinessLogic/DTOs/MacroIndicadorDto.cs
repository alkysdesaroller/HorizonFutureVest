namespace HorizonFutureVest.BusinessLogic.DTOs;

public class MacroIndicadorDto
{
    public int Id { get; set; }
    public string Nombre { get; set; } = string.Empty;
    public decimal Peso { get; set; }
    public bool EsMejorMasAlto { get; set; }
    
}