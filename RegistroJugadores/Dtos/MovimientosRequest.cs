namespace RegistroJugadores.Dtos;
public record MovimientosRequest(
    int PartidaId,
    int PosicionFila,
    int PosicionColumna,
    string Jugador
    );
