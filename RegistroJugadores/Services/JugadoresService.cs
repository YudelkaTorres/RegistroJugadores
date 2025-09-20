using RegistroJugadores.DAL;
using RegistroJugadores.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace RegistroJugadores.Services;
public class JugadoresService
    {
        private readonly IDbContextFactory<Contexto> _dbFactory;
        private readonly ILogger<JugadoresService> _logger;

        public JugadoresService(IDbContextFactory<Contexto> dbFactory, ILogger<JugadoresService> logger)
        {
            _dbFactory = dbFactory;
            _logger = logger;
        }

        public async Task<bool> Guardar(Jugadores jugador)
        {
            try
            {
                bool existe = await ExisteId(jugador.JugadorId);

                if (!existe)
                {
                    if (await ExisteNombre(jugador.Nombres))
                        return false;

                    return await Insertar(jugador);
                }
                else
                {
                    if (await ExisteNombre(jugador.Nombres, jugador.JugadorId))
                        return false;

                    return await Modificar(jugador);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al guardar jugador {JugadorId}", jugador.JugadorId);
                return false;
            }
        }

        private async Task<bool> ExisteId(int jugadorId)
        {
            try
            {
                await using var contexto = await _dbFactory.CreateDbContextAsync();
                return await contexto.Jugadores.AnyAsync(j => j.JugadorId == jugadorId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al verificar existencia del jugador {JugadorId}", jugadorId);
                return false;
            }
        }

        public async Task<bool> ExisteNombre(string nombre, int jugadorId = 0)
        {
            try
            {
                await using var contexto = await _dbFactory.CreateDbContextAsync();
                return await contexto.Jugadores.AnyAsync(j => j.Nombres.ToLower() == nombre.ToLower() && j.JugadorId != jugadorId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al verificar existencia del nombre {Nombre}", nombre);
                return false;
            }
        }

        private async Task<bool> Insertar(Jugadores jugador)
        {
            try
            {
                await using var contexto = await _dbFactory.CreateDbContextAsync();
                contexto.Jugadores.Add(jugador);
                var resultado = await contexto.SaveChangesAsync() > 0;
                _logger.LogInformation("Jugador {JugadorId} insertado correctamente", jugador.JugadorId);
                return resultado;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al insertar jugador {JugadorId}", jugador.JugadorId);
                return false;
            }
        }

        private async Task<bool> Modificar(Jugadores jugador)
        {
            try
            {
                await using var contexto = await _dbFactory.CreateDbContextAsync();
                contexto.Update(jugador);
                var modificado = await contexto.SaveChangesAsync() > 0;
                contexto.Entry(jugador).State = EntityState.Detached;
                _logger.LogInformation("Jugador {JugadorId} modificado correctamente", jugador.JugadorId);
                return modificado;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al modificar jugador {JugadorId}", jugador.JugadorId);
                return false;
            }
        }

        public async Task<Jugadores?> BuscarId(int jugadorId)
        {
            try
            {
                await using var contexto = await _dbFactory.CreateDbContextAsync();
                return await contexto.Jugadores.AsNoTracking().FirstOrDefaultAsync(j => j.JugadorId == jugadorId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al buscar jugador por Id {JugadorId}", jugadorId);
                return null;
            }
        }

        public async Task<Jugadores?> BuscarNombre(string nombre)
        {
            try
            {
                await using var contexto = await _dbFactory.CreateDbContextAsync();
                return await contexto.Jugadores.AsNoTracking().FirstOrDefaultAsync(j => j.Nombres.ToLower() == nombre.ToLower());
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al buscar jugador por nombre {Nombre}", nombre);
                return null;
            }
        }

        public async Task<bool> Eliminar(int jugadorId)
        {
            try
            {
                await using var contexto = await _dbFactory.CreateDbContextAsync();
                var eliminado = await contexto.Jugadores.Where(j => j.JugadorId == jugadorId).ExecuteDeleteAsync() > 0;
                _logger.LogInformation("Jugador {JugadorId} eliminado correctamente", jugadorId);
                return eliminado;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al eliminar jugador {JugadorId}", jugadorId);
                return false;
            }
        }

        public async Task<List<Jugadores>> Listar(Expression<Func<Jugadores, bool>> criterio)
        {
            try
            {
                await using var contexto = await _dbFactory.CreateDbContextAsync();
                return await contexto.Jugadores.Where(criterio).AsNoTracking().ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al listar jugadores");
                return new List<Jugadores>();
            }
        }

        public async Task<List<JugadorEstadistica>> ObtenerEstadisticas()
        {
            using var db = await _dbFactory.CreateDbContextAsync();
            var jugadores = await db.Jugadores.ToListAsync();
            var lista = new List<JugadorEstadistica>();

            foreach (var jugador in jugadores)
            {
                var victorias = await db.Partidas.CountAsync(p => p.GanadorId == jugador.JugadorId);
                var derrotas = await db.Partidas.CountAsync(p =>
                    p.EstadoPartida == "Finalizada" &&
                    p.GanadorId != jugador.JugadorId &&
                    p.GanadorId != null &&
                    (p.Jugador1Id == jugador.JugadorId || p.Jugador2Id == jugador.JugadorId));
                var empates = await db.Partidas.CountAsync(p =>
                    p.EstadoPartida == "Empate" &&
                    (p.Jugador1Id == jugador.JugadorId || p.Jugador2Id == jugador.JugadorId));

                lista.Add(new JugadorEstadistica
                {
                    JugadorId = jugador.JugadorId,
                    Nombres = jugador.Nombres,
                    Victorias = victorias,
                    Derrotas = derrotas,
                    Empates = empates
                });
            }

            return lista;
        }

        public class JugadorEstadistica
        {
            public int JugadorId { get; set; }
            public string Nombres { get; set; } = string.Empty;
            public int Victorias { get; set; }
            public int Derrotas { get; set; }
            public int Empates { get; set; }
        }

        public async Task<Partidas?> ObtenerPartidaConMovimientos(int partidaId)
        {
            using var context = _dbFactory.CreateDbContext();
            return await context.Partidas
                .Include(p => p.Movimientos)
                .FirstOrDefaultAsync(p => p.PartidaId == partidaId);
        }

}

