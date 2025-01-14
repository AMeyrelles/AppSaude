using AppSaude.MVVM.ViewModels;
using AppSaude.Services;
using Plugin.Maui.Audio;
using AppSaude.MVVM.Models;

namespace AppSaude.MVVM.Views;

public partial class HomePageView : ContentPage
{
	private readonly IService _services;    
    private readonly IAudioManager _audioManager;

    IServiceAndroid _servicesAndroid;

    private List<Alarme> _alarmeList = new();

    private List<Agendamento> _agendamentoList = new();
    public HomePageView(IService services, IServiceAndroid servicesAndroid, IAudioManager audioManager)
    {
        InitializeComponent();

        _services = services;
        _servicesAndroid = servicesAndroid;
        _audioManager = audioManager;

        var viewModel = new MainViewModel(services);

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

    //Carregar os dados ao carregar a tela
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
        try
        {
            // Obtém o horário atual
            DateTime now = DateTime.Now;

            // Carrega os alarmes do banco e armazena na lista
            var _alarmeList = await LoadAlarmsFromDatabaseAsync();

            foreach (var alarm in _alarmeList)
            {
                // Verifica se o horário atual coincide com o horário do alarme
                if (now.Hour == alarm.ReminderTime.Hours && now.Minute == alarm.ReminderTime.Minutes)
                {
                    var alarmBorder = this.FindByName<HorizontalStackLayout>("alarmBorder");    // Altere a cor do Border

                    alarmBorder.BackgroundColor = Colors.Red;


                    break; // Se encontrou o alarme, não precisa continuar verificando os outros
                }
                else
                {  // Localize o Border no layout (supondo que você tenha um Border com o nome "alarmVSL")
                    var alarmVSL = this.FindByName<HorizontalStackLayout>("alarmBorder");    // Altere a cor do Border

                    alarmVSL.BackgroundColor = Colors.LightSteelBlue;
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Erro ao verificar alarmes: {ex.Message}");
        }
    }


    //Button de navegacao para AlarmesView
    private async void btnAlarme_Clicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new AlarmesView(_services, _audioManager, _servicesAndroid));
    }

    private async void btnAgendamentos_Clicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new AgendamentosView(_services, _audioManager));
    }


    //Conta quantos alarmes foram carregados
    //private async Task ShowAlarmCountAsync()
    //{
    //    await DisplayAlert("Informação", $"Alarmes atuais: {_alarmeList.Count}", "OK");
    //}

    //Falta implementar as funcionalidades

    //Btn Maps
    private async void btnMaps_Clicked(object sender, EventArgs e)
    {
        await DisplayAlert("ALERTA!", "Em breve a funcionalidade estará disponivel", "OK");
    }

    //Btn Notification
    private async void btnNoticacao_Clicked(object sender, EventArgs e)
    {
        await DisplayAlert("ALERTA!", "Em breve a funcionalidade estará disponivel", "OK");
    }
}