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
            if (string.IsNullOrWhiteSpace(jugador.Nombres))
                throw new ArgumentException("El nombre es obligatorio.");

            if (jugador.Partidas < 0)
                throw new ArgumentException("El número de partidas no puede ser menor que cero");

            if (!await ExisteId(jugador.JugadorId))
            {
                if (await ExisteNombre(0, jugador.Nombres))
                    throw new InvalidOperationException("Ya existe un jugador con ese nombre.");

                return await Insertar(jugador);
            }
            else
            {
                if (await ExisteNombre(jugador.JugadorId, jugador.Nombres))
                    throw new InvalidOperationException("Ya existe un jugador con ese nombre.");

                return await Modificar(jugador);
            }
       
        }

        private async Task<bool> ExisteId(int jugadorId)
        {
            await using var contexto = await DbFactory.CreateDbContextAsync();
            return await contexto.Jugadores
                .AnyAsync(j => j.JugadorId == jugadorId);
        }

        public async Task<bool> ExisteNombre(int jugadorId, string nombre)
        {
            await using var contexto = await DbFactory.CreateDbContextAsync();
            return await contexto.Jugadores
				.AnyAsync(j => j.Nombres.ToLower() == nombre.ToLower()
							   && j.JugadorId != jugadorId);
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
