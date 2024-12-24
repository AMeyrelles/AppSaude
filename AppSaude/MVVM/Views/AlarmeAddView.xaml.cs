using AppSaude.MVVM.Models;
using AppSaude.MVVM.ViewModels;
using AppSaude.Services;
using Plugin.LocalNotification;

namespace AppSaude.MVVM.Views;

public partial class AlarmeAddView : ContentPage
{
    private readonly IAlarmeService _alarmeService;

    //List de alarmes
    private List<DateTime> alarmList = new List<DateTime>();

    public AlarmeAddView(IAlarmeService alarmeService)
    {
        InitializeComponent();
        _alarmeService = alarmeService;

        _alarmeService = alarmeService ?? throw new ArgumentNullException(nameof(alarmeService), "O serviço de alarme não foi fornecido.");

        // Defina o BindingContext, caso necessário para usar com MVVM.
        BindingContext = new AlarmeViewModel(_alarmeService);
    }

    private async void btnCancelarAlarme_Clicked(object sender, EventArgs e)
    {
        await Navigation.PopAsync();
    }
 

    private async void btnAddAlarme_Clicked(object sender, EventArgs e)
    {
         // Obtém o valor do TimePicker
             TimeSpan selectedTime = TimePickerControl.Time;

        // Extrai as horas e minutos
        int hours = selectedTime.Hours;
        int minutes = selectedTime.Minutes;

        var now = DateTime.Now;
        var alarmDateTime = new DateTime(now.Year, now.Month, now.Day, selectedTime.Hours, selectedTime.Minutes, 0);

        // Ajusta para o próximo dia, se o horário já passou
        if (alarmDateTime <= now)
        {
            alarmDateTime = alarmDateTime.AddDays(1);
        }

        // Adiciona o alarme à lista
        alarmList.Add(alarmDateTime);

   

        //await DisplayAlert("Alarme", $"Alarme configurado para {alarmDateTime:HH:mm}", "OK");

        // Exibe o horário selecionado        
        //await DisplayAlert("Alerta", $"Horário selecionado: {hours:D2}:{minutes:D2}", "OK");

        await VerifyPermissionsAsync();

        ScheduleAlarm(alarmDateTime);
    }

    private void ScheduleAlarm(DateTime alarmDateTime)
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
                    IconSmallName = {ResourceName = "alarme_clock_dois.png"}
                }
            };

            // Mostra a notificação
            LocalNotificationCenter.Current.Show(notification);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Erro ao agendar a notificação: {ex.Message}");
        }
    }


    //Metodo para verificar se o usuario concebeu a permissão
    private async Task VerifyPermissionsAsync()
    {
        try
        {
            // Solicita permissão para notificações
            var status = await Permissions.CheckStatusAsync<Permissions.PostNotifications>();

            if (status != PermissionStatus.Granted)
            {
                await DisplayAlert("Alerta", "Aceite para receber notificações!", "OK");
                status = await Permissions.RequestAsync<Permissions.PostNotifications>();

            }

            if (status == PermissionStatus.Granted)
            {
                // Permissão concedida
                Console.WriteLine("Permissão para notificações concedida.");
                //await DisplayAlert("Alerta", "Permissão para notificações concedida.", "OK");
            }
            else
            {
                // Permissão negada
                Console.WriteLine("Permissão para notificações negada.");
                await DisplayAlert("Alerta", "Permissão para notificações NEGADA!.", "OK");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Erro ao verificar permissões: {ex.Message}");
        }
    }
}
