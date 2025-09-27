using HorizonFutureVest.BusinessLogic.DTOs;
using HorizonFutureVest.BusinessLogic.Interfaces;
using HorizonFutureVest.Persistence.DbContextHorizon;
using HorizonFutureVest.Persistence.Entities;
using Microsoft.EntityFrameworkCore;

namespace HorizonFutureVest.BusinessLogic.Services;

public class ConfiguracionTasasService(HorizonContext context) : IConfiguracionTasasService
{
    public async Task<ConfiguracionTasasDto> GetConfiguracionTasasAsync()
    {
        var config = await context.ConfiguracionTasas.FirstOrDefaultAsync();

        if (config == null)
        {
            config = new ConfiguracionTasas
            {
                TasaMinima = 2m,
                TasaMaxima = 15m
            };
            context.ConfiguracionTasas.Add(config);
            await context.SaveChangesAsync();
        }

        return new ConfiguracionTasasDto
        {
            Id = config.Id,
            TasaMinima = config.TasaMinima,
            TasaMaxima = config.TasaMaxima
        };
    }

    public async Task<ConfiguracionTasasDto> UpdateConfiguracionTasasAsync(ConfiguracionTasasDto configuracionTasasDto)
    {
        var config = await context.ConfiguracionTasas.FirstOrDefaultAsync();
        if (config == null)
        {
            config = new ConfiguracionTasas();
            context.ConfiguracionTasas.Add(config);
        }

        config.TasaMinima = configuracionTasasDto.TasaMinima;
        config.TasaMaxima = configuracionTasasDto.TasaMaxima;

        await context.SaveChangesAsync();
        return new ConfiguracionTasasDto
        {
            Id = config.Id,
            TasaMinima = config.TasaMinima,
            TasaMaxima = config.TasaMaxima
        };
    }

    public async Task<(decimal TasaMinima, decimal TasaMaxima)> GetTasasAsync()
    {
        var config = await context.ConfiguracionTasas.FirstOrDefaultAsync();
        return (config!.TasaMinima, config.TasaMaxima);
    }
}