using AppEducandoFuturo.Services;

namespace AppEducandoFuturo.Views.Admin
{
    public partial class HomeAdminPage : ContentPage
    {
        private readonly AuthService _authService;

        public HomeAdminPage(AuthService authService)
        {
            InitializeComponent();
            _authService = authService;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            LabelSaudacao.Text = $"Olá, {_authService.UsuarioLogado.Nome}!";
        }

        private async void OnGerenciarUsuariosTapped(object sender, EventArgs e)
        {
            var page = Handler.MauiContext.Services.GetService<GerenciarUsuariosPage>();
            await Navigation.PushAsync(page);
        }

        private async void OnRelatoriosTapped(object sender, EventArgs e)
        {
            var page = Handler.MauiContext.Services.GetService<RelatoriosPage>();
            await Navigation.PushAsync(page);
        }
        private async void OnNotificacoesTapped(object sender, EventArgs e)
        {
            var page = Handler.MauiContext.Services.GetService<NotificacoesPage>();
            await Navigation.PushAsync(page);
        }
        private async void OnSairClicked(object sender, EventArgs e)
        {
            _authService.Logout();
            await Navigation.PopToRootAsync();
        }
    }
}