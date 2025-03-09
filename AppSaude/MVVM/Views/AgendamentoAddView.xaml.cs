using AppSaude.MVVM.ViewModels;
using AppSaude.Services;
using Plugin.LocalNotification;
using System.Text.RegularExpressions;

namespace AppSaude.MVVM.Views;

public partial class AgendamentoAddView : ContentPage
{
<<<<<<< HEAD
	private readonly IServicesTeste _services;
    private readonly IServiceAndroid _serviceAndroid;

    public AgendamentoAddView(IServicesTeste services, IServiceAndroid servicesAndroid)
	{
        InitializeComponent();
        _services = services ?? throw new ArgumentNullException(nameof(services));
        _serviceAndroid = servicesAndroid ?? throw new ArgumentNullException(nameof(servicesAndroid));
=======
	private readonly IService _services;

    private readonly List<DateTime> _agendamentoList = new List<DateTime>();
    public AgendamentoAddView(IService services)
	{
        InitializeComponent();
        _services = services;
>>>>>>> Primeira_Branch

        var viewModel = new AgendamentoViewModel(services);
        BindingContext = viewModel;

        // Define o MinimumDate como a data atual
        datePickerControl.MinimumDate = DateTime.Today;
    }   

    //Botão para salvar o agendamento
    private async void btnAddAgendamento_Clicked(object sender, EventArgs e)
    {
<<<<<<< HEAD
        //Inicia o serviço 
        StartService();  

        // Configura a notificação
        await VerifyPermissionsAsync();        
=======
        // Obtém a data e a hora selecionadas
        DateTime selectedDate = datePickerControl.Date;
        TimeSpan selectedTime = timePickerControl.Time;

        // Combina a data e a hora
        DateTime agendamentoDateTime = selectedDate.Date.Add(selectedTime);

        // Verifica se a data selecionada é no passado
        if (selectedDate < DateTime.Now.Date)
        {
            await DisplayAlert("Erro", "A data selecionada não pode ser no passado.", "OK");
            return;
        }

        //Dispara notificação
        if (selectedDate.Date == agendamentoDateTime.Date)
        {
            var notification = new NotificationRequest
            {
                NotificationId = 102,
                Title = "ALERTA!",
                Description = "Você tem um agendamento marcado HOJE!",
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

        // Adiciona o agendamento à lista (opcional)
        _agendamentoList.Add(selectedDate);

        // Configura a notificação
        await VerifyPermissionsAsync();
        ScheduleNotificationDay(selectedDate);
>>>>>>> Primeira_Branch
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
<<<<<<< HEAD
        string pattern = @"^[\p{L}0-9\s\-]*$"; // Permite letras, números, espaços , traços e acentos
=======
        string pattern = @"^[a-zA-Z0-9\s\-]*$"; // Permite letras, números, espaços e traços
>>>>>>> Primeira_Branch
        if (!Regex.IsMatch(e.NewTextValue, pattern))
        {
            // Reverte para o último valor válido
            entry.Text = e.OldTextValue;
        }
    }
<<<<<<< HEAD

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
=======
>>>>>>> Primeira_Branch
}