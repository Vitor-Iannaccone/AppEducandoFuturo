using AppEducandoFuturo.Data.Repositories;
using AppEducandoFuturo.Services;

namespace AppEducandoFuturo.Views.Aluno
{
    public partial class ProgressoPage : ContentPage
    {
        private readonly AuthService _authService;
        private readonly ProgressoRepository _progressoRepository;
        private readonly ModuloRepository _moduloRepository;

        public ProgressoPage(AuthService authService, ProgressoRepository progressoRepository, ModuloRepository moduloRepository)
        {
            InitializeComponent();
            _authService = authService;
            _progressoRepository = progressoRepository;
            _moduloRepository = moduloRepository;
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            await CarregarProgresso();
        }

        private async Task CarregarProgresso()
        {
            var usuario = _authService.UsuarioLogado;
            LabelPontuacaoTotal.Text = usuario.PontuacaoTotal.ToString();

            var progressos = await _progressoRepository.BuscarPorAlunoAsync(usuario.Id);
            var modulos = await _moduloRepository.BuscarTodosAsync();

            var itens = progressos.Select(p =>
            {
                var modulo = modulos.FirstOrDefault(m => m.Id == p.ModuloId);
                return new
                {
                    ModuloNome = modulo?.Titulo ?? "Módulo desconhecido",
                    ProgressoPercent = Math.Min(p.Pontuacao / 100.0, 1.0),
                    PontuacaoTexto = $"{p.Pontuacao} pontos"
                };
            }).ToList();

            CollectionProgresso.ItemsSource = itens;
        }
    }
}