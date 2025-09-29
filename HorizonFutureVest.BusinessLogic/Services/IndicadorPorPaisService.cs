using HorizonFutureVest.BusinessLogic.DTOs;
using HorizonFutureVest.BusinessLogic.Interfaces;
using HorizonFutureVest.Persistence.DbContextHorizon;
using HorizonFutureVest.Persistence.Entities;
using Microsoft.EntityFrameworkCore;

namespace HorizonFutureVest.BusinessLogic.Services;

public class IndicadorPorPaisService(HorizonContext context) : IIndicadorPorPaisService 
{

    public async Task<IEnumerable<IndicadorPorPaisDto>> GetAllAsync()
    {
        var indicadores = await context.IndicadoresPorPais
            .Include(i => i.Pais)
            .Include(i => i.Macroindicador)
            .OrderBy(i => i.Pais.Nombre)
            .ThenBy(i => i.Year)
            .ThenBy(i => i.Macroindicador.Nombre)
            .ToListAsync();
        
        return MapToDto(indicadores);
    }

    public async Task<IEnumerable<IndicadorPorPaisDto>> GetByFiltersAsync(int? paisId = null, int? year = null)
    {
        var query =  context.IndicadoresPorPais
            .Include(i => i.Pais)
            .Include(i => i.Macroindicador)
            .AsQueryable();
        
        if(paisId.HasValue)
            query = query.Where(i => i.PaisId == paisId.Value);
        
        if(year.HasValue)
            query = query.Where(y => year == year.Value);

        var indicadores = await query
            .OrderBy(i => i.Pais.Nombre)
            .ThenBy(i => i.Year)
            .ThenBy(i => i.Macroindicador.Nombre)
            .ToListAsync();
        
        return MapToDto(indicadores); 
    }

    public async Task<IndicadorPorPaisDto> GetByIdAsync(int id)
    {
        var indicador = await context.IndicadoresPorPais
            .Include(i => i.Pais)
            .Include(i => i.Macroindicador)
            .FirstOrDefaultAsync(i => i.Id == id);

        if (indicador == null) return null!;
        
        return MapToDto([indicador]).First(); 
    }

    public async Task<IndicadorPorPaisDto> CreateAsync(IndicadorPorPaisDto indicadorDto)
    {
        var indicador = new IndicadorPorPais
        {
            PaisId = indicadorDto.PaisId,
            MacroindicadorId = indicadorDto.MacroindicadorId,
            Valor = indicadorDto.Valor,
            Year = indicadorDto.Year
        };
        
        context.IndicadoresPorPais.Add(indicador);
        await context.SaveChangesAsync();
        
        await context.Entry(indicador)
            .Reference(i => i.Pais)
            .LoadAsync();
        await context.Entry(indicador)
            .Reference(i => i.Macroindicador)
            .LoadAsync();
        
        return MapToDto([indicador]).First();
    }

    public async Task<IndicadorPorPaisDto> UpdateAsync(IndicadorPorPaisDto indicadorDto)
    {
       var indicador = await context.IndicadoresPorPais
           .Include(i => i.Pais)
           .Include(i => i.Macroindicador)
           .FirstOrDefaultAsync(i => i.Id == indicadorDto.Id);
       
       return MapToDto(new []{indicador}!).First();
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var indicador = await context.IndicadoresPorPais.FindAsync(id);
            if(indicador == null) return false;
            
            context.IndicadoresPorPais.Remove(indicador);
            await context.SaveChangesAsync();
            return true;
    }

    public async Task<bool> ExistsAsync(int id)
    {
        return await context.IndicadoresPorPais.AnyAsync(i => i.Id == id);
    }

    public async Task<bool> ExisteCombinacionAsync(int paisId, int macroindicadorId, int year, int? excludeId = null)
    {
    var query = context.IndicadoresPorPais
        .Where(i => i.PaisId == paisId && i.MacroindicadorId == macroindicadorId && i.Year == year);
    
    if (excludeId.HasValue)
        query = query.Where(i => i.Id == excludeId.Value);
    
    return await query.AnyAsync();
    }

    public async Task<IEnumerable<int>> GetYearsAvalibleAsync()
    {
        return await context.IndicadoresPorPais
            .Select(i => i.Year)
            .Distinct()
            .OrderByDescending(a => a)
            .ToListAsync();
    }

    public async Task<IEnumerable<IndicadorPorPaisDto>> GetIndicadoresPorPaisYearAsync(int paisId, int year)
    {
        var indicadores = await context.IndicadoresPorPais
            .Include(i => i.Pais)
            .Include(i => i.Macroindicador)
            .Where(i => i.PaisId == paisId && i.Year == year)
            .ToListAsync();
        
        return MapToDto(indicadores);
    }

    private IEnumerable<IndicadorPorPaisDto> MapToDto(IEnumerable<IndicadorPorPais> indicadores)
    {
        return indicadores.Select(i => new IndicadorPorPaisDto
        {
            Id = i.Id,
            PaisId = i.PaisId,
            NombrePais = i.Pais.Nombre,
            MacroindicadorId = i.MacroindicadorId,
            NombreMacroindicador = i.Macroindicador.Nombre,
            Valor = i.Valor,
            Year = i.Year
        });
    }
}