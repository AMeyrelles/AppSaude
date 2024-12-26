using AppSaude.MVVM.ViewModels;
using AppSaude.Services;
using Plugin.LocalNotification;
using Plugin.Maui.Audio;
using System.Timers;

namespace AppSaude.MVVM.Views
{
    public partial class AlarmeAddView : ContentPage
    {
        private readonly IAlarmeService _alarmeService;
        private readonly IAudioManager _audioManager;
        private readonly List<DateTime> _alarmList = new List<DateTime>();
      

        public AlarmeAddView(IAlarmeService alarmeService, IAudioManager audioManager)
        {
            InitializeComponent();
            _audioManager = audioManager ?? throw new ArgumentNullException(nameof(audioManager), "O serviço de áudio não foi fornecido.");
            _alarmeService = alarmeService ?? throw new ArgumentNullException(nameof(alarmeService), "O serviço de alarme não foi fornecido.");
            BindingContext = new AlarmeViewModel(_alarmeService);                     
        }

        //Botão de voltar
        private async void btnCancelarAlarme_Clicked(object sender, EventArgs e)
        {
            await Navigation.PopAsync();
        }

        //Botão para salvar o alarme
        private async void btnAddAlarme_Clicked(object sender, EventArgs e)
        {
            TimeSpan selectedTime = TimePickerControl.Time;
            DateTime now = DateTime.Now;
            DateTime alarmDateTime = new DateTime(now.Year, now.Month, now.Day, selectedTime.Hours, selectedTime.Minutes, 0);

            if (alarmDateTime <= now)
            {
                alarmDateTime = alarmDateTime.AddDays(1);
            }

            _alarmList.Add(alarmDateTime);

            await VerifyPermissionsAsync();
            await ScheduleAlarmAsync(alarmDateTime);
        }

        //Verifica permissão para exibir notificacao
        private async Task VerifyPermissionsAsync()
        {
            try
            {
                var status = await Permissions.CheckStatusAsync<Permissions.PostNotifications>();

                if (status != PermissionStatus.Granted)
                {
                    await DisplayAlert("Alerta", "Aceite para receber notificações!", "OK");
                    status = await Permissions.RequestAsync<Permissions.PostNotifications>();
                }

                if (status == PermissionStatus.Granted)
                {
                    Console.WriteLine("Permissão para notificações concedida.");
                }
                else
                {
                    Console.WriteLine("Permissão para notificações negada.");
                    await DisplayAlert("Alerta", "Permissão para notificações NEGADA!", "OK");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro ao verificar permissões: {ex.Message}");
            }
        }

        //Dispara a notificacao
        private async Task ScheduleAlarmAsync(DateTime alarmDateTime)
        {
            try
            {
                var notification = new NotificationRequest
                {
                    NotificationId = 100,
                    Title = "Lembrete de Remédio",
                    Description = "É hora de tomar seu remédio!",
                    Schedule = new NotificationRequestSchedule
                    {
                        NotifyTime = alarmDateTime
                    },
                    Android = new Plugin.LocalNotification.AndroidOption.AndroidOptions
                    {
                        AutoCancel = true,
                        IconSmallName = { ResourceName = "bell.png" }
                    }
                };

                await LocalNotificationCenter.Current.Show(notification);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro ao agendar a notificação: {ex.Message}");
            }
        }


        //Checa se o alarme atual é igual o horario atual
        private async void CheckAlarms(object sender, ElapsedEventArgs e)
        {
            TimeSpan selectedTime = TimePickerControl.Time;
            DateTime now = DateTime.Now;
            DateTime alarmDateTime = new DateTime(now.Year, now.Month, now.Day, selectedTime.Hours, selectedTime.Minutes, 0);

            foreach (var alarm in _alarmList)
            {

                if (alarmDateTime <= now)
                {
                    alarmDateTime = alarmDateTime.AddDays(1);
                }
                else
                {
                    if (now.Hour == alarm.Hour && now.Minute == alarm.Minute)
                    {
                        await OnAudioTriggered();
                        await OpenAlarmeViewAsync();
                        break;
                    }
                }

            }
        }

        //Disparo para o som de notificação
        private async Task OnAudioTriggered()
        {
            var player = _audioManager.CreatePlayer(await FileSystem.OpenAppPackageFileAsync("careless_whisper.mp3"));
            player.Play();
            player.PlaybackEnded += (sender, e) =>
            {
                player.Dispose();
            };
        }

        //Navega para a tela quando o alarme disparar
        private async Task OpenAlarmeViewAsync()
        {
            await Navigation.PushAsync(new AlarmesView(_alarmeService, _audioManager));
        }
      
    }
}
