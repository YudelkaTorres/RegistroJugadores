using RegistroJugadores.Models;
using Microsoft.EntityFrameworkCore;

namespace RegistroJugadores.DAL;
public class Contexto : DbContext
{
    public Contexto(DbContextOptions<Contexto> options) : base(options){ }
    public DbSet<Jugadores> Jugadores { get; set;}
     public DbSet<Partidas> Partidas { get; set;}
     protected override void OnModelCreating(ModelBuilder modelBuilder)
     {
        modelBuilder.Entity<Partidas>()
           .HasOne(partida => partida.Jugador1)
           .WithMany()
           .HasForeignKey(Partidas => Partidas.Jugador1Id)
           .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Partidas>()
           .HasOne(p => p.Jugador2)
           .WithMany()
           .HasForeignKey(p => p.Jugador2Id)
           .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Partidas>()
           .HasOne(p => p.Ganador)
           .WithMany()
           .HasForeignKey(p => p.GanadorId)
           .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Partidas>()
            .HasOne(p => p.TurnoJugador)
            .WithMany()
            .HasForeignKey(p => p.TurnoJugadorId)
            .OnDelete(DeleteBehavior.Restrict);
     }
}

