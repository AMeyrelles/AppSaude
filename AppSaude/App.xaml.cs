using AppSaude.MVVM.Views;
using AppSaude.Services;
using Plugin.LocalNotification;

namespace AppSaude
{
    public partial class App : Application
    {
        private readonly IAlarmeService _alarmeService;
        public App(IAlarmeService alarmeService)
        {
            InitializeComponent();           

            LocalNotificationCenter.Current.NotificationActionTapped += Current_NotificationActionTapped; // Altere esta linha

            _alarmeService = alarmeService;
            MainPage = new NavigationPage(new HomePageView(alarmeService));
        }


        private void Current_NotificationActionTapped(Plugin.LocalNotification.EventArgs.NotificationActionEventArgs e)
        {
          if (e.IsDismissed)
            {
                return;
            }
          else if (e.IsTapped)
            {

            }
        }
    }
}
