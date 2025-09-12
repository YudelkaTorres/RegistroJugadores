using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace RegistroJugadores.Models;

    public class Partidas
    {
        [Key]
        public int PartidaId { get; set; }

        public int Jugador1Id { get; set; }
        public int? Jugador2Id { get; set; }

        [Required(ErrorMessage = "Debe seleccionar el estado de la partida")]
        [StringLength(20)]
        public string EstadoPartida { get; set; } = null!;

        public int? GanadorId { get; set; }
        public int TurnoJugadorId { get; set; }

    [StringLength(9)]
    public string EstadoTablero { get; set; } = "3x3";

        public DateTime FechaInicio { get; set; } = DateTime.UtcNow;
        public DateTime? FechaFin { get; set; }

        // Propiedades de navegación
        [ForeignKey(nameof(Jugador1Id))]
        public virtual Jugadores Jugador1 { get; set; } = null!;

        [ForeignKey(nameof(Jugador2Id))]
        public virtual Jugadores? Jugador2 { get; set; }

        [ForeignKey(nameof(GanadorId))]
        public virtual Jugadores? Ganador { get; set; } 

        [ForeignKey(nameof(TurnoJugadorId))]
        public virtual Jugadores? TurnoJugador { get; set; } 
    }

