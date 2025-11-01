using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RegistroJugadores.Shared.Dtos;
public record MovimientosRequest(
    int PartidaId,
    int PosicionFila,
    int PosicionColumna,
    string Jugador
    );