using HorizonFutureVest.BusinessLogic.DTOs;
using HorizonFutureVest.BusinessLogic.Interfaces;
using HorizonFutureVest.Persistence.DbContextHorizon;
using HorizonFutureVest.Persistence.Entities;
using Microsoft.EntityFrameworkCore;

namespace HorizonFutureVest.BusinessLogic.Services
{
    public class RankingService : IRankingService
    {
        private readonly IConfiguracionTasasService _configuracionTasasService;
        private readonly HorizonContext _context;

        public RankingService(HorizonContext context, IConfiguracionTasasService configuracionTasasService)
        {
            _configuracionTasasService = configuracionTasasService;
            _context = context;
        }

        public async Task<(bool Success, string ErrorMessage, List<RankingResultDto> Resultado)>
            GenerarRankingAsync(int year)
        {
            var macroIndicadores = await _context.MacroIndicadores.ToListAsync();
            var sumaPesos = macroIndicadores.Sum(mi => mi.Peso);

            if (Math.Abs(sumaPesos - 1.0m) > 0.0001m)
            {
                return (false,
                    "Se deben ajustar los pesos de los macroindicadores registrados hasta que la suma de los mismos sea igual a 1",
                    null!);
            }

            // Ahora devuelve List<Pais> en vez de object
            var paisesElegibles = await GetPaisesElegiblesAsync(year, macroIndicadores);

            if (paisesElegibles.Count < 2)
            {
                var mensaje = paisesElegibles.Count == 1
                    ? $"No hay suficientes países para poder calcular el ranking y la tasa de retorno, el único país que cumple con los requisitos es {paisesElegibles.First().Nombre}, debe agregar más indicadores a los demás países en el año seleccionado"
                    : "No hay países suficientes que cumplan con los requisitos para generar el ranking";

                return (false, mensaje, null!);
            }

            var resultado = await CalcularRankingAsync(paisesElegibles, macroIndicadores, year);
            return (true, string.Empty, resultado);
        }

        // Cambiado paisesElegibles a List<Pais>
        private async Task<List<RankingResultDto>> CalcularRankingAsync(List<PaisElegible> paisesElegibles, List<MacroIndicador> macroIndicadores, int year)
        {
           var resultados = new List<RankingResultDto>();
           var tasas = await _configuracionTasasService.GetConfiguracionTasasAsync();
           var tasaMinima = tasas.TasaMinima;
           var tasaMaxima = tasas.TasaMaxima;
           
           var valoresNormalizados = new Dictionary<int, (decimal min, decimal max)>();
           
              foreach (var mi in macroIndicadores)
              {
                var valores = paisesElegibles
                     .Select(p => p.Indicadores[mi.Id])
                     .ToList();
                
                valoresNormalizados[mi.Id] = (valores.Min(), valores.Max());
              }

              foreach (var pais in paisesElegibles)
              {
                  decimal scoring = 0;
                  foreach (var mi in macroIndicadores)
                  {
                      var valor = pais.Indicadores[mi.Id];
                      var (min, max) = valoresNormalizados[mi.Id];
                      
                      decimal valorNormalizado;
                      if (max == min)
                      {
                          valorNormalizado = 0.5m; // Todos los países tienen el mismo valor
                      }
                      else if (mi.EsMejorMasAlto)
                      {
                          valorNormalizado = (valor - min) / (max - min);
                      }
                      else
                      {
                          valorNormalizado = (max - valor) / (max - min);
                      }

                     decimal subPuntaje = valorNormalizado * mi.Peso;
                     scoring += subPuntaje;
                  }

                  scoring = Math.Max(0, Math.Min(1, scoring));
                  decimal tasaRetorno = tasaMinima + (scoring * (tasaMaxima - tasaMinima));
                  
                  resultados.Add(new RankingResultDto
                  {
                      NombrePais = pais.Nombre!,
                      CodigoIso = pais.CodigoIso!,
                      Scoring = scoring, // Escalado a 0-100
                      TasaRetornoEstimada = tasaRetorno / 100 // Escalado a porcentaje
                  });
              }
              resultados =  resultados.OrderByDescending(r => r.Scoring).ToList();
              for (int i = 0; i < resultados.Count; i++)
              {
                  resultados[i].Posicion = i + 1;
              }
                return resultados;
        }

        // Cambiado tipo de retorno a List<Pais>
        private async Task<List<PaisElegible>> GetPaisesElegiblesAsync(int year, List<MacroIndicador> macroIndicadores)
        {
            var paisesElegibles = new List<PaisElegible>();
            var paises = await _context.Paises.ToListAsync();
            
            var macroIndicadorValido = macroIndicadores.Where(m => m.Peso > 0).ToList();

            foreach (var pais in paises)
            {
                var indicadoresPais = await _context.IndicadoresPorPais
                    .Include(i => i.Macroindicador)
                    .Where(i => i.PaisId == pais.Id && i.Year == year )
                    .ToListAsync();
                
                var tieneTodosLosIndicadores = macroIndicadorValido.All(mi => indicadoresPais.Any(ip => ip.MacroindicadorId == mi.Id));

                if (tieneTodosLosIndicadores)
                {
                   paisesElegibles.Add(new PaisElegible
                   {
                       Id = pais.Id,
                       Nombre = pais.Nombre,
                       CodigoIso = pais.CodigoIso,
                       Indicadores = indicadoresPais.ToDictionary(ip => ip.MacroindicadorId, ip => ip.Valor)
                   });
                }
            }

            return paisesElegibles;
        }

        public async Task<(bool Success, string ErrorMessage, List<RankingResultDto> Resultado)>
            GenerarRankingSimulacionAsync(int year)
        {
           var macroIndicadoresSimulacion = await _context.MacroIndicadorSimulaciones
               .Include(ms => ms.Macroindicador)
               .ToListAsync();
           
           if (!macroIndicadoresSimulacion.Any())
           {
               return (false, "No hay macroindicadores agregados para simular.", null!);
           }
              var sumaPesos = macroIndicadoresSimulacion.Sum(mi => mi.PesoSimulacion);
                if (Math.Abs(sumaPesos - 1.0m) > 0.0001m)
                {
                    return (false,
                        "Se deben ajustar los pesos de los macroindicadores registrados hasta que la suma de los mismos sea igual a 1",
                        null!);
                }
                var macroIndicadores = macroIndicadoresSimulacion
                    .Select(ms => new MacroIndicador
                    {
                        Id = ms.Macroindicador.Id,
                        Nombre = ms.Macroindicador.Nombre,
                        Peso = ms.PesoSimulacion,
                        EsMejorMasAlto = ms.Macroindicador.EsMejorMasAlto
                    })
                    .ToList();
                
                var paisesElegibles = await GetPaisesElegiblesAsync(year, macroIndicadores.Select(m => new MacroIndicador
                {
                    Id = m.Id,
                    Nombre = m.Nombre,
                    Peso = m.Peso,
                    EsMejorMasAlto = m.EsMejorMasAlto
                }).ToList());

                if (paisesElegibles.Count < 2)
                {
                    var mensaje = paisesElegibles.Count == 1
                        ? $"No hay suficientes países para poder calcular el ranking y la tasa de retorno, el único país que cumple con los requisitos es {paisesElegibles.First().Nombre}, debe agregar más indicadores a los demás países en el año seleccionado"
                        : "No hay países suficientes que cumplan con los requisitos para generar el ranking";
                    return (false, mensaje, null!);
                }
                var resultado = await CalcularRankingAsync( paisesElegibles, macroIndicadores.Select(m => new MacroIndicador
                {
                    Id = m.Id,
                    Nombre = m.Nombre,
                    Peso = m.Peso,
                    EsMejorMasAlto = m.EsMejorMasAlto
                }).ToList(), year);
                
                return (true, string.Empty, resultado);
        }

        public async Task<IEnumerable<int>> GetYearConIndicadoresAsync()
        {
           return await _context.IndicadoresPorPais
                .Select(ip => ip.Year)
                .Distinct()
                .OrderByDescending(y => y)
                .ToListAsync();
        }

        public async Task<int> GetYearMasRecienteAsync()
        {
            return await _context.IndicadoresPorPais
                .MaxAsync(i => (int?)i.Year) ?? DateTime.Now.Year;
        }
    }

    class PaisElegible
    {
        public int Id { get; set; } 
        public string? Nombre { get; set; }
        public string? CodigoIso { get; set; }
        public Dictionary<int, decimal> Indicadores { get; set; } = new Dictionary<int, decimal>();
    }
}
