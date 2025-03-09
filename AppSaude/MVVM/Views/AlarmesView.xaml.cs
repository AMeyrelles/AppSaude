using AppSaude.MVVM.Models;
using AppSaude.MVVM.ViewModels;
using AppSaude.Services;
using Plugin.LocalNotification;
using Plugin.Maui.Audio;

namespace AppSaude.MVVM.Views;

public partial class AlarmesView : ContentPage
{
    private readonly IService _service;
    private readonly IAudioManager _audioManager;
    IServiceAndroid _servicesAndroid;    

    private List<Alarme> _alarmeList = new();
   
    public AlarmesView(IService servico, IServiceAndroid servicesAndroid , IAudioManager audioManager)
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
            // Busca todos os alarmes do banco de dados usando o serviço
            var alarms = await _service.GetAlarmes();

            // Verifica se a lista retornada não é nula
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
            // Trata possíveis erros ao carregar os dados
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
            // O BindingContext é da ViewModel que contém o DisplayCommand
            if (BindingContext is MainViewModel viewModel)
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
        _timer = new Timer(CheckAlarms, null, TimeSpan.Zero, TimeSpan.FromSeconds(30));
    }

    //Limpa o timer ao sair da tela
    //protected override void OnDisappearing()
    //{
    //    base.OnDisappearing();

    //    // Para o timer ao sair da tela
    //    _timer?.Dispose();
    //    _timer = null;
    //}

    private async void CheckAlarms(object state)
    {
        try
        {
            // Obtém a data e o horário atual
            DateTime now = DateTime.Now;

            // Carrega os alarmes do banco e armazena na lista
            var alarmeList = await LoadAlarmsFromDatabaseAsync();

            foreach (var alarm in alarmeList)
            {
                Console.WriteLine($"Verificando alarmes às {now:HH:mm}");

                // Verifica se o alarme está desativado
                if (!alarm.IsEnabled)
                {
                    Console.WriteLine($"Alarme '{alarm.MedicationName}' está desativado e será ignorado.");
                    continue; // Ignora este alarme e passa para o próximo
                }

                // Verifica se o alarme já foi notificado hoje
                if (alarm.LastNotifiedDate.HasValue && alarm.LastNotifiedDate.Value.Date == now.Date)
                {
                    Console.WriteLine($"Alarme '{alarm.MedicationName}' as {alarm.ReminderTime} já foi notificado hoje. Ignorando.");

                    continue; // Ignora este alarme
                }


                // Verifica se o horário atual coincide com o horário do alarme
                if (now.Hour == alarm.ReminderTime.Hours && now.Minute == alarm.ReminderTime.Minutes)
                {
                    // Marca o alarme como notificado para hoje
                    alarm.LastNotifiedDate = now;

                    // Dispara notificação, som e navega para a tela de alarme
                    await OnAudioTriggered();
                    await ScheduleAlarmAsync(alarm.ReminderTime);
                    Console.WriteLine($"Alarme encontrado: {alarm.MedicationName} às {alarm.ReminderTime}");

                    // Atualiza o alarme no banco
                    await _service.UpdateAlarme(alarm);
                    break; // Se encontrou o alarme, não precisa continuar verificando os outros
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
                NotificationId = 100, // ID único para identificar a notificação
                Title = "Lembrete de Remédio",
                Description = "É hora de tomar seu remédio!",
                Schedule = new NotificationRequestSchedule
                {
                    NotifyTime = alarmDateTime,
                    RepeatType = NotificationRepeat.Daily // Repete diariamente (se necessário)
                },
                Android = new Plugin.LocalNotification.AndroidOption.AndroidOptions
                {
                    AutoCancel = true,
                    IconSmallName = { ResourceName = "icon_mais_.svg" }
                }
            };

            await LocalNotificationCenter.Current.Show(notification);
            Console.WriteLine("Notificação agendada com sucesso!");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Erro ao agendar a notificação: {ex.Message}");
        }
    }


    //Disparo para o som de notificação
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
        await DisplayAlert("Informação", $"Alarmes atuais: {_alarmeList.Count}", "OK");
    }

    // Botão para navegar para AlarmeAddView
    private async void btnAdd_Clicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new AlarmeAddView(_service, _servicesAndroid));
    }

    //Switch para habilitar/desabilitar alarme
    private async void AlarmSwitch_Toggled(object sender, ToggledEventArgs e)
    {
        var switchControl = sender as Switch;

        if (switchControl?.BindingContext is Alarme alarme)
        {
            alarme.IsEnabled = e.Value;
            try
            {
                if (alarme.IsEnabled)
                {                    
                    await _service.UpdateAlarme(alarme); // Atualiza o banco de dados
                    Console.WriteLine($"Alarme '{alarme.MedicationName}' habilitado: {alarme.IsEnabled}");
                    return;
                }
                else
                {
                    alarme.IsEnabled = false;
                    await _service.UpdateAlarme(alarme); // Atualiza o banco de dados
                    await DisplayAlert("Alerta!", "Você tem alarmes inativos, verifique se é isto mesmo!", "OK");
                    Console.WriteLine($"Alarme '{alarme.MedicationName}' desativado: {alarme.IsEnabled}");
                }          
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro ao atualizar o alarme: {ex.Message}");
            }
        }
        else
        {
            await DisplayAlert("Alerta!", "Alarme OFF", "OK");
            Console.WriteLine("Erro: BindingContext inválido para o Switch.");
        }
    }
}
