using AppSaude.MVVM.Views;
using AppSaude.Services;
using Plugin.LocalNotification;
using Plugin.Maui.Audio;

namespace AppSaude
{
    public partial class App : Application
    {
        private readonly IServicesTeste _services;
        private readonly IAudioManager _audioManager;
        private readonly IServiceAndroid _servicesAndroid;
        private readonly IAlarmService _alarmeService;

        public App(IServicesTeste services, IServiceAndroid servicesAndroid, IAudioManager audioManager, IAlarmService alarmeService)
        {
            InitializeComponent();

            // Adicionar o manipulador de eventos para a notificação
            LocalNotificationCenter.Current.NotificationActionTapped += Current_NotificationActionTapped;

            // Atribuir dependências injetadas
            _services = services ?? throw new ArgumentNullException(nameof(services));
            _alarmeService = alarmeService ?? throw new ArgumentNullException(nameof(alarmeService));
            _audioManager = audioManager ?? throw new ArgumentNullException(nameof(audioManager));        
            _servicesAndroid = servicesAndroid ?? throw new ArgumentNullException(nameof(servicesAndroid));


            // Configurar a página inicial
            MainPage = new NavigationPage(new HomePageView(_services, _servicesAndroid, _audioManager, _alarmeService));

    
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
                        await navigationPage.PushAsync(new HomePageView(_services, _servicesAndroid, _audioManager, _alarmeService));
                    }
                    else
                    {
                        // Se sua MainPage não for um NavigationPage, substitua pela sua navegação personalizada
                        await Application.Current.MainPage.Navigation.PushAsync(new AlarmesView(_services, _servicesAndroid));
                    }
                });
            }
        }

    }
}


