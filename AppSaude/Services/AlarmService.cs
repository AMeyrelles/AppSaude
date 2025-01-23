using AppSaude.MVVM.Models;
using Plugin.LocalNotification;
using Plugin.Maui.Audio;


namespace AppSaude.Services
{
    public class AlarmService : IAlarmService
    {
        private readonly IServicesTeste _services;
        private readonly IAudioManager _audioManager;
        private readonly TimeSpan reminderTime;


        public async Task InitializeAsync()
        {
           await CheckAlarms();
        }
        public AlarmService(IServicesTeste services, IAudioManager audioManager)
        {
            _services = services ?? throw new ArgumentNullException(nameof(services));
            _audioManager = audioManager ?? throw new ArgumentNullException(nameof(audioManager));
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
                    Console.WriteLine("AlarmService : Nenhum alarme encontrado no banco de dados.");
                    return new List<Alarme>();
                }

                Console.WriteLine($"AlarmService : Total de alarmes encontrados: {alarms.Count}");
                return alarms;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"AlarmService : Erro ao carregar alarmes: {ex.Message}");
                return new List<Alarme>();
            }
        }

        //private async Task<IEnumerable<Alarme>> GetValidAlarmsAsync()
        //{
        //    try
        //    {
        //        var alarms = await _services.GetAlarmes();
        //        return alarms?.Where(a => a.IsEnabled) ?? Enumerable.Empty<Alarme>();
        //    }
        //    catch (Exception ex)
        //    {
        //        Console.WriteLine($"Erro ao carregar alarmes: {ex.Message}");
        //        return Enumerable.Empty<Alarme>();
        //    }
        //}

        public async Task CheckAlarms()
        {
            try
            {
                await VerifyPermissionsAsync();

                // Obtém a data e o horário atual
                DateTime now = DateTime.Now;

                var alarms = await LoadAlarmsFromDatabaseAsync();
                Console.WriteLine("AlarmService: Passei pelo LoadAlarmsFromDatabaseAsync");
                Console.WriteLine($"AlarmService: Alarmes verificados às {now.Hour}:{now.Minute}");

                if (alarms == null || !alarms.Any())
                {
                    Console.WriteLine("AlarmeService: Nenhum alarme encontrado.");
                    return;
                }

                foreach (var alarm in alarms)
                {
                    if (alarm.LastNotifiedDate.HasValue && alarm.LastNotifiedDate.Value.Date == now.Date) continue;

                    if (now.Hour == alarm.ReminderTime.Hours && now.Minute == alarm.ReminderTime.Minutes)
                    {
                        alarm.LastNotifiedDate = now;

                        await OnAudioTriggered();
                        await ScheduleAlarmAsync(alarm);                                          

                        //Atualiza o alarme no banco de dados
                        await _services.UpdateAlarme(alarm);
                        Console.WriteLine("AlarmeService: Passei pelo Foreach");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"AlarmService : Erro ao verificar alarmes: {ex.Message}");
            }
        }

        //Dispara o som de notificação
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
                        IconSmallName = { ResourceName = "icon_mais_.svg" }
                    }
                };

                await LocalNotificationCenter.Current.Show(notification);
                Console.WriteLine("Notificação agendada com sucesso!");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro ao agendar a notificação: {ex.Message}");
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
    }
}
