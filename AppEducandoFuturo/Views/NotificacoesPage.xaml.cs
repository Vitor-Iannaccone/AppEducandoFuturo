using AppEducandoFuturo.Data.Repositories;
using AppEducandoFuturo.Models;
using AppEducandoFuturo.Services;

namespace AppEducandoFuturo.Views
{
    public partial class NotificacoesPage : ContentPage
    {
        private readonly NotificacaoService _notificacaoService;
        private readonly AuthService _authService;
        private readonly UsuarioRepository _usuarioRepository;
        private List<Usuario> _usuarios;
        private List<NotificacaoItem> _notificacoes;

        public NotificacoesPage(NotificacaoService notificacaoService,
            AuthService authService,
            UsuarioRepository usuarioRepository)
        {
            InitializeComponent();
            _notificacaoService = notificacaoService;
            _authService = authService;
            _usuarioRepository = usuarioRepository;
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();

            var usuario = _authService.UsuarioLogado;

            // Mostra formulário de envio apenas para Admin
            if (usuario.TipoUsuario == "Administrador")
            {
                FrameEnvio.IsVisible = true;
                await CarregarDestinatarios();
            }

            await CarregarNotificacoes();
        }

        private async Task CarregarDestinatarios()
        {
            _usuarios = await _usuarioRepository.BuscarTodosAsync();
            var nomes = _usuarios.Select(u => $"{u.Nome} ({u.TipoUsuario})").ToList();
            PickerDestinatario.ItemsSource = nomes;
        }

        private async Task CarregarNotificacoes()
        {
            var usuario = _authService.UsuarioLogado;
            var notificacoes = await _notificacaoService.BuscarPorUsuarioAsync(usuario.Id);

            _notificacoes = notificacoes.Select(n => new NotificacaoItem
            {
                Id = n.Id,
                Titulo = n.Titulo,
                Mensagem = n.Mensagem,
                DataFormatada = n.DataEnvio.ToString("dd/MM/yyyy HH:mm"),
                NaoLida = !n.Lida,
                CorFundo = n.Lida ? "#FFFFFF" : "#FFF3E0",
                Notificacao = n
            }).OrderByDescending(n => n.DataFormatada).ToList();

            CollectionNotificacoes.ItemsSource = _notificacoes;
        }

        private async void OnEnviarClicked(object sender, EventArgs e)
        {
            if (PickerDestinatario.SelectedIndex < 0)
            {
                LabelMensagemFeedback.IsVisible = true;
                LabelMensagemFeedback.Text = "Selecione um destinatário.";
                LabelMensagemFeedback.TextColor = Colors.Red;
                return;
            }

            var destinatario = _usuarios[PickerDestinatario.SelectedIndex];
            var (sucesso, mensagem) = await _notificacaoService.EnviarAsync(
                EntryTitulo.Text,
                EditorMensagem.Text,
                destinatario.Id
            );

            LabelMensagemFeedback.IsVisible = true;
            LabelMensagemFeedback.Text = mensagem;
            LabelMensagemFeedback.TextColor = sucesso ? Colors.Green : Colors.Red;

            if (sucesso)
            {
                EntryTitulo.Text = string.Empty;
                EditorMensagem.Text = string.Empty;
                PickerDestinatario.SelectedIndex = -1;
                await CarregarNotificacoes();
            }
        }

        private async void OnNotificacaoTapped(object sender, TappedEventArgs e)
        {
            if (e.Parameter is NotificacaoItem item && item.NaoLida)
            {
                await _notificacaoService.MarcarComoLidaAsync(item.Notificacao);
                await CarregarNotificacoes();
            }
        }

        private async void OnDeletarClicked(object sender, EventArgs e)
        {
            if (sender is Button btn && btn.CommandParameter is int id)
            {
                var item = _notificacoes.FirstOrDefault(n => n.Id == id);
                if (item != null)
                {
                    bool confirmar = await DisplayAlert("Confirmar",
                        "Deseja deletar esta notificação?", "Sim", "Não");
                    if (confirmar)
                    {
                        await _notificacaoService.DeletarAsync(item.Notificacao);
                        await CarregarNotificacoes();
                    }
                }
            }
        }
    }

    // Classe auxiliar para exibir notificações na lista
    public class NotificacaoItem
    {
        public int Id { get; set; }
        public string Titulo { get; set; }
        public string Mensagem { get; set; }
        public string DataFormatada { get; set; }
        public bool NaoLida { get; set; }
        public string CorFundo { get; set; }
        public Notificacao Notificacao { get; set; }
    }
}