using AppEducandoFuturo.Models;
using AppEducandoFuturo.Services;

namespace AppEducandoFuturo.Views.Aluno
{
    public partial class AtividadePage : ContentPage
    {
        private readonly AtividadeService _atividadeService;
        private readonly AuthService _authService;
        private Atividade _atividade;

        public AtividadePage(AtividadeService atividadeService, AuthService authService)
        {
            InitializeComponent();
            _atividadeService = atividadeService;
            _authService = authService;
        }

        public void CarregarAtividade(Atividade atividade)
        {
            _atividade = atividade;
            LabelPergunta.Text = atividade.Pergunta;

            // Gera os botões de opção dinamicamente
            StackOpcoes.Children.Clear();
            var opcoes = atividade.Opcoes?.Split('|') ?? new[] { atividade.RespostaCorreta };

            foreach (var opcao in opcoes)
            {
                var btn = new Button
                {
                    Text = opcao.Trim(),
                    BackgroundColor = Color.FromArgb("#E8F5E9"),
                    TextColor = Color.FromArgb("#212121"),
                    CornerRadius = 8,
                    HeightRequest = 50
                };
                btn.Clicked += async (s, e) => await OnOpcaoClicked(opcao.Trim());
                StackOpcoes.Children.Add(btn);
            }
        }

        private async Task OnOpcaoClicked(string resposta)
        {
            var usuario = _authService.UsuarioLogado;
            var (correto, mensagem, pontos) = await _atividadeService.ResponderAsync(
                usuario.Id, _atividade, resposta);

            // Atualiza o usuário na sessão com os dados mais recentes do banco
            await _authService.AtualizarUsuarioLogadoAsync();

            // Desabilita todos os botões após responder
            foreach (var child in StackOpcoes.Children)
            {
                if (child is Button btn)
                    btn.IsEnabled = false;
            }

            // Exibe feedback
            FrameFeedback.IsVisible = true;
            FrameFeedback.BackgroundColor = correto
                ? Color.FromArgb("#E8F5E9")
                : Color.FromArgb("#FFEBEE");
            LabelFeedback.Text = mensagem;
            LabelFeedback.TextColor = correto
                ? Color.FromArgb("#2E7D32")
                : Color.FromArgb("#D32F2F");

            BtnProxima.IsVisible = true;
        }

        private async void OnProximaClicked(object sender, EventArgs e)
        {
            await Navigation.PopAsync();
        }
    }
}