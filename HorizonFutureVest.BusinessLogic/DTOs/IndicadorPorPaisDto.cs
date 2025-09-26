namespace HorizonFutureVest.BusinessLogic.DTOs
{
    public class IndicadorPorPaisDto
    {
        public int Id { get; set; }
        public int PaisId { get; set; }
        public string NombrePais { get; set; } = string.Empty;
        public int MacroindicadorId { get; set; }
        public string NombreMacroindicador { get; set; } = string.Empty;
        public decimal Valor { get; set; }
        public int Year { get; set; }
    }
}