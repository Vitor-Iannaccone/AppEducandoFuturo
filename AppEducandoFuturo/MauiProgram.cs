using AppEducandoFuturo.Data;
using AppEducandoFuturo.Data.Repositories;
using AppEducandoFuturo.Services;
using Microsoft.Extensions.Logging;

namespace AppEducandoFuturo
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();

            builder
                .UseMauiApp<App>()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                });

            // ── Repositories ─────────────────────────────────────────
            builder.Services.AddSingleton<UsuarioRepository>();
            builder.Services.AddSingleton<ModuloRepository>();
            builder.Services.AddSingleton<ConteudoRepository>();
            builder.Services.AddSingleton<AtividadeRepository>();
            builder.Services.AddSingleton<ProgressoRepository>();
            builder.Services.AddSingleton<NotificacaoRepository>();

            // ── Services ─────────────────────────────────────────────
            builder.Services.AddSingleton<AuthService>();
            builder.Services.AddSingleton<ModuloService>();
            builder.Services.AddSingleton<AtividadeService>();
            builder.Services.AddSingleton<NotificacaoService>();

            // ── Views ────────────────────────────────────────────────
            builder.Services.AddTransient<Views.Auth.LoginPage>();
            builder.Services.AddTransient<Views.Auth.CadastroPage>();
            builder.Services.AddTransient<Views.Aluno.HomePage>();
            builder.Services.AddTransient<Views.Aluno.ModulosPage>();
            builder.Services.AddTransient<Views.Aluno.ProgressoPage>();
            builder.Services.AddTransient<Views.Aluno.ConteudoPage>();
            builder.Services.AddTransient<Views.Aluno.AtividadePage>();
            builder.Services.AddTransient<Views.Educador.HomeEducadorPage>();
            builder.Services.AddTransient<Views.Educador.GerenciarModulosPage>();
            builder.Services.AddTransient<Views.Educador.InserirAtividadePage>();
            builder.Services.AddTransient<Views.Admin.HomeAdminPage>();
            builder.Services.AddTransient<Views.Admin.GerenciarUsuariosPage>();
            builder.Services.AddTransient<Views.Admin.RelatoriosPage>();
            builder.Services.AddTransient<Views.NotificacoesPage>();

            // ── Shell ────────────────────────────────────────────────
            builder.Services.AddSingleton<AppShell>();

#if DEBUG
            builder.Logging.AddDebug();
#endif

            // ── Build Admin  ───────────────────────────
            var app = builder.Build();

            Task.Run(async () =>
            {
                var usuarioRepo = app.Services.GetService<UsuarioRepository>();
                var authService = app.Services.GetService<AuthService>();

                var adminExistente = await usuarioRepo.BuscarPorEmailAsync("admin@admin.com");
                if (adminExistente == null)
                {
                    await authService.CadastrarAsync(
                        "Administrador",
                        "admin@admin.com",
                        "admin123",
                        "Administrador"
                    );
                }
            }).Wait();

            return builder.Build();
        }
    }
}