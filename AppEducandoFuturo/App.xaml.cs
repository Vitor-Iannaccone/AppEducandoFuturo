namespace AppEducandoFuturo
{
    public partial class App : Application
    {
        public App(Views.Auth.LoginPage loginPage)
        {
            InitializeComponent();
            MainPage = new NavigationPage(loginPage);
        }
    }
}