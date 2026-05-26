using AppEducandoFuturo.Models;
using AppEducandoFuturo.Services;

namespace AppEducandoFuturo.Views.Educador
{
    public partial class GerenciarModulosPage : ContentPage
    {
        private readonly ModuloService _moduloService;
        private readonly AuthService _authService;

        public GerenciarModulosPage(ModuloService moduloService, AuthService authService)
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

        private async void OnSalvarClicked(object sender, EventArgs e)
        {
            var modulo = new Modulo
            {
                Titulo = EntryTitulo.Text,
                Descricao = EntryDescricao.Text,
                Tema = EntryTema.Text,
                EducadorId = _authService.UsuarioLogado.Id
            };

            var (sucesso, mensagem) = await _moduloService.SalvarAsync(modulo);

            LabelMensagem.IsVisible = true;
            LabelMensagem.Text = mensagem;
            LabelMensagem.TextColor = sucesso ? Colors.Green : Colors.Red;

            if (sucesso)
            {
                EntryTitulo.Text = string.Empty;
                EntryDescricao.Text = string.Empty;
                EntryTema.Text = string.Empty;
                await CarregarModulos();
            }
        }

        private async void OnDeletarClicked(object sender, EventArgs e)
        {
            if (sender is Button btn && btn.CommandParameter is Modulo modulo)
            {
                bool confirmar = await DisplayAlert("Confirmar",
                    $"Deseja deletar o módulo '{modulo.Titulo}'?", "Sim", "Não");
                if (confirmar)
                {
                    await _moduloService.DeletarAsync(modulo);
                    await CarregarModulos();
                }
            }
        }
    }
}