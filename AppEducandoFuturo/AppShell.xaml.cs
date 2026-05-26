namespace AppEducandoFuturo
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();

            // Registra as rotas para navegação por GoToAsync
            Routing.RegisterRoute("CadastroPage", typeof(Views.Auth.CadastroPage));
        }
    }
}