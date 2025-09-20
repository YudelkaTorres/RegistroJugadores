using RegistroJugadores.Components;
using RegistroJugadores.DAL;
using Microsoft.EntityFrameworkCore;
using RegistroJugadores.Services;

namespace RegistroJugadores
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddRazorComponents()
                .AddInteractiveServerComponents();

            var ConnectionString = builder.Configuration.GetConnectionString("SqlConStr");

            // Usar AddDbContextFactory para que JugadoresService reciba IDbContextFactory<Contexto>
            builder.Services.AddDbContextFactory<Contexto>(options =>
                options.UseSqlServer(ConnectionString, sqlOptions =>
                    sqlOptions.EnableRetryOnFailure()));

            builder.Services.AddScoped<JugadoresService>();
            builder.Services.AddScoped<PartidasService>();

            builder.Services.AddBlazorBootstrap();

            var app = builder.Build();

            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseAntiforgery();

            app.MapRazorComponents<App>()
                .AddInteractiveServerRenderMode();

            app.Run();
        }
    }
}
