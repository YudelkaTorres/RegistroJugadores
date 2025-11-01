using RegistroJugadores.Shared;
using RegistroJugadores.Shared.Dtos;
namespace RegistroJugadores.BlazorWasm.Services;
public interface IPartidasApiService
{
    Task<Resource<List<PartidasResponse>>> GetPartidasAsync();
    Task<Resource<PartidasResponse>> GetPartidaAsync(int partidaId);
    Task<Resource<PartidasResponse>> PostPartida(int jugador1, int? jugador2);
    Task<Resource<PartidasResponse>> PutPartida (int partidaId, int jugador1, int? jugador2);
}
