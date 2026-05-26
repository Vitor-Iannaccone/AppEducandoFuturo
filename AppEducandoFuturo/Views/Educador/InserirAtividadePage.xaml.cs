using AppEducandoFuturo.Models;
using AppEducandoFuturo.Services;

namespace AppEducandoFuturo.Views.Educador
{
    public partial class InserirAtividadePage : ContentPage
    {
        private readonly AtividadeService _atividadeService;
        private readonly ModuloService _moduloService;
        private List<Modulo> _modulos;

        public InserirAtividadePage(AtividadeService atividadeService, ModuloService moduloService)
        {
            InitializeComponent();
            _atividadeService = atividadeService;
            _moduloService = moduloService;
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            _modulos = await _moduloService.BuscarTodosAsync();
            PickerModulo.ItemsSource = _modulos.Select(m => m.Titulo).ToList();
        }

        private async void OnSalvarClicked(object sender, EventArgs e)
        {
            if (PickerModulo.SelectedIndex < 0)
            {
                LabelMensagem.IsVisible = true;
                LabelMensagem.Text = "Selecione um módulo.";
                LabelMensagem.TextColor = Colors.Red;
                return;
            }

            var moduloSelecionado = _modulos[PickerModulo.SelectedIndex];
            int pontuacao = int.TryParse(EntryPontuacao.Text, out int p) ? p : 10;

            var atividade = new Atividade
            {
                ModuloId = moduloSelecionado.Id,
                Pergunta = EditorPergunta.Text,
                Opcoes = EntryOpcoes.Text,
                RespostaCorreta = EntryResposta.Text,
                Pontuacao = pontuacao
            };

            var (sucesso, mensagem) = await _atividadeService.SalvarAsync(atividade);

            LabelMensagem.IsVisible = true;
            LabelMensagem.Text = mensagem;
            LabelMensagem.TextColor = sucesso ? Colors.Green : Colors.Red;

            if (sucesso)
            {
                EditorPergunta.Text = string.Empty;
                EntryOpcoes.Text = string.Empty;
                EntryResposta.Text = string.Empty;
                EntryPontuacao.Text = string.Empty;
                PickerModulo.SelectedIndex = -1;
            }
        }
    }
}