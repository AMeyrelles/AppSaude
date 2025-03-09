using AppSaude.MVVM.ViewModels;
using AppSaude.Services;
using Plugin.Maui.Audio;
using AppSaude.MVVM.Models;


namespace AppSaude.MVVM.Views;

public partial class HomePageView : ContentPage 
{
	private readonly IServicesTeste _services;    
    private readonly IAudioManager _audioManager;
    private readonly IServiceAndroid _servicesAndroid;
    private readonly IAlarmService _alarmService;

    private List<Alarme> _alarmeList = new();

    public HomePageView(IServicesTeste services, IServiceAndroid servicesAndroid, IAudioManager audioManager, IAlarmService alarmService)
    {
        InitializeComponent();

        // Atribuir dependências injetadas
        _services = services ?? throw new ArgumentNullException(nameof(services));
        _audioManager = audioManager ?? throw new ArgumentNullException(nameof(audioManager));
        _servicesAndroid = servicesAndroid ?? throw new ArgumentNullException(nameof(servicesAndroid));
        _alarmService = alarmService ?? throw new ArgumentNullException(nameof(alarmService));
        var viewModel = new MainViewModel(_services);

        BindingContext = viewModel;

    }

    //Carregar Lista de alarmes
    private async Task<List<Alarme>> LoadAlarmsFromDatabaseAsync()
    {
        try
        {
            // Busca todos os alarmes do banco de dados usando o serviço
            var alarms = await _services.GetAlarmes();

            // Verifica se a lista retornada não é nula
            if (alarms == null)
            {
                Console.WriteLine("Nenhum alarme encontrado no banco de dados.");
                return new List<Alarme>();
            }

            //Console.WriteLine($"HOMEPAGE - de alarmes encontrados: {alarms.Count}");
            return alarms;
        }
        catch (Exception ex)
        {
            //Trata possíveis erros ao carregar os dados
            Console.WriteLine($"HOMEPAGE - Erro ao carregar alarmes: {ex.Message}");
            return new List<Alarme>();
        }
    }

    //CARREGA A LIS DE NOTIFICACOES
    private async Task<List<NotificacaoAlarme>> LoadNotificacaoFromDatabaseAsync()
    {
        try
        {
            // Busca todos os alarmes do banco de dados usando o serviço
            var notificacao = await _services.GetNotAlarmes();

            // Verifica se a lista retornada não é nula
            if (notificacao == null)
            {
                Console.WriteLine("NOTIFICACAOVIEW : Nenhum notificacao de alarme encontrado no banco de dados.");
                return new List<NotificacaoAlarme>();
            }

            Console.WriteLine($"NOTIFICACAOVIEW : Total de notificação de alarmes encontrados: {notificacao.Count}");
            return notificacao;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"NOTIFICACAOVIEW : Erro ao carregar alarmes: {ex.Message}");
            return new List<NotificacaoAlarme>();
        }
    }

    //Carregar os dados ao carregar a tela
    private Timer _timer;
    protected override async void OnAppearing()
    {
        base.OnAppearing();
        StartService();

        // Carrega os alarmes do banco e armazena na lista
        _alarmeList = await LoadAlarmsFromDatabaseAsync();

        // Exibe a quantidade de alarmes carregados
        Console.WriteLine($"HOMEPAGE: Alarmes carregados: {_alarmeList.Count}");

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
    protected override void OnDisappearing()
    {
        base.OnDisappearing();

        // Para o timer ao sair da tela
        _timer?.Dispose();
        _timer = null;
    }

    private async void CheckAlarms(object state)
    {
        _ = _alarmService.CheckAlarms();
        try
        {
            
            // Obtém o horário atual
            DateTime now = DateTime.Now;

            // Carrega os alarmes do banco e armazena na lista
            var _alarmeList = await LoadAlarmsFromDatabaseAsync();
            var _notificacaoList = await LoadNotificacaoFromDatabaseAsync();
            var alarms = await _services.GetAlarmes();
            

            if (alarms == null || !alarms.Any())
            {
                Console.WriteLine("HOMEPAGE: Nenhum alarme encontrado.");
                return;
            }

            foreach (var alarm in alarms)
            {
                //if (alarm.LastNotifiedDate.HasValue && alarm.LastNotifiedDate.Value.Date == now.Date) continue;

                if (alarm.IsNotified)
                {
                    var lblLabel = this.FindByName<Label>("lblNameAlarme"); // Altere a cor do Border

                    if (lblLabel != null)
                    {
                        lblLabel.BackgroundColor = Colors.Green;
                    }
                    else
                    {
                        Console.WriteLine("Border 1 'lblNameAlarme' não encontrado.");
                    }
                    continue;
                }

                if (!alarm.IsNotified)
                {
                    // Encontra o Border pelo nome definido no XAML
                    var lblLabel = this.FindByName<Label>("lblNameAlarme");

                    if (lblLabel != null)
                    {
                        // Altera a cor do Border usando Color.FromArgb para o valor hexadecimal
                        lblLabel.BackgroundColor = Color.FromArgb("#195986");
                    }
                    else
                    {
                        // Log ou tratamento de erro caso o Border não seja encontrado
                        Console.WriteLine("Border 2 'lblNameAlarme' não encontrado.");
                    }
                    continue;
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"HOMEPAGE: Erro ao verificar alarmes: {ex.Message}");
        }
        
    }


    //Button de navegacao para AlarmesView
    private async void btnAlarme_Clicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new AlarmesView(_services, _servicesAndroid));
    }

    private async void btnAgendamentos_Clicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new AgendamentosView(_services, _audioManager, _servicesAndroid));
    }

    //Falta implementar as funcionalidades

    //Btn Maps
    private async void btnMaps_Clicked(object sender, EventArgs e)
    {
        await DisplayAlert("ALERTA!", "Em breve a funcionalidade estará disponivel", "OK");
    }

    //Btn Notification
    private async void btnNotificacao_Clicked(object sender, EventArgs e)
    {
        //await DisplayAlert("ALERTA!", "Em breve a funcionalidade estará disponivel", "OK");
        await Navigation.PushAsync(new NotificacaoView(_services));
    }

    //Inicia o serviço em primeiro Plano

    public  void StartService()
    {
        if (!_servicesAndroid.IsRunning)
        {
             _servicesAndroid.Start(); // Inicia o serviço
        }
        else
        {
            Console.WriteLine("HOMEPAGE: O serviço já está em execução.");
        }

    }
}