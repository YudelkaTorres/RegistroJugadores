using RegistroJugadores.Shared;
using RegistroJugadores.Shared.Dtos;
namespace RegistroJugadores.BlazorWasm.Services;
public interface IMovimientosApiService
{
    Task<Resource<List<MovimientosResponse>>> GetMovimientosAsync(int partidaId);
    Task<Resource<MovimientosResponse>> PostMovimiento(int partidaId, int posicionFila, int posicionColumna, string jugador);
}
