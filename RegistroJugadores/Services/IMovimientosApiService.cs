using RegistroJugadores.Dtos;

namespace RegistroJugadores.Services;
public interface IMovimientosApiService
{
    Task<Resource<List<MovimientosResponse>>> GetMovimientosAsync(int partidaId);
    Task<Resource<MovimientosResponse>> PostMovimiento(int partidaId, int posicionFila, int posicionColumna, string jugador);
}
