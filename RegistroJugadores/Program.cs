using RegistroJugadores.Components;
using RegistroJugadores.DAL;
using Microsoft.EntityFrameworkCore;
using RegistroJugadores.Services;
using BootstrapBlazor.Components;

namespace RegistroJugadores
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddRazorComponents()
                .AddInteractiveServerComponents();

            //Obtenemos el ConStr para usarlo en el contexto
            var ConStr = builder.Configuration.GetConnectionString("ConStr");

            //Agregamos el contexto al builder con el ConStr
            builder.Services.AddDbContextFactory<Contexto>(Options => Options.UseSqlite(ConStr));

            //Injeccion del service
            builder.Services.AddScoped<JugadoresService>();

            //Injeccion del servicio de Bootstrap
            builder.Services.AddBootstrapBlazor();
            builder.Services.AddScoped<ToastService>();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
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
