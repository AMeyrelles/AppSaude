using AppSaude.MVVM.Models;
using AppSaude.MVVM.ViewModels;
using AppSaude.Services;
using Plugin.LocalNotification;
using Plugin.Maui.Audio;

namespace AppSaude.MVVM.Views;

public partial class AgendamentosView : ContentPage
{
	private readonly IService _service;

    private readonly IAudioManager  _audioManager;

    private List<Agendamento> _agendamentoList = new();
    public AgendamentosView(IService services, IAudioManager audioManager)
    {
        InitializeComponent();

        _audioManager = audioManager;

        _service = services;

        var viewModel = new MainViewModel(services);

        BindingContext = viewModel;
    }

    //Carregar Lista de agendamentos
    private async Task<List<Agendamento>> LoadAgendamentoFromDatabaseAsync()
    {
        try
        {
            // Busca todos os agendamentos do banco de dados usando o servi�o
            var agendamento = await _service.GetAgendamentos();

            // Verifica se a lista retornada n�o � nula
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
            // Trata poss�veis erros ao carregar os dados
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
            // O BindingContext � da ViewModel que cont�m o DisplayCommand
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

        // Configura o timer para chamar o m�todo de verifica��o a cada 1 minuto
        _timer = new Timer(CheckAgendamentos, null, TimeSpan.Zero, TimeSpan.FromMinutes(1));
    }

    private async void CheckAgendamentos(object state)
    {
        try
        {
            // Obt�m o hor�rio atual
            DateTime now = DateTime.Now;

            // Carrega os alarmes do banco e armazena na lista
            var _agendamentoList = await LoadAgendamentoFromDatabaseAsync();

            foreach (var agendamento in _agendamentoList)
            {
                // Verifica se o hor�rio atual coincide com o hor�rio do alarme
                if (now.Hour == agendamento.AppointmentDateTime.Hours && now.Minute == agendamento.AppointmentDateTime.Minutes)
                {
                    // Dispara notifica��o, som e navega para a tela de alarme
                    await OnAudioTriggered();
                    await ScheduleAgendamentoAsync(agendamento.AppointmentDateTime);

                    break; // Se encontrou o alarme, n�o precisa continuar verificando os outros
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
            // Cria um DateTime combinando a data atual com o hor�rio do alarme
            DateTime alarmDateTime = DateTime.Now.Date.Add(reminderTime); // Cria a data e hora completa

            var notification = new NotificationRequest
            {
                NotificationId = 100,
                Title = "Lembrete de Rem�dio",
                Description = "� hora de tomar seu rem�dio!",
                Schedule = new NotificationRequestSchedule
                {
                    NotifyTime = alarmDateTime // Usa o DateTime com a data de hoje e o hor�rio do alarme
                },
                //Sound = "careless_whisper.mp3", // Caminho para o arquivo de som da notifica��o
                Android = new Plugin.LocalNotification.AndroidOption.AndroidOptions
                {
                    AutoCancel = true,
                    IconSmallName = { ResourceName = "appicon.svg" }
                }
            };
            await LocalNotificationCenter.Current.Show(notification);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Erro ao agendar a notifica��o: {ex.Message}");
        }
    }

    //Disparo para o som de notifica��o
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
        await DisplayAlert("Informa��o", $"Agendamentos atuais: {_agendamentoList.Count}", "OK");
    }

    private async void btnAdd_Clicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new AgendamentoAddView(_service));
    }
}