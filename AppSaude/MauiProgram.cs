using AppSaude.Services;
using Microsoft.Extensions.Logging;
using Plugin.LocalNotification;
using Plugin.Maui.Audio;
using AppSaude.Platforms.Android;
using AppSaude.MVVM.Views;

namespace AppSaude.Platforms.Android
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();

#if ANDROID
            // Registra serviços específicos para Android
            builder.Services.AddSingleton<IServiceAndroid, ServiceAndroid>();
                      
#endif

            // Configuração geral do aplicativo
            builder
                .UseMauiApp<App>()
                .UseLocalNotification()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                });

#if DEBUG
            builder.Logging.AddDebug();
#endif

            // Serviços gerais do aplicativo
            builder.Services.AddSingleton(AudioManager.Current); // Gerenciador de Áudio
            builder.Services.AddSingleton<IServicesTeste, ServicesTeste>(); // Serviço de dados
            builder.Services.AddSingleton<IAlarmService, AlarmService>();

            // Registra páginas
            builder.Services.AddTransient<HomePageView>(); // Página inicial

            var app = builder.Build();

            // Valida serviços (apenas para debug)
#if DEBUG
            ValidateServices(app.Services);
#endif

            return app;
        }

        private static void ValidateServices(IServiceProvider serviceProvider)
        {
            var alarmService = serviceProvider.GetService<IAlarmService>();
            if (alarmService == null)
                Console.WriteLine("Erro: IAlarmService não foi registrado corretamente.");

            var serviceAndroid = serviceProvider.GetService<IServiceAndroid>();
            if (serviceAndroid == null)
                Console.WriteLine("Erro: IServiceAndroid não foi registrado corretamente.");            
        }
    }
}
