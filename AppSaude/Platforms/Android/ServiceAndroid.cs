using Android.OS;
using AppSaude.Services;
using Android.App;
using Android.Content;
using Android.Util;
using Service = Android.App.Service;
using Resource = AppSaude.Resource;
using Android.Runtime;


namespace AppSaude
{
    [Service(ForegroundServiceType = Android.Content.PM.ForegroundService.TypeDataSync)]
    public class ServiceAndroid : Service, IServiceAndroid
    {
        public override IBinder OnBind(Intent intent)
        {
            throw new NotImplementedException();
        }
        [return: GeneratedEnum]
        public override StartCommandResult OnStartCommand(Intent intent, [GeneratedEnum] StartCommandFlags flags, int startId)
        {
            if (intent.Action == "START_SERVICE")
            {
                System.Diagnostics.Debug.WriteLine("Serviço inicado!");
                RegisterNotification();
            }
            else if (intent.Action == "STOP_SERVICE")
            {
                System.Diagnostics.Debug.WriteLine("Serviço encerrado!");
                if (Build.VERSION.SdkInt >= BuildVersionCodes.Tiramisu)
                {
                    StopForeground(StopForegroundFlags.Remove);
                }
                else
                {
                    StopForeground(true);
                }
                StopSelfResult(startId);
            }
            return StartCommandResult.NotSticky;
        }
          
        public void Start()
        {

            Intent startService = new Intent(MainActivity.ActivityCurrent, typeof(ServiceAndroid));
            startService.SetAction("START_SERVICE");
            MainActivity.ActivityCurrent.StartService(startService);
        }

        public void Stop()
        {
            Intent stopIntent = new Intent(MainActivity.ActivityCurrent, this.Class);
            stopIntent.SetAction("STOP_SERVICE");
            MainActivity.ActivityCurrent.StartService(stopIntent);
        }

        private void RegisterNotification()
        {
            NotificationChannel channel = new NotificationChannel("ServiceChannel", "Servico Teste", NotificationImportance.Max);
            NotificationManager manager = (NotificationManager)MainActivity.ActivityCurrent.GetSystemService(Context.NotificationService);
            manager.CreateNotificationChannel(channel);
            Notification notification = new Notification.Builder(this, "ServiceChannel")
                .SetContentTitle("Estou trabalhando!")
                .SetSmallIcon(Resource.Drawable.abc_ab_share_pack_mtrl_alpha)
                .SetOngoing(true)
                .Build();

            StartForeground(100, notification);

        }
    }
}
