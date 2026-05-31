using AppEducandoFuturo.Services;

namespace AppEducandoFuturo.Views.Auth
{
    public partial class LoginPage : ContentPage
    {
        private readonly AuthService _authService;

        public LoginPage(AuthService authService)
        {
            InitializeComponent();
            _authService = authService;
        }

        // ── BOTÃO ENTRAR ──────────────────────────────────────────────
        private async void OnLoginClicked(object sender, EventArgs e)
        {
            var (sucesso, mensagem) = await _authService.LoginAsync(
                EntryEmail.Text,
                EntrySenha.Text
            );

            LabelMensagem.IsVisible = true;
            LabelMensagem.Text = mensagem;

            if (!sucesso)
            {
                LabelMensagem.TextColor = Colors.Red;
                return;
            }

            LabelMensagem.TextColor = Colors.Green;

            var usuario = _authService.UsuarioLogado;

            if (usuario.TipoUsuario == "Aluno")
            {
                var homePage = Handler.MauiContext.Services.GetService<Views.Aluno.HomePage>();
                await Navigation.PushAsync(homePage);
            }
            else if (usuario.TipoUsuario == "Educador")
            {
                var homePage = Handler.MauiContext.Services.GetService<Views.Educador.HomeEducadorPage>();
                await Navigation.PushAsync(homePage);
            }
            else if (usuario.TipoUsuario == "Administrador")
            {
                var homePage = Handler.MauiContext.Services.GetService<Views.Admin.HomeAdminPage>();
                await Navigation.PushAsync(homePage);
            }

            // Sucesso: redireciona conforme o tipo de usuário
            // Por enquanto exibe mensagem de sucesso
            LabelMensagem.TextColor = Colors.Green;
        }

        // ── LINK CADASTRO ─────────────────────────────────────────────
        private async void OnCadastroTapped(object sender, EventArgs e)
        {
            var cadastroPage = Handler.MauiContext.Services.GetService<Views.Auth.CadastroPage>();
            await Navigation.PushAsync(cadastroPage);
        }
        protected override void OnAppearing()
        {
            base.OnAppearing();
            EntryEmail.Text = string.Empty;
            EntrySenha.Text = string.Empty;
            LabelMensagem.IsVisible = false;
        }
    }
}