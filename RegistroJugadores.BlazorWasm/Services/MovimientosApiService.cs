using RegistroJugadores.Shared;
using RegistroJugadores.Shared.Dtos;
using System.Net.Http.Json;

namespace RegistroJugadores.BlazorWasm.Services;
public class MovimientosApiService (HttpClient httpClient) : IMovimientosApiService
{
    public async Task<Resource<List<MovimientosResponse>>> GetMovimientosAsync(int partidaId)
    {
        try
        {
            var response = await httpClient.GetFromJsonAsync<List<MovimientosResponse>>($"api/Movimientos/{partidaId}");
            return new Resource<List<MovimientosResponse>>.Success(response ?? []);
        }
        catch (Exception ex)
        {
            return new Resource<List<MovimientosResponse>>.Error(ex.Message);
        }
    }
    public async Task<Resource<MovimientosResponse>> PostMovimiento(int partidaId, int posicionFila, int posicionColumna, string jugador)
    {
        var request = new MovimientosRequest(partidaId, posicionFila, posicionColumna, jugador);
        try
        {
            var response = await httpClient.PostAsJsonAsync("api/Movimientos", request);
            response.EnsureSuccessStatusCode();
            return new Resource<MovimientosResponse>.Success(null!);
        }
        catch (HttpRequestException ex)
        {
            return new Resource<MovimientosResponse>.Error($"Error de red: {ex.Message}");
        }
        catch (NotSupportedException)
        {
            return new Resource<MovimientosResponse>.Error("Respuesta inválida del servidor.");
        }
    }
}
