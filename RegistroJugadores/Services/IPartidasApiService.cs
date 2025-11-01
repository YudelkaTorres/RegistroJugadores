using RegistroJugadores.Dtos;

namespace RegistroJugadores.Services;
public interface IPartidasApiService
{
    Task<Resource<PartidasResponse>> GetPartidaAsync(int partidaId);
    Task<Resource<PartidasResponse>> PostPartida(int jugador1, int? jugador2);
    Task<Resource<PartidasResponse>> PutPartida(int partidaId, int jugador1, int? jugador2);
}

