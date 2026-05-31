using AppEducandoFuturo.Data.Repositories;
using AppEducandoFuturo.Models;
using AppEducandoFuturo.Services;

namespace AppEducandoFuturo.Views.Aluno
{
    public partial class ConteudoPage : ContentPage
    {
        private readonly AtividadeService _atividadeService;
        private readonly AuthService _authService;
        private readonly ProgressoRepository _progressoRepository;
        private readonly UsuarioRepository _usuarioRepository;
        private Modulo _modulo;

        public ConteudoPage(AtividadeService atividadeService,
            AuthService authService,
            ProgressoRepository progressoRepository,
            UsuarioRepository usuarioRepository)
        {
            InitializeComponent();
            _atividadeService = atividadeService;
            _authService = authService;
            _progressoRepository = progressoRepository;
            _usuarioRepository = usuarioRepository;
        }

        public void CarregarModulo(Modulo modulo)
        {
            _modulo = modulo;
            LabelTitulo.Text = modulo.Titulo;
            LabelTema.Text = modulo.Tema;
            LabelDescricao.Text = modulo.Descricao;
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            if (_modulo != null)
                await CarregarAtividades();
        }

        private async Task CarregarAtividades()
        {
            var usuario = _authService.UsuarioLogado;
            var atividades = await _atividadeService.BuscarPorModuloAsync(_modulo.Id);
            var progresso = await _progressoRepository
                .BuscarPorAlunoEModuloAsync(usuario.Id, _modulo.Id);

            // Busca IDs de atividades já respondidas corretamente
            var atividadesRespondidas = progresso?.AtividadesRespondidas?
                .Split(',', StringSplitOptions.RemoveEmptyEntries)
                .Select(int.Parse).ToList() ?? new List<int>();

            var itens = atividades.Select(a => new AtividadeItem
            {
                Atividade = a,
                Respondida = atividadesRespondidas.Contains(a.Id),
                StatusTexto = atividadesRespondidas.Contains(a.Id)
                    ? "✅ Concluída" : "Toque para responder",
                CorFundo = atividadesRespondidas.Contains(a.Id)
                    ? "#E8F5E9" : "#FFFFFF",
                CorStatus = atividadesRespondidas.Contains(a.Id)
                    ? "#2E7D32" : "#2E7D32"
            }).ToList();

            CollectionAtividades.ItemsSource = itens;

            // Mostra botão concluir se todas as atividades foram respondidas
            bool todasRespondidas = atividades.Any() &&
                atividades.All(a => atividadesRespondidas.Contains(a.Id));
            BtnConcluir.IsVisible = todasRespondidas;

            // Atualiza texto do botão se módulo já foi concluído
            var usuarioAtualizado = await _usuarioRepository.BuscarPorIdAsync(usuario.Id);
            if (todasRespondidas)
                BtnConcluir.Text = "✅ Todas atividades concluídas — Ver módulo concluído";
        }

        private async void OnAtividadeTapped(object sender, TappedEventArgs e)
        {
            if (e.Parameter is AtividadeItem item)
            {
                if (item.Respondida)
                {
                    await DisplayAlert("Atividade concluída",
                        "Você já respondeu esta atividade corretamente!", "OK");
                    return;
                }

                var page = Handler.MauiContext.Services.GetService<AtividadePage>();
                page.CarregarAtividade(item.Atividade);
                await Navigation.PushAsync(page);
            }
        }

        private async void OnConcluirClicked(object sender, EventArgs e)
        {
            await DisplayAlert("Parabéns!",
                $"Você concluiu o módulo '{_modulo.Titulo}'! Continue assim!", "OK");
        }
    }

    public class AtividadeItem
    {
        public Atividade Atividade { get; set; }
        public bool Respondida { get; set; }
        public string StatusTexto { get; set; }
        public string CorFundo { get; set; }
        public string CorStatus { get; set; }
    }
}