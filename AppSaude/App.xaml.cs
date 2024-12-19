using AppSaude.MVVM.Views;
using AppSaude.Services;

namespace AppSaude
{
    public partial class App : Application
    {
        private readonly IAlarmeService _alarmeService;
        public App(IAlarmeService alarmeService)
        {
            InitializeComponent();

            _alarmeService = alarmeService;
            MainPage = new NavigationPage(new HomePageView(_alarmeService));
        }
    }
}
