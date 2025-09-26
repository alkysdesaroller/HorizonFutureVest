namespace HorizonFutureVest.BusinessLogic.DTOs;

public class MacroIndicadorSimulacionDto
{
    public int Id { get; set; }
    public int MacroindicadorId { get; set; }
    public string NombreMacroindicador { get; set; } = string.Empty;
    public decimal PesoSimulacion { get; set; }
    public bool EsMejorMasAlto { get; set; }
}