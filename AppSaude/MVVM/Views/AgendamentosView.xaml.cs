using AppSaude.MVVM.ViewModels;
using AppSaude.Services;
using Plugin.LocalNotification;
using Plugin.Maui.Audio;

namespace AppSaude.MVVM.Views;

public partial class AgendamentosView : ContentPage
{
	private readonly IService _services;

    private readonly IAudioManager  _audioManager;
    public AgendamentosView(IService services, IAudioManager audioManager)
    {
        InitializeComponent();

        _audioManager = audioManager;

        _services = services;

        var viewModel = new AgendamentoViewModel(services);

        BindingContext = viewModel;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        try
        {
            // O BindingContext � da ViewModel que cont�m o DisplayCommand
            var viewModel = BindingContext as AgendamentoViewModel;
            if (viewModel != null)
            {
                // Dispara o DisplayCommand para carregar os dados automaticamente
                viewModel.DisplayCommand.Execute(null);
            }
        }
        catch (Exception ex)
        {
            await DisplayAlert("Erro", ex.Message, "OK");
        }
    }

    //Verifica permiss�o para exibir notificacao
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
                Console.WriteLine("Permiss�o para notifica��es negada.");
                await DisplayAlert("Alerta", "Permiss�o para notifica��es NEGADA!", "OK");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Erro ao verificar permiss�es: {ex.Message}");
        }
    }

    //Dispara a notificacao
    private async Task ScheduleAlarmAsync(DateTime alarmDateTime)
    {
        try
        {
            var notification = new NotificationRequest
            {
                NotificationId = 101,
                Title = "Hoje � o dia!",
                Description = "N�o esque�a do seu agendamento!",
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
            Console.WriteLine($"Erro ao agendar a notifica��o: {ex.Message}");
        }
    }

    private async void btnAdd_Clicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new AgendamentoAddView(_services));
    }
}