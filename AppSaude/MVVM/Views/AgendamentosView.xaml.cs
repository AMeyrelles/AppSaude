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
            // O BindingContext é da ViewModel que contém o DisplayCommand
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
                NotificationId = 101,
                Title = "Hoje é o dia!",
                Description = "Não esqueça do seu agendamento!",
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

    private async void btnAdd_Clicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new AgendamentoAddView(_services));
    }
}