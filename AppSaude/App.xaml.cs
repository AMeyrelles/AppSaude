using AppSaude.MVVM.Views;
using AppSaude.Services;
using Plugin.LocalNotification;
using Plugin.Maui.Audio;

namespace AppSaude
{
    public partial class App : Application
    {
        private readonly IService _alarmeService;

        private readonly IAudioManager _audioManager;

        IServiceAndroid _servicesAndroid;

        public App(IService alarmeService, IServiceAndroid servicesAndroid, IAudioManager audioManager)
        {
            InitializeComponent();

            // Adicionar o manipulador de eventos para a notificação
            LocalNotificationCenter.Current.NotificationActionTapped += Current_NotificationActionTapped;

            // Atribuir dependências injetadas
            _alarmeService = alarmeService ?? throw new ArgumentNullException(nameof(alarmeService));
            _audioManager = audioManager ?? throw new ArgumentNullException(nameof(audioManager));

            _audioManager = audioManager;

            _servicesAndroid = servicesAndroid;

            // Configurar a página inicial
            MainPage = new NavigationPage(new HomePageView(_alarmeService, _servicesAndroid, _audioManager));
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
                        await navigationPage.PushAsync(new HomePageView(_alarmeService, _servicesAndroid, _audioManager));
                    }
                    else
                    {
                        // Se sua MainPage não for um NavigationPage, substitua pela sua navegação personalizada
                        await Application.Current.MainPage.Navigation.PushAsync(new HomePageView(_alarmeService, _servicesAndroid, _audioManager));
                    }
                });
            }
        }

    }
}


