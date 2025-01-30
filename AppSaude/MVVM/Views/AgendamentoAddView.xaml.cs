using AppSaude.MVVM.ViewModels;
using AppSaude.Services;
using Plugin.LocalNotification;
using System.Collections.ObjectModel;
using System.Text.RegularExpressions;

namespace AppSaude.MVVM.Views;

public partial class AgendamentoAddView : ContentPage
{
	private readonly IServicesTeste _services;
    private readonly IServiceAndroid _serviceAndroid;

    public AgendamentoAddView(IServicesTeste services, IServiceAndroid servicesAndroid)
	{
        InitializeComponent();
        _services = services ?? throw new ArgumentNullException(nameof(services));
        _serviceAndroid = servicesAndroid ?? throw new ArgumentNullException(nameof(servicesAndroid));

        var viewModel = new AgendamentoViewModel(services);
        BindingContext = viewModel;

        // Define o MinimumDate como a data atual
        datePickerControl.MinimumDate = DateTime.Today;
    }   

    //Botão para salvar o agendamento
    private async void btnAddAgendamento_Clicked(object sender, EventArgs e)
    {
        //Inicia o serviço 
        StartService();  

        // Configura a notificação
        await VerifyPermissionsAsync();
    }

    //Dispara notificação
    private void ScheduleNotificationDay(DateTime agendamentoDateTime)
    {
        try
        {
            var notification = new NotificationRequest
            {
                NotificationId = 101,
                Title = "ALERTA!",
                Description = "Não esqueça do seu agendamento HOJE!",
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
            Console.WriteLine($"Erro ao agendar a notificação: {ex.Message}");
        }
    }

    //Verificar a permissão do usuario
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
                await App.Current.MainPage.DisplayAlert("Alerta!", "Permissão para notificações negada.", "OK");
                await DisplayAlert("Alerta", "Permissão para notificações NEGADA!", "OK");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Erro ao verificar permissões: {ex.Message}");
        }
    }

    //Botão de Cancelar/Retornar
    private async void btnCancelarAgendamento_Clicked(object sender, EventArgs e)
    {
        await Navigation.PopAsync();
    }

    //Entry com apenas letras
    private void OnLetterEntryTextChanged(object sender, TextChangedEventArgs e)
    {
        // Remove todos os caracteres que não sejam letras
        var entry = (Entry)sender;
        entry.Text = string.Concat(e.NewTextValue.Where(c => char.IsLetter(c) || char.IsWhiteSpace(c)));
    }

    //Entry com apenas números
    private void OnNumericEntryTextChanged(object sender, TextChangedEventArgs e)
    {
        var entry = (Entry)sender;
        entry.Text = string.Concat(e.NewTextValue.Where(char.IsDigit));
    }

    //Entry com letras, números e traços
    private void OnCustomEntryTextChanged(object sender, TextChangedEventArgs e)
    {
        var entry = (Entry)sender;
        string pattern = @"^[a-zA-Z0-9\s\-]*$"; // Permite letras, números, espaços e traços
        if (!Regex.IsMatch(e.NewTextValue, pattern))
        {
            // Reverte para o último valor válido
            entry.Text = e.OldTextValue;
        }
    }

    public void StartService()
    {
        if (!_serviceAndroid.IsRunning)
        {
            _serviceAndroid.Start(); // Inicia o serviço
        }
        else
        {
            Console.WriteLine("HOMEPAGE: O serviço já está em execução.");
        }

    }

    //FIM
}