using System.ComponentModel.DataAnnotations;

namespace RegistroJugadores.Models
{
    public class Jugadores
    {
        [Key]
        public int JugadorId { get; set; }

        [Required(ErrorMessage = "El nombre es obligatorio.")]
        public string Nombres { get; set; } = string.Empty;

        [Required(ErrorMessage ="Por favor digitar la cantidad de partidas.")]
        [Range(0, int.MaxValue, ErrorMessage ="La cantidad de partidas debe ser un número positivo")]
        public int Partidas { get; set; }
    }
}
