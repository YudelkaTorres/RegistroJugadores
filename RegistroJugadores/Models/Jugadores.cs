using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace RegistroJugadores.Models;

    [Index(nameof(Nombres), IsUnique = true)]
    public class Jugadores
    {
        [Key]
        public int JugadorId { get; set; }

        [RegularExpression(@"^[a-zA-Z\s]+$", ErrorMessage = "No se permiten caracteres especiales")]
        [Required(ErrorMessage = "El nombre es obligatorio.")]

        [MaxLength(100)]
        public string Nombres { get; set; } = string.Empty;

        [Required(ErrorMessage ="Por favor digitar la cantidad de partidas.")]
        [Range(0, int.MaxValue, ErrorMessage ="La cantidad de partidas debe ser un número positivo")]
        public int Partidas { get; set; }
    }

