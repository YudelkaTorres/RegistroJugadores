namespace RegistroJugadores.Dtos;
public record MovimientosResponse(
    int PartidadId,
    int PosicionFila,
    int PosicionColumna,
    string Jugador
    );
