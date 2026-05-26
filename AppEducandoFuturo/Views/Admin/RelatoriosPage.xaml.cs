using AppEducandoFuturo.Data.Repositories;

namespace AppEducandoFuturo.Views.Admin
{
    public partial class RelatoriosPage : ContentPage
    {
        private readonly UsuarioRepository _usuarioRepository;

        public RelatoriosPage(UsuarioRepository usuarioRepository)
        {
            InitializeComponent();
            _usuarioRepository = usuarioRepository;
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            await CarregarRelatorio();
        }

        private async Task CarregarRelatorio()
        {
            var usuarios = await _usuarioRepository.BuscarTodosAsync();
            var alunos = usuarios.Where(u => u.TipoUsuario == "Aluno").ToList();

            var itens = alunos.Select(a => new
            {
                a.Nome,
                a.Email,
                PontuacaoTexto = $"{a.PontuacaoTotal} pontos",
                ModulosTexto = $"{a.ModulosConcluidos} módulos concluídos"
            }).OrderByDescending(a => a.PontuacaoTexto).ToList();

            CollectionRelatorio.ItemsSource = itens;
        }
    }
}