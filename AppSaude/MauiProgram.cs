using AppSaude.MVVM.Views;
using AppSaude.Services;
using Microsoft.Extensions.Logging;
using Plugin.LocalNotification;
using Plugin.Maui.Audio;

namespace AppSaude
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();

#if ANDROID
            builder.Services.AddTransient<IServiceAndroid, ServiceAndroid>();   
           
#endif
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
            builder.Services.AddSingleton(AudioManager.Current);
            builder.Services.AddSingleton<IService, Service>();
            builder.Services.AddTransient<HomePageView>();  

            return builder.Build();
        }
    }
}
