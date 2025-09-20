using RegistroJugadores.Models;
using Microsoft.EntityFrameworkCore;

namespace RegistroJugadores.DAL;
public class Contexto : DbContext
{
    public Contexto(DbContextOptions<Contexto> options) : base(options){ }
    public DbSet<Jugadores> Jugadores { get; set;}
    public DbSet<Partidas> Partidas { get; set;}
    public DbSet<Movimientos> Movimientos { get; set; }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Partidas>()
            .HasMany(p => p.Movimientos)
            .WithOne(m => m.Partida)
            .HasForeignKey(m => m.PartidaId)
            .OnDelete(DeleteBehavior.Cascade); //Borra movimientos si se elimina la partida
    }
}

