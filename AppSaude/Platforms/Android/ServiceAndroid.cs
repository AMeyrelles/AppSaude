using Android.OS;
using AppSaude.Services;
using Android.App;
using Android.Content;
using AppSaude.Platforms.Android;
using Android.Content.PM;


namespace AppSaude
{
    [Service(ForegroundServiceType = Android.Content.PM.ForegroundService.TypeDataSync)]
    public class ServiceAndroid : Service, IServiceAndroid
    {
        public bool IsRunning { get; private set; } = false;
        private CancellationTokenSource _cancellationTokenSource;

        private IAlarmService _alarmService { get; }            

        public override IBinder OnBind(Intent intent) => null;
        public override StartCommandResult OnStartCommand(Intent intent, StartCommandFlags flags, int startId)
        {
            _cancellationTokenSource?.Cancel();
            _cancellationTokenSource?.Dispose();
            _cancellationTokenSource = new CancellationTokenSource();

            _ = ExecuteAlarmServiceAsync(_cancellationTokenSource.Token);
            return StartCommandResult.Sticky;
        }

        private async Task ExecuteAlarmServiceAsync(CancellationToken cancellationToken)
        {
            try
            {
                RegisterNotification();

                while (!cancellationToken.IsCancellationRequested)
                {
                    try
                    {
                        if (_alarmService == null)
                        {
                            Console.WriteLine("ServiceAndroid: Erro: _alarmService não foi inicializado.");
                            return;
                        }

                        await CheckAlarmsAsync();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"ServiceAndroid: Erro ao verificar alarmes: {ex.Message}");
                    }

                    try
                    {
                        await Task.Delay(TimeSpan.FromSeconds(15), cancellationToken);
                    }
                    catch (TaskCanceledException)
                    {
                        break;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"ServiceAndroid: Erro na execução do serviço: {ex.Message}");
            }
        }

        public override void OnDestroy()
        {
            base.OnDestroy();
            IsRunning = false;
            _cancellationTokenSource?.Cancel();
            _cancellationTokenSource?.Dispose();
            _cancellationTokenSource = null;
        }

        public void Start()
        {
            if (!IsRunning)
            {
                IsRunning = true;
                Console.WriteLine("ServiceAndroid: Serviço iniciado.");
            }

            if (MainActivity.ActivityCurrent == null)
            {
                Console.WriteLine("Erro: MainActivity.ActivityCurrent está null.");
                return;
            }

            Intent startService = new(MainActivity.ActivityCurrent, typeof(ServiceAndroid));
            startService.SetAction("START_SERVICE");
            MainActivity.ActivityCurrent.StartService(startService);
        }

        public void Stop()
        {
            if (IsRunning)
            {
                IsRunning = false;
                Console.WriteLine("ServiceAndroid: Serviço parado.");
            }

            if (MainActivity.ActivityCurrent == null)
            {
                Console.WriteLine("Erro: MainActivity.ActivityCurrent está null.");
                return;
            }

            Intent stopIntent = new(MainActivity.ActivityCurrent, this.Class);
            stopIntent.SetAction("STOP_SERVICE");
            MainActivity.ActivityCurrent.StartService(stopIntent);
        }

        private void RegisterNotification()
        {
            try
            {
                NotificationChannel channel = new("ServiceChannel", "Servico Teste", NotificationImportance.Max);
                NotificationManager manager = (NotificationManager)MainActivity.ActivityCurrent.GetSystemService(Context.NotificationService);
                manager.CreateNotificationChannel(channel);

                Notification notification = new Notification.Builder(this, "ServiceChannel")
                    .SetContentTitle("Lembre+")
                    .SetContentText("Estou trabalhando!")
                    .SetSmallIcon(Resource.Drawable.icon_mais)
                    .SetOngoing(true)
                    .Build();

                StartForeground(100, notification);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro ao registrar notificação: {ex.Message}");
            }
        }

        public async Task CheckAlarmsAsync()
        {
            if (_alarmService == null)
            {
                Console.WriteLine("Erro: _alarmService não foi inicializado.");
                return;
            }

            await _alarmService.CheckAlarms();
        }
    }
}
