using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
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

        [Required(ErrorMessage ="Por favor digitar la cantidad de Victorias.")]
        [Range(0, int.MaxValue, ErrorMessage ="La cantidad de Victorias debe ser un número positivo")]
        public int Victorias { get; set; } = 0;
        public int Derrotas { get; set; } = 0;
        public int Empates { get; set; } = 0;
        [InverseProperty(nameof(Models.Movimientos.Jugador))]
        public virtual ICollection<Movimientos> Movimientos { get; set; } = new List<Movimientos>();
}

