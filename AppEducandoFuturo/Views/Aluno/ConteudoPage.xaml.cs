using AppEducandoFuturo.Models;
using AppEducandoFuturo.Services;

namespace AppEducandoFuturo.Views.Aluno
{
    public partial class ConteudoPage : ContentPage
    {
        private readonly AtividadeService _atividadeService;
        private Modulo _modulo;

        public ConteudoPage(AtividadeService atividadeService)
        {
            InitializeComponent();
            _atividadeService = atividadeService;
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
            {
                var atividades = await _atividadeService.BuscarPorModuloAsync(_modulo.Id);
                CollectionAtividades.ItemsSource = atividades;
            }
        }

        private async void OnAtividadeTapped(object sender, TappedEventArgs e)
        {
            if (e.Parameter is Atividade atividade)
            {
                var page = Handler.MauiContext.Services.GetService<AtividadePage>();
                page.CarregarAtividade(atividade);
                await Navigation.PushAsync(page);
            }
        }
    }
}