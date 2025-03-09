using Android.OS;
using AppSaude.Services;
using Android.App;
using Android.Content;
using AppSaude.Platforms.Android;
using Android.Content.PM;
using Plugin.Maui.Audio;
using AppSaude.MVVM.Models;
using Plugin.LocalNotification;
using Microsoft.Maui.Controls;


namespace AppSaude
{
    [Service(ForegroundServiceType = ForegroundService.TypeDataSync)]
    public class ServiceAndroid : Service, IServiceAndroid   
    {
        
        private IServicesTeste _services;
        private IAudioManager _audioManager;


        public ServiceAndroid()
        {
            // Inicialize manualmente os serviços necessários, se for necessário
            _services = IPlatformApplication.Current.Services.GetService<IServicesTeste>();
            _audioManager = IPlatformApplication.Current.Services.GetService<IAudioManager>();

            if (_services == null || _audioManager == null)
            {
                throw new InvalidOperationException("Erro ao resolver dependências para ServiceAndroid.");
            }
        }

        public ServiceAndroid(IServicesTeste services, IAudioManager audioManager)
        {
            _services = services ?? throw new ArgumentNullException(nameof(services));
            _audioManager = audioManager ?? throw new ArgumentNullException(nameof(audioManager));
        }
        public bool IsRunning { get; set; } = false;

        private CancellationTokenSource _cancellationTokenSource;
        public override IBinder OnBind(Intent intent) => null;
        public override StartCommandResult OnStartCommand(Intent intent, StartCommandFlags flags, int startId)
        {
            _cancellationTokenSource?.Cancel();
            _cancellationTokenSource?.Dispose();
            _cancellationTokenSource = new CancellationTokenSource();

            _ = ExecuteAsync(_cancellationTokenSource.Token);

            return StartCommandResult.Sticky;
        }

        private async Task ExecuteAsync(CancellationToken cancellationToken)
        {
            RegisterNotification();

            while (!cancellationToken.IsCancellationRequested)
            {
                try
                {
                    await CheckAlarms();
                    await Task.Delay(TimeSpan.FromSeconds(15), cancellationToken);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"SERVICEANDROID: Erro ao executar serviço: {ex.Message}");
                    await Task.Delay(TimeSpan.FromSeconds(30), cancellationToken);
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



        //INICIO - ALARMES E SUA FUNCIONALIDADES
        public Task InitializeAsync()
        {
            return CheckAlarms();
        }
        public async Task CheckAlarms()
        {
            try
            {
                await VerifyPermissionsAsync();

                // Obtém a data e o horário atual
                DateTime now = DateTime.Now;

                var alarms = await LoadAlarmsFromDatabaseAsync();
                Console.WriteLine($"SERVICEANDROID: Alarmes verificados às {now.Hour}:{now.Minute}");

                if (alarms == null || !alarms.Any())
                {
                    Console.WriteLine("SERVICEANDROID: Nenhum alarme encontrado.");
                    return;
                }

                foreach (var alarm in alarms)
                {                   
                    if (alarm.IsNotified 
                        && now.Hour == alarm.ReminderTime.Hours 
                        && now.Minute == alarm.ReminderTime.Minutes) //testar para ver se salva no mesmo horario
                    {                                                
                            alarm.IsNotified = false;
                            // Cria uma instância de NotificacaoAlarme a partir de Alarme
                            var notificacaoAlarme = new NotificacaoAlarme
                            {
                                IdNA = alarm.Id,
                                MedicationNameNA = alarm.MedicationName,
                                PatientNameNA = alarm.PatientName,
                                DescriptionNA = alarm.Description,
                                ReminderTimeNA = alarm.ReminderTime,
                                IsEnabledNA = alarm.IsEnabled,
                                IsNotifiedNA = alarm.IsNotified,
                                LastNotifiedDateNA = alarm.LastNotifiedDate                                
                            };

                            //Adiciona a instancia em notificação alarme
                            await _services.AddNotAlarme(notificacaoAlarme);
                            ////Atualiza o alarme no banco de dados
                            await _services.UpdateAlarme(alarm);                    

                            continue;                        
                    }

                    if (alarm.LastNotifiedDate.HasValue && alarm.LastNotifiedDate.Value.Date == now.Date) continue;

                    if (now.Hour == alarm.ReminderTime.Hours && now.Minute == alarm.ReminderTime.Minutes)
                    {
                        alarm.LastNotifiedDate = now;
                        alarm.IsNotified = true;

                        await OnAudioTriggered();
                        await ScheduleAlarmAsync(alarm);


                        ////Atualiza o alarme no banco de dados
                        await _services.UpdateAlarme(alarm);

                        Console.WriteLine("SERVICEANDROID: Passei pelo Foreach");

                        break;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"SERVICEANDROID : Erro ao verificar alarmes: {ex.Message}");
            }
        }
        private async Task<List<Alarme>> LoadAlarmsFromDatabaseAsync()
        {
            try
            {
                // Busca todos os alarmes do banco de dados usando o serviço
                var alarms = await _services.GetAlarmes();

                // Verifica se a lista retornada não é nula
                if (alarms == null)
                {
                    Console.WriteLine("SERVICEANDROID : Nenhum alarme encontrado no banco de dados.");
                    return new List<Alarme>();
                }

                Console.WriteLine($"SERVICEANDROID : Total de alarmes encontrados: {alarms.Count}");
                return alarms;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"SERVICEANDROID : Erro ao carregar alarmes: {ex.Message}");
                return new List<Alarme>();
            }
        }
        private async Task OnAudioTriggered()
        {
            var player = _audioManager.CreatePlayer(await FileSystem.OpenAppPackageFileAsync("ola_esta_na_hora_de_tomar_o_seu_medicamento_tom.mp3"));
            player.Play();
            player.PlaybackEnded += (sender, e) =>
            {
                player.Dispose();
            };
        }
        //Dispara a mensagem de notificacao
        private async Task ScheduleAlarmAsync(Alarme alarm)
        {
            try
            {
                // Define a hora exata do alarme para hoje
                DateTime alarmDateTime = DateTime.Now.Date.Add(alarm.ReminderTime);

                var notification = new NotificationRequest
                {
                    NotificationId = alarm.Id, // ID único para identificar a notificação
                    Title = "Lembrete de Remédio",
                    Description = $"Hora de tomar: {alarm.MedicationName}",
                    Schedule = new NotificationRequestSchedule
                    {
                        NotifyTime = alarmDateTime,
                        RepeatType = NotificationRepeat.Daily // Repete diariamente (se necessário)
                    },
                    Android = new Plugin.LocalNotification.AndroidOption.AndroidOptions
                    {
                        AutoCancel = true,
                        IconSmallName = { ResourceName = "sino" }
                    }
                };

                await LocalNotificationCenter.Current.Show(notification);
                Console.WriteLine("SERVICEANDROID: Notificação agendada com sucesso!");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"SERVICEANDROID: Erro ao agendar a notificação: {ex.Message}");
            }
        }
        private static async Task VerifyPermissionsAsync()
        {
            try
            {
                var status = await Permissions.CheckStatusAsync<Permissions.PostNotifications>();

                if (status != PermissionStatus.Granted)
                {

                    status = await Permissions.RequestAsync<Permissions.PostNotifications>();
                }

                if (status == PermissionStatus.Granted)
                {
                    Console.WriteLine("Permissão para notificações concedida!");
                }
                else
                {
                    Console.WriteLine("Permissão para notificações negada!");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro ao verificar permissões: {ex.Message}");
            }
        }

        //FIM - ALARMES E SUA FUNCIONALIDADES
    }

}

