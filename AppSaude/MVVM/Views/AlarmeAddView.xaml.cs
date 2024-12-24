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

        _alarmeService = alarmeService ?? throw new ArgumentNullException(nameof(alarmeService), "O servi�o de alarme n�o foi fornecido.");

        // Defina o BindingContext, caso necess�rio para usar com MVVM.
        BindingContext = new AlarmeViewModel(_alarmeService);
    }

    private async void btnCancelarAlarme_Clicked(object sender, EventArgs e)
    {
        await Navigation.PopAsync();
    }
 

    private async void btnAddAlarme_Clicked(object sender, EventArgs e)
    {
         // Obt�m o valor do TimePicker
             TimeSpan selectedTime = TimePickerControl.Time;

        // Extrai as horas e minutos
        int hours = selectedTime.Hours;
        int minutes = selectedTime.Minutes;

        var now = DateTime.Now;
        var alarmDateTime = new DateTime(now.Year, now.Month, now.Day, selectedTime.Hours, selectedTime.Minutes, 0);

        // Ajusta para o pr�ximo dia, se o hor�rio j� passou
        if (alarmDateTime <= now)
        {
            alarmDateTime = alarmDateTime.AddDays(1);
        }

        // Adiciona o alarme � lista
        alarmList.Add(alarmDateTime);

   

        //await DisplayAlert("Alarme", $"Alarme configurado para {alarmDateTime:HH:mm}", "OK");

        // Exibe o hor�rio selecionado        
        //await DisplayAlert("Alerta", $"Hor�rio selecionado: {hours:D2}:{minutes:D2}", "OK");

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
                Title = "Lembrete de Rem�dio",
                Description = "� hora de tomar seu rem�dio!",
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

            // Mostra a notifica��o
            LocalNotificationCenter.Current.Show(notification);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Erro ao agendar a notifica��o: {ex.Message}");
        }
    }


    //Metodo para verificar se o usuario concebeu a permiss�o
    private async Task VerifyPermissionsAsync()
    {
        try
        {
            // Solicita permiss�o para notifica��es
            var status = await Permissions.CheckStatusAsync<Permissions.PostNotifications>();

            if (status != PermissionStatus.Granted)
            {
                await DisplayAlert("Alerta", "Aceite para receber notifica��es!", "OK");
                status = await Permissions.RequestAsync<Permissions.PostNotifications>();

            }

            if (status == PermissionStatus.Granted)
            {
                // Permiss�o concedida
                Console.WriteLine("Permiss�o para notifica��es concedida.");
                //await DisplayAlert("Alerta", "Permiss�o para notifica��es concedida.", "OK");
            }
            else
            {
                // Permiss�o negada
                Console.WriteLine("Permiss�o para notifica��es negada.");
                await DisplayAlert("Alerta", "Permiss�o para notifica��es NEGADA!.", "OK");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Erro ao verificar permiss�es: {ex.Message}");
        }
    }
}
