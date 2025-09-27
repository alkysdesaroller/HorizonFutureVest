using HorizonFutureVest.BusinessLogic.DTOs;
using HorizonFutureVest.BusinessLogic.Interfaces;
using HorizonFutureVest.Persistence.DbContextHorizon;
using HorizonFutureVest.Persistence.Entities;
using Microsoft.EntityFrameworkCore;

namespace HorizonFutureVest.BusinessLogic.Services;

public class PaisService(HorizonContext context) : IPaisService
{
    public async Task<IEnumerable<PaisDto>> GetAllPaisesAsync()
    {
        var pais = await context.Paises
            .OrderBy(p => p.Nombre)
            .ToListAsync();

        return pais.Select(p => new PaisDto
        {
            Id = p.Id,
            Nombre = p.Nombre,
            CodigoIso = p.CodigoIso
        });
    }

    public async Task<PaisDto> GetPaisByIdAsync(int id)
    {
        var pais = await context.Paises.FindAsync(id);
        if (pais == null) return null!;

        return new PaisDto
        {
            Id = pais.Id,
            Nombre = pais.Nombre,
            CodigoIso = pais.CodigoIso
        };
    }

    public async Task<PaisDto> CreatePaisAsync(PaisDto paisDto)
    {
        var pais = new Pais
        {
            Nombre = paisDto.Nombre,
            CodigoIso = paisDto.CodigoIso.ToUpper()
        };
        context.Paises.Add(pais);
        await context.SaveChangesAsync();

        paisDto.Id = pais.Id;
        return paisDto;
    }

    public async Task<PaisDto> UpdatePaisAsync(PaisDto paisDto)
    {
        var pais = await context.Paises.FindAsync(paisDto.Id);
        if (pais == null) return null!;

        pais.Nombre = paisDto.Nombre;
        pais.CodigoIso = paisDto.CodigoIso.ToUpper();

        await context.SaveChangesAsync();
        return paisDto;
    }

    public async Task<bool> DeletePaisAsync(int id)
    {
        var pais = await context.Paises.FindAsync(id);
        if (pais == null) return false;

        context.Paises.Remove(pais);
        await context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> PaisExistsAsync(int id)
    {
        return await context.Paises.AnyAsync(p => p.Id == id);
    }

    public async Task<bool> CodigoIsoExistsAsync(string codigoIso, int? excludeId = null)
    {
        var query = context.Paises.Where(p => p.CodigoIso.ToUpper() == codigoIso.ToUpper());
        if (excludeId.HasValue)
            query = query.Where(p => p.Id != excludeId.Value);
        
        return await query.AnyAsync();
    }
}
