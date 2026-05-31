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

            await _authService.AtualizarUsuarioLogadoAsync();

            if (!correto)
            {
                // Pergunta se deseja tentar novamente
                bool tentarNovamente = await DisplayAlert(
                    "Resposta incorreta ❌",
                    "Sua resposta está incorreta. Deseja tentar novamente?",
                    "Sim", "Não");

                if (tentarNovamente)
                    return; // Mantém os botões ativos para tentar novamente

                // Não quer tentar — exibe feedback e encerra
                FrameFeedback.IsVisible = true;
                FrameFeedback.BackgroundColor = Color.FromArgb("#FFEBEE");
                LabelFeedback.Text = $"❌ {mensagem}";
                LabelFeedback.TextColor = Color.FromArgb("#D32F2F");
                BtnProxima.IsVisible = true;

                foreach (var child in StackOpcoes.Children)
                    if (child is Button btn) btn.IsEnabled = false;

                return;
            }

            // Correto — desabilita botões e exibe feedback
            foreach (var child in StackOpcoes.Children)
                if (child is Button btn) btn.IsEnabled = false;

            FrameFeedback.IsVisible = true;
            FrameFeedback.BackgroundColor = Color.FromArgb("#E8F5E9");
            LabelFeedback.Text = $"✅ {mensagem}";
            LabelFeedback.TextColor = Color.FromArgb("#2E7D32");
            BtnProxima.IsVisible = true;
        }

        private async void OnProximaClicked(object sender, EventArgs e)
        {
            await Navigation.PopAsync();
        }
    }
}