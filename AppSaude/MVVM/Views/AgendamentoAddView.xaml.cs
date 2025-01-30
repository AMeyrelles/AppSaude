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

    //Bot�o para salvar o agendamento
    private async void btnAddAgendamento_Clicked(object sender, EventArgs e)
    {
        //Inicia o servi�o 
        StartService();  

        // Configura a notifica��o
        await VerifyPermissionsAsync();
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

    //Entry com apenas letras
    private void OnLetterEntryTextChanged(object sender, TextChangedEventArgs e)
    {
        // Remove todos os caracteres que n�o sejam letras
        var entry = (Entry)sender;
        entry.Text = string.Concat(e.NewTextValue.Where(c => char.IsLetter(c) || char.IsWhiteSpace(c)));
    }

    //Entry com apenas n�meros
    private void OnNumericEntryTextChanged(object sender, TextChangedEventArgs e)
    {
        var entry = (Entry)sender;
        entry.Text = string.Concat(e.NewTextValue.Where(char.IsDigit));
    }

    //Entry com letras, n�meros e tra�os
    private void OnCustomEntryTextChanged(object sender, TextChangedEventArgs e)
    {
        var entry = (Entry)sender;
        string pattern = @"^[a-zA-Z0-9\s\-]*$"; // Permite letras, n�meros, espa�os e tra�os
        if (!Regex.IsMatch(e.NewTextValue, pattern))
        {
            // Reverte para o �ltimo valor v�lido
            entry.Text = e.OldTextValue;
        }
    }

    public void StartService()
    {
        if (!_serviceAndroid.IsRunning)
        {
            _serviceAndroid.Start(); // Inicia o servi�o
        }
        else
        {
            Console.WriteLine("HOMEPAGE: O servi�o j� est� em execu��o.");
        }

    }

    //FIM
}