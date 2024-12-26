using AppSaude.MVVM.Views;
using AppSaude.Services;
using Plugin.LocalNotification;
using Plugin.Maui.Audio;
using System.Globalization;

namespace AppSaude
{
    public partial class App : Application
    {
        private readonly IAlarmeService _alarmeService;
        private readonly IAudioManager _audioManager;

        public App(IAlarmeService alarmeService, IAudioManager audioManager)
        {
            InitializeComponent();               

            // Adicionar o manipulador de eventos para a notificação
            LocalNotificationCenter.Current.NotificationActionTapped += Current_NotificationActionTapped;

            _audioManager = audioManager;

            // Atribuir dependências injetadas
            _alarmeService = alarmeService ?? throw new ArgumentNullException(nameof(alarmeService));
            _audioManager = audioManager ?? throw new ArgumentNullException(nameof(audioManager));

            // Configurar a página inicial
            MainPage = new NavigationPage(new HomePageView(_alarmeService, _audioManager));
        }

        // Método de manipulação para NotificationActionTapped
        private void Current_NotificationActionTapped(Plugin.LocalNotification.EventArgs.NotificationActionEventArgs e)
        {
            if (e.IsDismissed)
            {
                return;
            }

            if (e.IsTapped)
            {
                MainThread.BeginInvokeOnMainThread(async () =>
                {
                    // Navegue para a página desejada
                    var navigationPage = (Application.Current.MainPage as NavigationPage);
                    if (navigationPage != null)
                    {
                        await navigationPage.PushAsync(new AlarmesView(_alarmeService, _audioManager));
                    }
                    else
                    {
                        // Se sua MainPage não for um NavigationPage, substitua pela sua navegação personalizada
                        await Application.Current.MainPage.Navigation.PushAsync(new AlarmesView(_alarmeService, _audioManager));
                    }
                });
            }
        }

    }
}


