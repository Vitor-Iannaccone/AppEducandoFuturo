using AppEducandoFuturo.Services;

namespace AppEducandoFuturo.Views.Auth
{
    public partial class CadastroPage : ContentPage
    {
        private readonly AuthService _authService;

        public CadastroPage(AuthService authService)
        {
            InitializeComponent();
            _authService = authService;
        }

        private async void OnCadastrarClicked(object sender, EventArgs e)
        {
            if (EntrySenha.Text != EntryConfirmarSenha.Text)
            {
                LabelMensagem.IsVisible = true;
                LabelMensagem.Text = "As senhas não coincidem.";
                LabelMensagem.TextColor = Colors.Red;
                return;
            }

            if (PickerTipo.SelectedItem == null)
            {
                LabelMensagem.IsVisible = true;
                LabelMensagem.Text = "Selecione o tipo de usuário.";
                LabelMensagem.TextColor = Colors.Red;
                return;
            }

            var (sucesso, mensagem) = await _authService.CadastrarAsync(
                EntryNome.Text,
                EntryEmail.Text,
                EntrySenha.Text,
                PickerTipo.SelectedItem.ToString()
            );

            LabelMensagem.IsVisible = true;
            LabelMensagem.Text = mensagem;

            if (!sucesso)
            {
                LabelMensagem.TextColor = Colors.Red;
                return;
            }

            // Sucesso: volta para a tela de Login
            LabelMensagem.TextColor = Colors.Green;
            await Task.Delay(1500);
            await Navigation.PopAsync();
        }

        private async void OnLoginTapped(object sender, EventArgs e)
        {
            await Navigation.PopAsync();
        }
    }
}