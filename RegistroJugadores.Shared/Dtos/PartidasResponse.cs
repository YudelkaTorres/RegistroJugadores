using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RegistroJugadores.Shared.Dtos;
public record PartidasResponse(
    int Jugador1Id,
    int? Jugador2Id
    );