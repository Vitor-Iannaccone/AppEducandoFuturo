using AppEducandoFuturo.Models;
using AppEducandoFuturo.Services;

namespace AppEducandoFuturo.Views.Aluno
{
    public partial class ModulosPage : ContentPage
    {
        private readonly ModuloService _moduloService;
        private readonly AuthService _authService;

        public ModulosPage(ModuloService moduloService, AuthService authService)
        {
            InitializeComponent();
            _moduloService = moduloService;
            _authService = authService;
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            await CarregarModulos();
        }

        private async Task CarregarModulos()
        {
            var modulos = await _moduloService.BuscarTodosAsync();
            CollectionModulos.ItemsSource = modulos;
        }

        private async void OnModuloTapped(object sender, TappedEventArgs e)
        {
            if (e.Parameter is Modulo modulo)
            {
                var page = Handler.MauiContext.Services.GetService<ConteudoPage>();
                page.CarregarModulo(modulo);
                await Navigation.PushAsync(page);
            }
        }
    }
}