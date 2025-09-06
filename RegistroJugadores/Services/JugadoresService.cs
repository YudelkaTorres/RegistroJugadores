using RegistroJugadores.DAL;
using RegistroJugadores.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using System.Security.AccessControl;

namespace RegistroJugadores.Services
{
    public class JugadoresService(IDbContextFactory<Contexto> DbFactory)
    {
        public async Task<bool> Guardar(Jugadores jugador)
        {
            try
            {
                bool existe = await ExisteId(jugador.JugadorId);
                if (!existe)
                {
                    if (await ExisteNombre(jugador.Nombres))
                    {
                        return false;
                    }
                    return await Insertar(jugador);
                }
                else
                {
                    if (await ExisteNombre(jugador.Nombres, jugador.JugadorId))
                    {
                        return false;
                    }
                    return await Modificar(jugador);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al guardar el jugador: {ex.Message}");
                return false;
            }
        }

        private async Task<bool> ExisteId(int jugadorId)
        {
            await using var contexto = await DbFactory.CreateDbContextAsync();
            return await contexto.Jugadores
                .AnyAsync(j => j.JugadorId == jugadorId);
        }

        public async Task<bool> ExisteNombre(string nombre, int jugadorId = 0)
        {
            await using var contexto = await DbFactory.CreateDbContextAsync();
            return await contexto.Jugadores
                 .AnyAsync(j => j.Nombres.ToLower() == nombre.ToLower() && j.JugadorId != jugadorId);

        }

        private async Task<bool> Insertar(Jugadores jugador)
        {
            await using var contexto = await DbFactory.CreateDbContextAsync();
            contexto.Jugadores.Add(jugador);
            return await contexto.SaveChangesAsync() > 0;
        }

        private async Task<bool> Modificar(Jugadores jugador)
        {
            await using var contexto = await DbFactory.CreateDbContextAsync();
            contexto.Update(jugador);
            var modificado = await contexto.SaveChangesAsync() > 0;
            contexto.Entry(jugador).State = EntityState.Detached;
            return modificado;
        }

        public async Task<Jugadores?> BuscarId(int jugadorId)
        {
            await using var contexto = await DbFactory.CreateDbContextAsync();
            return await contexto.Jugadores
                .AsNoTracking()
                .FirstOrDefaultAsync(j => j.JugadorId == jugadorId);
        }

        public async Task<Jugadores?> BuscarNombre(string nombre)
        {
            await using var contexto = await DbFactory.CreateDbContextAsync();
            return await contexto.Jugadores
                .AsNoTracking()
                .FirstOrDefaultAsync(j => j.Nombres.ToLower() == nombre.ToLower());
        }

        public async Task<bool> Eliminar(int jugadorId)
        {
            await using var contexto = await DbFactory.CreateDbContextAsync();
            return await contexto .Jugadores
                .Where(j => j.JugadorId == jugadorId)
                .ExecuteDeleteAsync() > 0;
        }

        public async Task<List<Jugadores>> Listar(Expression<Func<Jugadores, bool>> criterio)
        {
            await using var contexto = await DbFactory.CreateDbContextAsync();
            return await contexto.Jugadores
                .Where(criterio)
                .AsNoTracking()
                .ToListAsync();
        }
    }
}
