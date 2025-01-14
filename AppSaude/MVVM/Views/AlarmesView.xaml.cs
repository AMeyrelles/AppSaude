using AppSaude.MVVM.Models;
using AppSaude.MVVM.ViewModels;
using AppSaude.Services;
using Plugin.LocalNotification;
using Plugin.LocalNotification.AndroidOption;
using Plugin.Maui.Audio;

namespace AppSaude.MVVM.Views;

public partial class AlarmesView : ContentPage
{
    private readonly IService _service;
    private readonly IAudioManager _audioManager;

    IServiceAndroid _servicesAndroid;

    

    private List<Alarme> _alarmeList = new List<Alarme>();
   
    public AlarmesView(IService servico, IAudioManager audioManager, IServiceAndroid servicesAndroid)
    {
        InitializeComponent();

        this._audioManager = audioManager;

        _service = servico;

        _servicesAndroid = servicesAndroid;

        var viewModel = new MainViewModel(servico);
        BindingContext = viewModel;
    }

    //Carregar Lista de alarmes
    private async Task<List<Alarme>> LoadAlarmsFromDatabaseAsync()
    {
        try
        {
            // Busca todos os alarmes do banco de dados usando o servi�o
            var alarms = await _service.GetAlarmes();

            // Verifica se a lista retornada n�o � nula
            if (alarms == null)
            {
                Console.WriteLine("Nenhum alarme encontrado no banco de dados.");
                return new List<Alarme>();
            }

            Console.WriteLine($"Total de alarmes encontrados: {alarms.Count}");
            return alarms;
        }
        catch (Exception ex)
        {
            // Trata poss�veis erros ao carregar os dados
            Console.WriteLine($"Erro ao carregar alarmes: {ex.Message}");
            return new List<Alarme>();
        }
    }

    // Atualiza os dados na tela toda vez que for acessada
    private Timer _timer;
    protected override async void OnAppearing()
    {
        base.OnAppearing();

        // Carrega os alarmes do banco e armazena na lista
        _alarmeList = await LoadAlarmsFromDatabaseAsync();

        // Exibe a quantidade de alarmes carregados
        Console.WriteLine($"Alarmes carregados: {_alarmeList.Count}");
        
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
        _timer = new Timer(CheckAlarms, null, TimeSpan.Zero, TimeSpan.FromMinutes(1));
    }

    private async void CheckAlarms(object state)
    {
        try
        {
            // Obt�m o hor�rio atual
            DateTime now = DateTime.Now;

            // Carrega os alarmes do banco e armazena na lista
            var alarmeList = await LoadAlarmsFromDatabaseAsync();

            foreach (var alarm in alarmeList)
            {
                // Verifica se o hor�rio atual coincide com o hor�rio do alarme
                if (now.Hour == alarm.ReminderTime.Hours && now.Minute == alarm.ReminderTime.Minutes)
                {
                    // Dispara notifica��o, som e navega para a tela de alarme
                    await OnAudioTriggered();
                    await ScheduleAlarmAsync(alarm.ReminderTime);                
                   
                    break; // Se encontrou o alarme, n�o precisa continuar verificando os outros
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Erro ao verificar alarmes: {ex.Message}");
        }
    }

    //Dispara a notificacao
    private async Task ScheduleAlarmAsync(TimeSpan reminderTime)
    {
        try
        {
            // Define a hora exata do alarme para hoje
            DateTime alarmDateTime = DateTime.Now.Date.Add(reminderTime);

            var notification = new NotificationRequest
            {
                NotificationId = 100, // ID �nico para identificar a notifica��o
                Title = "Lembrete de Rem�dio",
                Description = "� hora de tomar seu rem�dio!",
                Schedule = new NotificationRequestSchedule
                {
                    NotifyTime = alarmDateTime,
                    RepeatType = NotificationRepeat.Daily // Repete diariamente (se necess�rio)
                },
                Android = new Plugin.LocalNotification.AndroidOption.AndroidOptions
                {
                    AutoCancel = true,
                    IconSmallName = { ResourceName = "bell.png" }
                }
            };

            await LocalNotificationCenter.Current.Show(notification);
            Console.WriteLine("Notifica��o agendada com sucesso!");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Erro ao agendar a notifica��o: {ex.Message}");
        }
    }


    //Disparo para o som de notifica��o
    private async Task OnAudioTriggered()
    {
        var player = _audioManager.CreatePlayer(await FileSystem.OpenAppPackageFileAsync("ola_esta_na_hora_de_tomar_o_seu_medicamento_tom.mp3"));
        player.Play();
        player.PlaybackEnded += (sender, e) =>
        {
            player.Play();
            player.Dispose();
        };
    }

    //Conta quantos alarmes foram carregados
    private async Task ShowAlarmCountAsync()
    {
        await DisplayAlert("Informa��o", $"Alarmes atuais: {_alarmeList.Count}", "OK");
    }

    // Bot�o para navegar para AlarmeAddView
    private async void btnAdd_Clicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new AlarmeAddView(_service, _servicesAndroid));
    }

}
