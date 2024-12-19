using AppSaude.MVVM.Views;
using AppSaude.Services;

namespace AppSaude
{
    public partial class App : Application
    {

        public App(IAlarmeService alarmeService)
        {
            InitializeComponent();

            MainPage = new NavigationPage(new AlarmesView(alarmeService));
        }
    }
}
