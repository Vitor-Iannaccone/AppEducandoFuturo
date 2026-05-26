using AppEducandoFuturo.Services;

namespace AppEducandoFuturo.Views.Educador
{
    public partial class HomeEducadorPage : ContentPage
    {
        private readonly AuthService _authService;

        public HomeEducadorPage(AuthService authService)
        {
            InitializeComponent();
            _authService = authService;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            LabelSaudacao.Text = $"Olá, {_authService.UsuarioLogado.Nome}!";
        }

        private async void OnGerenciarModulosTapped(object sender, EventArgs e)
        {
            var page = Handler.MauiContext.Services.GetService<GerenciarModulosPage>();
            await Navigation.PushAsync(page);
        }

        private async void OnInserirAtividadeTapped(object sender, EventArgs e)
        {
            var page = Handler.MauiContext.Services.GetService<InserirAtividadePage>();
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