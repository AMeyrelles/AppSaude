using AppSaude.MVVM.Models;
using AppSaude.MVVM.ViewModels;
using AppSaude.Services;
using Plugin.LocalNotification;
using Plugin.Maui.Audio;
using System.Security.Claims;

namespace AppSaude.MVVM.Views;

public partial class AgendamentosView : ContentPage
{
    private readonly IServicesTeste _services;
    private readonly IServiceAndroid _serviceAndroid;
    private readonly IAudioManager _audioManager;

    private List<Agendamento> _agendamentoList = new();    
    public AgendamentosView(IServicesTeste services, IAudioManager audioManager, IServiceAndroid servicesAndroid)
    {
        InitializeComponent();

        _audioManager = audioManager;

        _services = services ?? throw new ArgumentNullException(nameof(services));
        _serviceAndroid = servicesAndroid ?? throw new ArgumentNullException(nameof(servicesAndroid));

        var viewModel = new MainViewModel(services);

        BindingContext = viewModel;
    }

    //Carregar Lista de agendamentos
    private async Task<List<Agendamento>> LoadAgendamentoFromDatabaseAsync()
    {
        try
        {
            // Busca todos os agendamentos do banco de dados usando o serviço
            var agendamento = await _services.GetAgendamentos();

            // Verifica se a lista retornada não é nula
            if (agendamento == null)
            {
                Console.WriteLine("Nenhum agendamento encontrado no banco de dados.");
                return new List<Agendamento>();
            }

            Console.WriteLine($"Total de agendamento encontrados: {agendamento.Count}");
            return agendamento;
        }
        catch (Exception ex)
        {
            // Trata possíveis erros ao carregar os dados
            Console.WriteLine($"Erro ao carregar agendamento: {ex.Message}");
            return new List<Agendamento>();
        }
    }

    // Atualiza os dados na tela toda vez que for acessada
    private Timer _timer;
    protected override async void OnAppearing()
    {
        base.OnAppearing();

        // Carrega os agendamentos do banco e armazena na lista
        _agendamentoList = await LoadAgendamentoFromDatabaseAsync();

        // Exibe a quantidade de agendamentos carregados
        Console.WriteLine($"agendamento carregados: {_agendamentoList.Count}");

        try
        {
            // O BindingContext é da ViewModel que contém o DisplayCommand
            var viewModel = BindingContext as MainViewModel;
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

        // Configura o timer para chamar o método de verificação a cada 1 minuto
        _timer = new Timer(CheckAgendamentos, null, TimeSpan.Zero, TimeSpan.FromMinutes(1));
    }

    private async void CheckAgendamentos(object state)
    {
        try
        {
            // Obtém o horário atual
            var now = DateTime.Now;
            var currentTime = new DateTime(now.Year, now.Month, now.Day, now.Hour, now.Minute, 0);

            // Carrega os agendamentos do banco e armazena na lista
            var _agendamentoList = await LoadAgendamentoFromDatabaseAsync();

            foreach (var agendamento in _agendamentoList)
            {              
                // Verifica se o horário atual coincide com o horário do agendamento - OK
                if (!agendamento.IsNotified
                    && now.Month == agendamento.SelectedDate.Month
                    && now.Day == agendamento.SelectedDate.Day
                    && now.Hour == agendamento.AppointmentDateTime.Hours
                    && now.Minute == agendamento.AppointmentDateTime.Minutes)
                {
                    agendamento.IsNotified = true;
                    agendamento.IsEnabled = true;                    

                    // Dispara notificação e som 
                    await OnAudioTriggered();
                    await ScheduleAgendamentoAsync(agendamento.AppointmentDateTime);

                    await _services.UpdateAgendamento(agendamento); // Atualiza o banco                    

                    Console.WriteLine($"Notificando: {agendamento.SpecialistName} em " +
                        $"{agendamento.SelectedDate.Hour}:{agendamento.SelectedDate.Minute}");
                }
                else if (agendamento.IsNotified)
                {
                    Console.WriteLine($"Agendamento já notificado!{agendamento.SpecialistName}, {agendamento.SelectedDate}");
                }

                if (!agendamento.IsNotified
                    && now.Month == agendamento.SelectedDate.Month
                    && now.Day == agendamento.SelectedDate.Day
                    && agendamento.NotificationDailyCount == 0)
                {
                    agendamento.NotificationDailyCount = 1;
                    agendamento.LastNotifiedDate = DateTime.Now;

                    await ScheduleDailyReminderAsync(agendamento);                  

                    Console.WriteLine($"Notificando: {agendamento.SpecialistName} em " +
                                      $"{agendamento.AppointmentDateTime.Hours}:" +
                                      $"{agendamento.AppointmentDateTime.Minutes}");

                    await _services.UpdateAgendamento(agendamento); // Atualiza o banco                   
                }
                // Verifica se já se passaram 24 horas desde a última notificação
                if (agendamento.LastNotifiedDate.HasValue &&
                    DateTime.Now >= agendamento.LastNotifiedDate.Value.AddHours(24))
                {
                    // Reseta o contador após 24 horas
                    agendamento.NotificationDailyCount = 0;
                    agendamento.LastNotifiedDate = null; // Opcional: limpa a data de notificação
                    Console.WriteLine("Contador resetado após 24 horas.");
                }
                else
                {
                    Console.WriteLine($"Agendamento já notificado HOJE! {agendamento.SpecialistName}, Contador: {agendamento.NotificationDailyCount}");
                }

                // Verifica se o horário atual coincide com o horário do agendamento - OK
                if (agendamento.IsNotified && agendamento.IsEnabled
                    && now.Month == agendamento.SelectedDate.Month
                    && now.Day == agendamento.SelectedDate.Day
                    && now.Minute == agendamento.AppointmentDateTime.Minutes)
                {
                    var notificacaoAgendamento = new NotificacaoAgendamento
                    {
                        SpecialistNameNAg = agendamento.SpecialistName,
                        SpecialtyNAg = agendamento.Specialty,
                        PostalCodeNAg = agendamento.PostalCode,
                        StreetNAg = agendamento.Street,
                        NeighborhoodNAg = agendamento.Neighborhood,
                        CityNAg = agendamento.City,
                        AppointmentDateTimeNAg = agendamento.AppointmentDateTime,
                        SelectedDateNAg = agendamento.SelectedDate,
                        MinDate = agendamento.MinDate,
                        DescriptionAppointmentsNAg = agendamento.DescriptionAppointments,
                        IsEnabledNAg = agendamento.IsEnabled,
                        IsNotifiedNAg = agendamento.IsNotified
                    };
                                
                    agendamento.IsEnabled = false;
                    await _services.AddNotAgendamento(notificacaoAgendamento);
                    await _services.UpdateAgendamento(agendamento);
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Erro ao verificar agendamentos: {ex.Message}");
        }
    }


    //Dispara a notificacao
    private async Task ScheduleAgendamentoAsync(TimeSpan reminderTime)
    {
        try
        {
            // Cria um DateTime combinando a data atual com o horário do alarme
            DateTime alarmDateTime = DateTime.Now.Date.Add(reminderTime); // Cria a data e hora completa

            var notification = new NotificationRequest
            {
                NotificationId = 111,
                Title = "Lembrete do agendamento!",
                Description = "Agendamento marcado para HOJE!!!",
                Schedule = new NotificationRequestSchedule
                {
                    NotifyTime = alarmDateTime // Usa o DateTime com a data de hoje e o horário do alarme
                },
                //Sound = "careless_whisper.mp3", // Caminho para o arquivo de som da notificação
                Android = new Plugin.LocalNotification.AndroidOption.AndroidOptions
                {
                    AutoCancel = true,
                    IconSmallName = { ResourceName = "sino" }
                }
            };
            await LocalNotificationCenter.Current.Show(notification);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Erro ao agendar a notificação: {ex.Message}");
        }
    }

    //Dispara a notificacao
    private async Task ScheduleDailyReminderAsync(Agendamento _agendamento)
    {
        try
        {
            var notification = new NotificationRequest
            {
                NotificationId = _agendamento.AppointmentsId,
                Title = "Lembrete do agendamento!",
                Description = $"Você tem um agendamento marcado para HOJE : {_agendamento.Specialty}!!!",
                Schedule = new NotificationRequestSchedule
                {
                    NotifyTime = DateTime.Now.AddMinutes(1) 
                },
                Android = new Plugin.LocalNotification.AndroidOption.AndroidOptions
                {
                    AutoCancel = true,
                    IconSmallName = { ResourceName = "sino" }
                }
            };
            await LocalNotificationCenter.Current.Show(notification);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Erro ao agendar a notificação: {ex.Message}");
        }
    }

    //Disparo para o som de notificação
    private async Task OnAudioTriggered()
    {
        var player = _audioManager.CreatePlayer(await FileSystem.OpenAppPackageFileAsync("ola_voce_tem_um_agendamento_marcado_para_hoje_tom2.mp3"));
        player.Play();
        player.PlaybackEnded += (sender, e) =>
        {
            player.Play();
            player.Dispose();
        };
    }

    //Conta quantos agendamentos foram carregados
    private async Task ShowAgendamentoCountAsync()
    {
        await DisplayAlert("Informação", $"Agendamentos atuais: {_agendamentoList.Count}", "OK");
    }

    private async void btnAdd_Clicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new AgendamentoAddView(_services, _serviceAndroid));
    }
}