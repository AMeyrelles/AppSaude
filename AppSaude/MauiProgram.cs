using AppSaude.MVVM.Views;
using AppSaude.Services;
using Microsoft.Extensions.Logging;
using Plugin.LocalNotification;
using Plugin.Maui.Audio;

namespace AppSaude.Platforms.Android

{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();

            // Registra serviços específicos de plataforma
#if ANDROID
             builder.Services.AddSingleton<IServiceAndroid, ServiceAndroid>();
#endif

            // Configuração do aplicativo
            builder
                .UseMauiApp<App>()
                .UseLocalNotification() // Registra a notificação local
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                });

            // Adiciona logs apenas em DEBUG
#if DEBUG
            builder.Logging.AddDebug();
#endif

            // Adiciona serviços Singleton
            builder.Services.AddSingleton(AudioManager.Current); // Gerenciador de Áudio
            builder.Services.AddSingleton<IServicesTeste, ServicesTeste>(); // Serviço de dados
            builder.Services.AddSingleton<IAlarmService, AlarmService>(); // Serviço de alarmes
                      

            // Adiciona páginas com diferentes ciclos de vida
            builder.Services.AddTransient<HomePageView>(); // Página inicial

            return builder.Build();
        }
    }
}
