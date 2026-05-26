using AppEducandoFuturo.Models;
using AppEducandoFuturo.Data.Repositories;
using AppEducandoFuturo.Services;

namespace AppEducandoFuturo.Views.Admin
{
    public partial class GerenciarUsuariosPage : ContentPage
    {
        private readonly UsuarioRepository _usuarioRepository;
        private readonly AuthService _authService;

        public GerenciarUsuariosPage(UsuarioRepository usuarioRepository, AuthService authService)
        {
            InitializeComponent();
            _usuarioRepository = usuarioRepository;
            _authService = authService;
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            await CarregarUsuarios();
        }

        private async Task CarregarUsuarios()
        {
            var usuarios = await _usuarioRepository.BuscarTodosAsync();
            // Não exibe o próprio admin logado na lista
            var lista = usuarios.Where(u => u.Id != _authService.UsuarioLogado.Id).ToList();
            CollectionUsuarios.ItemsSource = lista;
        }

        private async void OnDeletarClicked(object sender, EventArgs e)
        {
            if (sender is Button btn && btn.CommandParameter is Usuario usuario)
            {
                bool confirmar = await DisplayAlert("Confirmar",
                    $"Deseja remover o usuário '{usuario.Nome}'?", "Sim", "Não");
                if (confirmar)
                {
                    await _usuarioRepository.DeletarAsync(usuario);
                    await CarregarUsuarios();
                }
            }
        }
    }
}