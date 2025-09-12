using RegistroJugadores.DAL;
using RegistroJugadores.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace RegistroJugadores.Services;
public class PartidasService
{
    private readonly IDbContextFactory<Contexto> DbFactory;
    private readonly ILogger<PartidasService> _Logger;
    public PartidasService(IDbContextFactory<Contexto> dbFactory, ILogger<PartidasService> logger)
    {
        DbFactory = dbFactory;
        _Logger = logger;
    }
    public async Task<bool> Guardar(Partidas partida)
    {
        if (!await Existe(partida.PartidaId))
        {
            _Logger.LogInformation("Insertando nueva partida para Jugador1: {Jugador}", partida.Jugador1?.Nombres);
            return await Insertar(partida);
        }
        else
        {
            _Logger.LogInformation("Modificando partida con Id {PartidaId}", partida.PartidaId);
            return await Modificar(partida);
        }  
    }
    public async Task<bool> Existe(int partidaId)
    {
        await using var contexto = await DbFactory.CreateDbContextAsync();
        _Logger.LogInformation("Verificando existencia de la partida con Id {PartidaId}", partidaId);
        return await contexto.Partidas.AnyAsync(partida => partida.PartidaId == partidaId);
    }
    public async Task<bool> Insertar(Partidas partida)
    {
        await using var contexto = await DbFactory.CreateDbContextAsync();
        contexto.Partidas.Add(partida);
        _Logger.LogInformation("Partida preparada para inserción.");
        return await contexto.SaveChangesAsync() > 0;
    }
    public async Task<bool> Modificar(Partidas partida)
    {
        await using var contexto = await DbFactory.CreateDbContextAsync();
        contexto.Update(partida);
        _Logger.LogInformation("Partida con Id {PartidaId} preparada para modificación.", partida.PartidaId);
        return await contexto.SaveChangesAsync() > 0;
    }
    public async Task<Partidas?> Buscar(int partidaId)
    {
        await using var contexto = await DbFactory.CreateDbContextAsync();
        _Logger.LogInformation("Buscando partida con Id {PartidaId}", partidaId);

        var partida = await contexto.Partidas
            .Include(partida => partida.Jugador1)
            .Include(partida => partida.Jugador2)
            .Include(partida => partida.Ganador)
            .Include(partida => partida.TurnoJugador)
            .FirstOrDefaultAsync(partida => partida.PartidaId == partidaId);

        if (partida == null)
            _Logger.LogWarning("No se encontró la partida con Id {PartidaId}", partidaId);

        return partida;
    }
    public async Task<bool> Eliminar(int partidaId)
    {
        await using var contexto = await DbFactory.CreateDbContextAsync();
        var partida = await contexto.Partidas.FindAsync(partidaId);

        if (partida == null)
        {
            _Logger.LogWarning("No se encontró la partida con Id {PartidaId} para eliminar.", partidaId);
            return false;
        }

        contexto.Partidas.Remove(partida);
        _Logger.LogInformation("Partida con Id {PartidaId} preparada para eliminación.", partidaId);
        return await contexto.SaveChangesAsync() > 0;
    }

    public async Task<List<Partidas>> Listar(Expression<Func<Partidas, bool>> criterio)
    {
        await using var contexto = await DbFactory.CreateDbContextAsync();
        _Logger.LogInformation("Listando partidas con criterio {Criterio}", criterio);

        var lista = await contexto.Partidas
            .Include(partida => partida.Jugador1)
            .Include(partida => partida.Jugador2)
            .Include(partida => partida.Ganador)
            .Include(partida => partida.TurnoJugador)
            .Where(criterio)
            .AsNoTracking()
            .ToListAsync();

        if (lista.Count == 0)
            _Logger.LogWarning("La consulta de partidas no devolvió resultados.");
        else
            _Logger.LogInformation("La consulta de partidas devolvió {Cantidad} resultados.", lista.Count);

        return lista;
    }
}

