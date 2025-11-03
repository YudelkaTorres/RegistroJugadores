namespace RegistroJugadores.Dtos;
public record MovimientosResponse(
    int PartidaId,
    int PosicionFila,
    int PosicionColumna,
    string Jugador
    );
