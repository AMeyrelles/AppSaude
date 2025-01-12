using AppSaude.MVVM.ViewModels;
using AppSaude.Services;
using Plugin.LocalNotification;

namespace AppSaude.MVVM.Views;

public partial class AgendamentoAddView : ContentPage
{
	private readonly IService _services;

    private readonly List<DateTime> _agendamentoList = new List<DateTime>();
    public AgendamentoAddView(IService services)
	{
        InitializeComponent();
        _services = services;

        var viewModel = new AgendamentoViewModel(services);
        BindingContext = viewModel;
    }   

    //Bot�o para salvar o agendamento
    private async void btnAddAgendamento_Clicked(object sender, EventArgs e)
    {
        // Obt�m a data e a hora selecionadas
        DateTime selectedDate = datePickerControl.Date;
        TimeSpan selectedTime = timePickerControl.Time;

        // Combina a data e a hora
        DateTime agendamentoDateTime = selectedDate.Date.Add(selectedTime);

        // Verifica se a data selecionada � no passado
        if (selectedDate < DateTime.Now.Date)
        {
            await DisplayAlert("Erro", "A data selecionada n�o pode ser no passado.", "OK");
            return;
        }

        //Dispara notifica��o
        if (selectedDate.Date == agendamentoDateTime.Date)
        {
            var notification = new NotificationRequest
            {
                NotificationId = 102,
                Title = "ALERTA!",
                Description = "Voc� tem um agendamento marcado HOJE!",
                Schedule = new NotificationRequestSchedule
                {
                    NotifyTime = agendamentoDateTime
                },
                Android = new Plugin.LocalNotification.AndroidOption.AndroidOptions
                {
                    AutoCancel = true,
                    IconSmallName = { ResourceName = "bell.png" }
                }
            };

           await LocalNotificationCenter.Current.Show(notification);
        }

        // Adiciona o agendamento � lista (opcional)
        _agendamentoList.Add(selectedDate);

        // Configura a notifica��o
        await VerifyPermissionsAsync();
        ScheduleNotificationDay(selectedDate);
    }

    //Dispara notifica��o
    private void ScheduleNotificationDay(DateTime agendamentoDateTime)
    {
        try
        {
            var notification = new NotificationRequest
            {
                NotificationId = 101,
                Title = "ALERTA!",
                Description = "N�o esque�a do seu agendamento HOJE!",
                Schedule = new NotificationRequestSchedule
                {
                    NotifyTime = agendamentoDateTime
                },
                Android = new Plugin.LocalNotification.AndroidOption.AndroidOptions
                {
                    AutoCancel = true,
                    IconSmallName = { ResourceName = "appicon.svg" }
                }
            };

            LocalNotificationCenter.Current.Show(notification);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Erro ao agendar a notifica��o: {ex.Message}");
        }
    }

    //Verificar a permiss�o do usuario
    private async Task VerifyPermissionsAsync()
    {
        try
        {
            var status = await Permissions.CheckStatusAsync<Permissions.PostNotifications>();

            if (status != PermissionStatus.Granted)
            {
                await DisplayAlert("Alerta", "Aceite para receber notifica��es!", "OK");
                status = await Permissions.RequestAsync<Permissions.PostNotifications>();
            }

            if (status == PermissionStatus.Granted)
            {
                Console.WriteLine("Permiss�o para notifica��es concedida.");
            }
            else
            {
                await App.Current.MainPage.DisplayAlert("Alerta!", "Permiss�o para notifica��es negada.", "OK");
                await DisplayAlert("Alerta", "Permiss�o para notifica��es NEGADA!", "OK");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Erro ao verificar permiss�es: {ex.Message}");
        }
    }

    //Bot�o de Cancelar/Retornar
    private async void btnCancelarAgendamento_Clicked(object sender, EventArgs e)
    {
        await Navigation.PopAsync();
    }


}